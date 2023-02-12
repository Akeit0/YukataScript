using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
namespace YS.Collections {
    public static class CollectionsUtility {
        sealed class DummyList<T> {
            public T[] _items;
            public int _size;
            public int _version;
        }
        public static void Add<T>(ref T[] array,ref int  count, T element,float factor=2) {
            if(array.Length==count) {
                Array.Resize(ref array,(int) (array.Length * factor));
            }
            array[count++] = element;
        }

        public static unsafe RentSpan<T> RentUnmanagedSpan<T>(int length) where T : unmanaged {
            var size = sizeof(T);
            var array = ArrayPool<byte>.Shared.Rent(length * size);
            var span=MemoryMarshal.Cast<byte, T>(array.AsSpan()).Slice(0, length);
            return new RentSpan<T>(array,span);
        }

        public static RentSpan<T> RentClassSpan<T>(int length) where T : class {
            var array = ArrayPool<object>.Shared.Rent(length);
            array.AsSpan().Clear();
            var span = MemoryMarshal.CreateSpan(ref Unsafe.As<object,T>(ref array[0]),length);
            return new RentSpan<T>(array,span);
        }
           

        public static void ReturnToPool<T>(this T[] array,bool clearArray=false) => ArrayPool<T>.Shared.Return(array,clearArray);

        public static FixedArray1<T> FixedArray<T>(T item1) => new (item1);
        public static FixedArray2<T> FixedArray<T>(T item1,T item2) => new (item1,item2);
        public static FixedArray3<T> FixedArray<T>(T item1,T item2,T item3) => new (item1,item2,item3);
        public static FixedArray4<T> FixedArray<T>(T item1,T item2,T item3,T item4) => new (item1,item2,item3,item4);

        public static ReversedSpan<T> AsReversed<T>(this Span<T> span) {
            return new ReversedSpan<T>(span);
        }
        public static Span<T> AsSpan<T>(this List<T> list) {
          
            ref var dummyList =ref  Unsafe.As<List<T>,DummyList<T>>(ref list);
            return dummyList._items.AsSpan(0, list.Count);
        }
        public static T[] GetArray<T>(this List<T> list) {
          
            ref var dummyList =ref  Unsafe.As<List<T>,DummyList<T>>(ref list);
            return dummyList._items;
        }
        public static ref T[] GetArrayRef<T>(this List<T> list) {
          
            ref var dummyList =ref  Unsafe.As<List<T>,DummyList<T>>(ref list);
            return ref dummyList._items;
        }
        public static ref int GetSizeRef<T>(this List<T> list) {
          
            ref var dummyList =ref  Unsafe.As<List<T>,DummyList<T>>(ref list);
            return ref dummyList._size;
        }
        
        public static void DisposeIfCreated<T>(ref this NativeArray<T> nativeArray)where T :struct {
            if (nativeArray.IsCreated) nativeArray.Dispose();
        }
  

    }
}