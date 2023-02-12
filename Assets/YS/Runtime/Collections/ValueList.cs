using System;
using System.Buffers;

namespace YS.Collections {
    public ref struct ValueList<T> {
        [ThreadStatic]
        private static ArrayWrapper<T>  ArrayWrapper;
        public int Count => _size;
        public int Capacity => _span.Length;
        private T[] _toReturn;
        private Span<T> _span;
        private int _size;
        private bool _useStatic;
        public ValueList(Span<T> span) {
            _span = span;
            _toReturn = null;
            _size = 0;
            _useStatic = false;
        }
        public ValueList(int capacity) {
            if (ArrayWrapper.IsRented) {
                _toReturn = ArrayPool<T>.Shared.Rent(capacity);
                _span = _toReturn.AsSpan();
                _size = 0;
                _useStatic = false;
            }
            else {
                ArrayWrapper.EnsureCapacity(capacity);
                _span = ArrayWrapper.Array.AsSpan();
                ArrayWrapper.IsRented = true;
                _size = 0;
                _toReturn = null;
                _useStatic = true;
            }
        }
        public Span<T> AsSpan() => _span[.._size];
        public void Add(T item) {
            if(_span.Length==_size) {
                if (!_useStatic&&ArrayWrapper.IsRented) {
                    _toReturn?.ReturnToPool();
                    _toReturn = ArrayPool<T>.Shared.Rent(_size * 2);
                    var nextSpan  = _toReturn.AsSpan();
                    _span.CopyTo(nextSpan.Slice(0,_span.Length));
                    _span = nextSpan;
                }
                else {
                    ArrayWrapper.EnsureCapacity(_size*2+1);
                    var nextSpan = ArrayWrapper.Array.AsSpan();
                    _span.CopyTo(nextSpan.Slice(0,_span.Length));
                    _span = nextSpan;
                    ArrayWrapper.IsRented = true;
                }
            }
            _span[_size++] = item;
            
        }
        public void AddRange(ReadOnlySpan<T> span) {
            var newSize = _size + span.Length;
            if(_span.Length<=newSize) {
                if (!_useStatic&&ArrayWrapper.IsRented) {
                    _toReturn?.ReturnToPool();
                    _toReturn = ArrayPool<T>.Shared.Rent(newSize);
                    var nextSpan  = _toReturn.AsSpan();
                    _span.CopyTo(nextSpan.Slice(0,_span.Length));
                    _span = nextSpan;
                }
                else {
                    ArrayWrapper.EnsureCapacity(newSize);
                    var nextSpan = ArrayWrapper.Array.AsSpan();
                    _span.CopyTo(nextSpan.Slice(0,_span.Length));
                    _span = nextSpan;
                    ArrayWrapper.IsRented = true;
                }
            }
            span.CopyTo(_span.Slice(_size,span.Length));
            _size = newSize;
        }

        public ref T this[int index] {
            get {
                if (_size < index) throw new IndexOutOfRangeException();
                return ref _span[index];
            }
        }
        public void Dispose() {
            _span[.._size].Clear();
            if (_useStatic) {
                ArrayWrapper.IsRented = false;
            }
            else _toReturn?.ReturnToPool();
            
        }
        public  void Clear() {
            _size = 0;
            AsSpan().Clear();
        }

        public override string ToString() {
            return AsSpan().ToString();
        }

    }
}