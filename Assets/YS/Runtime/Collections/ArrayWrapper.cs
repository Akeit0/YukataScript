using System;

namespace YS.Collections {

    public struct ArrayWrapper<T> {
        const int ThreadStaticBufferSize = 17;
        public T[] Array;
        public bool IsRented;
        public bool IsCreated => Array != null;
        
        public void EnsureCapacity(int capacity) {
            if(IsRented)throw new Exception();
            if(Array==null||Array.Length<capacity)  Array = new T[Math.Max(ThreadStaticBufferSize, capacity)];
        }
    }
}