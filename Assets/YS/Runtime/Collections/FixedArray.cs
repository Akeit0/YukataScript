using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static YS.Collections.CollectionsUtility;
namespace YS.Collections {
    public interface IFixedArray<T> {
        Span<T> AsSpan();
       
        int Length { get; }
    }
    public struct FixedArray1<T> :IFixedArray<T>{
        public T Item1;
        public FixedArray1(T item1) {
            Item1 = item1;
        }

        public static implicit operator Span<T>(FixedArray1<T> array1) => array1.AsSpan();
        public Span<T> AsSpan() {
            return MemoryMarshal.CreateSpan(ref Item1, 1);
        }
       
        public int Length => 2;
    }
public struct FixedArray2<T> :IFixedArray<T>{
    public T Item1; public T Item2;

    public FixedArray2(T item1, T item2) {
        Item1 = item1;
        Item2 = item2;
    }
    public static implicit operator Span<T>(FixedArray2<T> array2) => array2.AsSpan();
        public Span<T> AsSpan() {
            return MemoryMarshal.CreateSpan(ref Item1, 2);
        }
       
        public int Length => 2;
    }

    public struct FixedArray3<T> :IFixedArray<T>{
        public T Item1; public T Item2; public T Item3;
        public FixedArray3(T item1, T item2, T item3) {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }
        public static implicit operator Span<T>(FixedArray3<T> array3) => array3.AsSpan();
        public Span<T> AsSpan() {
          return MemoryMarshal.CreateSpan(ref Item1, 3);
        }
       
        public int Length => 3;
    } 
    public struct FixedArray4<T> :IFixedArray<T>{
        public T Item1; public T Item2; public T Item3;public T Item4;
        public FixedArray4(T item1, T item2, T item3, T item4) {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }
        public static implicit operator Span<T>(FixedArray4<T> array4) => array4.AsSpan();
        public Span<T> AsSpan() {
          return MemoryMarshal.CreateSpan(ref Item1, 4);
        }
        public int Length => 4;
    }
}