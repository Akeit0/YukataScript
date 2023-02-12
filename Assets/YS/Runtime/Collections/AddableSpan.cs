using System;

namespace YS.Collections {
    public ref struct AddableSpan<T> {
        Span<T> _span;
        int _size;
        public int Count => _size;
        public int Capacity => _span.Length;
        public Span<T> AsSpan() => _span[.._size];
        
        public AddableSpan(Span<T> span) {
            _span = span;
            _size = 0;
        }

        public void Add(T item) {
            if(_span.Length==_size) {
                throw new ArgumentOutOfRangeException();
            }
            _span[_size++] = item;
            
        }
        public void AddRange(ReadOnlySpan<T> span) {
            var newSize = _size + span.Length;
            if(_span.Length<=newSize) {
                throw new ArgumentOutOfRangeException();
            }
            span.CopyTo(_span.Slice(_size,span.Length));
            _size = newSize;
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