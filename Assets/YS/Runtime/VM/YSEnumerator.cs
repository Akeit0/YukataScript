using System;
using UnityEngine;

namespace YS.VM {
    public interface IEnumeratorSource {
        public bool MoveNext(ref YSEnumerator ysEnumerator);
        public void Reset(ref YSEnumerator ysEnumerator);
        public void Dispose(ref YSEnumerator ysEnumerator);
        public Type GetCurrentType();
    }

    public struct YSEnumerator {
        public YSEnumerator(IEnumeratorSource source,Variable enumerable,Variable current) {
            Enumerable = enumerable;
            Source = source;
            Current = current;
            UserData = null;
            intA = 0;
            intB = 0;
        }
        public Variable Enumerable { get; }
        public Variable Current { get; }
        public object UserData;
        public int intA;
        public int intB;
        public IEnumeratorSource Source;

        public bool MoveNext() {
            if (Source is null) return true;
            return Source.MoveNext(ref this);
        }
        public void Reset() => Source.Reset(ref this);
        public void Dispose() => Source.Dispose(ref this);
    }
    public class IntRangeEnumerator:IEnumeratorSource {
        public bool MoveNext(ref YSEnumerator ysEnumerator) {
            if (ysEnumerator.intA < ysEnumerator.intB) {
                ysEnumerator.Current.SetValue(ysEnumerator.intA++);
                return true;
            }
            return false;
        }

        public void Reset(ref YSEnumerator ysEnumerator) {
            var range = ysEnumerator.Enumerable.As<IntRange>();
            ysEnumerator.intA = range.Start;
            ysEnumerator.intB = range.End;
        }

        public void Dispose(ref YSEnumerator ysEnumerator) {
        }
        public Type GetCurrentType() => typeof(int);
    }
    public class ArrayEnumerator<T>:IEnumeratorSource {
        public bool MoveNext(ref YSEnumerator ysEnumerator) {
            if (ysEnumerator.intA < ysEnumerator.intB) {
                ysEnumerator.Current.SetValue(ysEnumerator.Enumerable.As<T[]>()[ysEnumerator.intA++]);
                return true;
            }
            return false;
        }
        public void Reset(ref YSEnumerator ysEnumerator) {
            var array = ysEnumerator.Enumerable.As<T[]>();
            ysEnumerator.intA = 0;
            ysEnumerator.intB = array.Length;
        }

        public void Dispose(ref YSEnumerator ysEnumerator) {
        }
        public Type GetCurrentType() => typeof(T);
    }
    
   
}