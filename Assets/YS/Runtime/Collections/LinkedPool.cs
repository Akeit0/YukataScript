using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace YS.Collections {
    
    public interface IPoolNode<T>
    {
        ref T NextNode { get; }

    }

    [StructLayout(LayoutKind.Auto)]
    public struct ThreadSafePool<T>
        where T : class, IPoolNode<T> {
        int gate;
        int size;
        T root;

        public int Size => size;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPop(out T result) {
            if (Interlocked.CompareExchange(ref gate, 1, 0) == 0) {
                var v = root;
                if (!(v is null)) {
                    ref var nextNode = ref v.NextNode;
                    root = nextNode;
                    nextNode = null;
                    size--;
                    result = v;
                    Volatile.Write(ref gate, 0);
                    return true;
                }

                Volatile.Write(ref gate, 0);
            }

            result = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPush(T item) {
            if (Interlocked.CompareExchange(ref gate, 1, 0) == 0) {
                if (size < 500) {
                    item.NextNode = root;
                    root = item;
                    size++;
                    Volatile.Write(ref gate, 0);
                    return true;
                }
                else {
                    Volatile.Write(ref gate, 0);
                }
            }

            return false;
        }
    }

    [StructLayout(LayoutKind.Auto)]
    public struct Pool<T>
        where T : class, IPoolNode<T>
    {
        int size;
        T root;
        public Func<T> Creator;
        public static readonly int MaxPoolSize = 500;
        public int Size => size;

        public T Pop() {
            if (TryPop(out var result)) {
                return result;
            }
            return Creator();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPop(out T result)
        {
            var v = root;
            if (!(v is null))
            {
                ref var nextNode = ref v.NextNode;
                root = nextNode;
                nextNode = null;
                size--;
                result = v;
                return true;
            }
            result = default;
            return false;
        } 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPush(T item)
        {
            if (size < MaxPoolSize)
            {
                item.NextNode = root;
                root = item;
                size++;
                return true;
            }
            return false;
        }
    }
}