using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YS.Collections {
    public struct ListLike<T> {
        public  T[] GetArray() => _items;
        T[] _items;
        public int Count => _size;
        int _size;
        public int Capacity => _items.Length;

        public ListLike(int capacity=4) {
            _items = new T[capacity];
            _size = 0;
        }
        public ListLike(T[] array) {
            _items = array;
            _size = 0;
        }

        public static ListLike<T> NewShallowCopy(List<T> list) => new (list);
        ListLike(List<T> list) {
            _items = list.GetArray();
            _size = list.Count;
        }
        
        public ref T this[int index] {
            get {
                if (_size <=index) {
                    throw new IndexOutOfRangeException();
                }
                return ref _items[index];
            }
        }

      
        public  void Add(T value) {
            if(Capacity==_size) {
                Array.Resize(ref _items,_size*2);
            }
            _items[_size++] = value;
        }


        public T Pop() {
            var result = _items[--_size];
            _items[_size] = default;
            return result;
        }

        public bool TryPop(out T value) {
            if (0 < _size) {
                value = _items[--_size];
                return true;
            }
            value = default;
            return false;
        }
        public void RemoveAt(int index) {
            if ((uint)index >= (uint)_size) {
                throw new IndexOutOfRangeException();
            }
            _size--;
            if (index < _size) {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = default;
        } 
        public void RemoveAtSwapBack(int index) {
            if ((uint)index >= (uint)_size) {
                throw new IndexOutOfRangeException();
            }
            _size--;
            if (index < _size) {
                _items[index]=  _items[_size];
            }
            _items[_size] = default;
        }
        public void RemoveLast() {
            if(0<_size) {
                _items[--_size] = default;
            }
        } 
        public  bool Remove(T value) {
            int index = IndexOf(value);
            if (index >= 0) {
                RemoveAt(index);
                return true;
            }
            return false;
        }
        public  bool RemoveSwapBack(T value) {
            int index = IndexOf(value);
            if (index >= 0) {
                RemoveAtSwapBack(index);
                return true;
            }
            return false;
        }
        public  bool RemoveSwapBack(ref T item) {
            int index = IndexOf(ref item);
            if (index >= 0) {
                _size--;
                if (index < _size) {
                    _items[index]=  _items[_size];
                }
                _items[_size] = default;
                return true;
            }
            return false;
        }

        public void AddRange(ReadOnlySpan<T> span) {
            var newSize = _size + span.Length;
            if(Capacity<=newSize) {
                var newCapacity = Capacity;
                while (newCapacity <= newSize) {
                    newCapacity *= 2;
                }
                Array.Resize(ref _items,newCapacity);
            }
            span.CopyTo(_items.AsSpan(_size,span.Length));
            _size = newSize;
        }
        public void AddRange(List<T> list) {
            var newSize = _size + list.Count;
            if(Capacity<=newSize) {
                var newCapacity = Capacity;
                while (newCapacity <= newSize) {
                    newCapacity *= 2;
                }
                Array.Resize(ref _items,newCapacity);
            }
            list.AsSpan().CopyTo(_items.AsSpan(_size,list.Count));
            _size = newSize;
        }
        
         public void Resize(int count) {
            var newSize =count;
            if(Capacity<newSize) {
                var newCapacity = Capacity;
                while (newCapacity <= newSize) {
                    newCapacity *= 2;
                }
                Array.Resize(ref _items,newCapacity);
            }
            _size = newSize;
        }
         
        public void EnsureCapacity(int capacity) {
            if (capacity <= Capacity) return;
            var newCapacity = Capacity;
            while (newCapacity<capacity) {
                newCapacity *= 2;
            }
            Array.Resize(ref _items,newCapacity);
        }
        
        
        
        public void RemoveRange(int index, int count) {
            if (index < 0||count<0) 
                throw new IndexOutOfRangeException();
            
                
            if (_size - index < count)
                throw new ArgumentException();
    
            if (count > 0) {
                _size -= count;
                if (index < _size) {
                    Array.Copy(_items, index + count, _items, index, _size - index);
                }
                Array.Clear(_items, _size, count);
            }
        }
        
        public int IndexOf(ref T item) {
            var offset=(int)Unsafe.ByteOffset(ref _items[0],ref item);
            if (offset < 0 ||  _size * Unsafe.SizeOf<T>()<offset) return -1;
            return offset/Unsafe.SizeOf<T>();
        }
        
        public void Reverse() {
            AsSpan().Reverse();
        }
        public void Clear() {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size); 
                _size = 0;
            }
        }
        public int IndexOf(T item) {
            return Array.IndexOf(_items, item, 0, _size);
        }

        public bool Contains(T item) {
            return 0<=Array.IndexOf(_items, item, 0, _size);
        }
        public Span<T> AsSpan() => _items.AsSpan(0, _size);
        public ReversedSpan<T> AsReversedSpan() => _items.AsSpan(0, _size).AsReversed();
    }
}