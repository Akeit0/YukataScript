using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace YS.Collections {

    public ref struct RentSpan<T> {
        private object _toReturn;
        public  Span<T> Span;
        
        public RentSpan(object[] toReturn, Span<T> span) {
            _toReturn = toReturn;
            Span = span;
        } 
        public RentSpan(Span<T> span) {
            _toReturn = null;
            Span = span;
        }
        public RentSpan(byte[] toReturn, Span<T> span) {
            _toReturn = toReturn;
            Span = span;
        }
        public RentSpan(int length) {
            var array = ArrayPool<T>.Shared.Rent(length);
            _toReturn = array;
            Span = array.AsSpan(0,length);
        }

        public bool NeedToReturn => _toReturn != null;
        public void Dispose() {
            Span.Clear();
            Span = default;
            if(_toReturn!=null) {
                if (_toReturn is T[] tArray) 
                    ArrayPool<T>.Shared.Return(tArray);
                else if (_toReturn is byte[] bytes)
                    ArrayPool<byte>.Shared.Return(bytes);
                else if (_toReturn is object[] objects)
                    ArrayPool<object>.Shared.Return(objects);
                else throw new Exception();
                _toReturn = null;
            }
        }
        public void DisposeIfRent() {
            if(_toReturn!=null) {
                Dispose();
            }
        }
    }
    
    
    
}