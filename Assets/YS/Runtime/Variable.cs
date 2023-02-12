using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using YS.Collections;
using YS.VM;

namespace YS {
   public abstract class Variable {
        public static readonly UnusableMemoryBox Discarded = new UnusableMemoryBox();
        public static readonly UnusableMemoryBox MustBeZero = new UnusableMemoryBox();

        public  static StringDictionary<Variable> NonGenericDictionary;

        protected static TypeKeyAddOnlyDictionary<Variable> dict = new(16);
        protected Variable() {}
        public static Variable New(Type type) {
            if (dict.TryGetValue(type,out var variable)) {
                return variable.New();
            }
            try {
                var genericType = typeof(Variable<>).MakeGenericType(type);
                return  Activator.CreateInstance(genericType) as Variable;
            }
            catch (Exception) {
                if (type.IsClass) {
                    return new TypedObjectVariable(type);
                }
                return null;
            }
        }
        
        public abstract Type type { get; }
       

        public abstract object ToObject();
        public abstract void SetObject(object value);
        

        public ref T As<T>() {
            return ref Unsafe.As<Variable<T>>(this).value;
        }
        
        

        public void SetValue<T>(T value) {
            Unsafe.As<Variable<T>>(this).value = value;
        }

        public abstract void SetDefault();

        public abstract void CopyFrom(Variable src);

        public virtual void Swap(Variable src){}
        public abstract ref object Cache { get; }
 
        public abstract Variable New();

        public Variable Clone() {
            return (Variable)this.MemberwiseClone();
        }

   }

    public sealed class UnusableMemoryBox : Variable {
#pragma warning disable CS0414
        private LongMemory _memory;
#pragma warning restore CS0414
        
        [StructLayout(LayoutKind.Sequential, Size = 32)]
        private  struct LongMemory {
        }
        public override Type type => null;

        public override object ToObject() {
            return null;
        }

        public void Clear() {
            _memory = default;
        }

        public override void SetObject(object value) { }

        public override void SetDefault() {
        }

        public override void CopyFrom(Variable src) { }

        public override ref object Cache => throw new NotImplementedException();

        public override Variable New() {
            throw new NotImplementedException();
        }


        public static string NullString = "null";
    }
    
    public class TypedObjectVariable : Variable<object> {
        public override Type type => ClassType;
        public Type ClassType;
        public TypedObjectVariable() {
        }
        public TypedObjectVariable(Type type) {
            ClassType = type;
        }
    }
    
    
    [Serializable]
    
    public  class Variable <T>:Variable {

        static Variable() {
            if(!dict.ContainsKey(typeof(T))) {
                dict.Add(typeof(T), new Variable<T>());
            }
        }
        public Variable(){}
     
        public Variable(T value)
        {
            this.value = value;
        }
        public T value;
   
       public override Type type => typeof(T);

       public override object ToObject() {
            return value;
        }

        public override void SetObject(object obj) {
            value = (T) obj;
        }
        public override void SetDefault() {
            value = default;
        }
        public override void CopyFrom(Variable src) {
            value=src.As<T>() ;
        }

        public override Variable New() {
            return new Variable<T>();
        }

        public override void Swap(Variable target) {
            var v = value;
            value = target.As<T>();
            target.As<T>() = v;
        }

        public override ref object Cache => ref _cache;
        static object _cache;

        public override string ToString() {
            return value!=null?value.ToString():"null";
        }
        
        
    }

}