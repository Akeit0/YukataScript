using System;
using System.Collections;
using UnityEngine;
using YS.Async;
using YS.Collections;
using YS.Instructions;
using static YS.Util;

namespace YS.Modules {
    public static class GlobalModule {

        static GlobalModule() {
            var module = new TypeModule(typeof(GlobalModule));
#if UNITY_EDITOR
            foreach (var methodInfo in UnityEditor.TypeCache.GetMethodsWithAttribute(typeof(ReflectionCallAttribute))) {
                module.RegisterMethod(methodInfo);
            }
#else
            
#endif
            
            module.RegisterMethod(nameof(print), print);
            module.RegisterMethod(nameof(ms),Types(typeof(int),typeof(bool)), (result,input1,input2)=>result.SetValue(input1.As<int>().ms(input2.As<bool>())));
            module.RegisterMethod(nameof(ms),Types(typeof(float),typeof(bool)), (result,input1)=>result.SetValue(input1.As<float>().ms()));
            module.RegisterMethod(nameof(s),Types(typeof(int),typeof(bool)), (result,input1,input2)=>result.SetValue(input1.As<int>().s(input2.As<bool>())));
            module.RegisterMethod(nameof(s),Types(typeof(float),typeof(bool)), (result,input1)=>result.SetValue(input1.As<float>().s()));
            module.RegisterMethod(nameof(s),Types(typeof(double),typeof(bool)), (result,input1)=>result.SetValue(input1.As<double>().s()));
            Instance = module;
        }
        public static void Activate(){}
        public static readonly TypeModule Instance;
        public static void print(Variable input1) => Debug.Log(input1);
        public static UniTime ms(this int input,bool ignoreScale=false) =>new UniTime(input/1000f,ignoreScale) ;
        public static UniTime s(this int input,bool ignoreScale=false) =>new UniTime(input,ignoreScale) ;
        public static UniTime ms(this float input,bool ignoreScale=false) => new UniTime(input/1000f,ignoreScale) ;
        public static UniTime s(this float input,bool ignoreScale=false) =>new UniTime(input,ignoreScale) ;
        
        public static UniTime s(this double input,bool ignoreScale=false) =>new UniTime((float)input,ignoreScale) ;

    }


    public class VariableModule {
        static VariableModule() {
            var module = new TypeModule(typeof(VariableModule));
            module.RegisterMethod("ToString", Types(typeof(Variable)),
                (result, input1) => result.SetValue(ToString(input1)));
            module.RegisterMethod("GetType", Types(typeof(Variable)),
                (result, input1) => result.SetValue(GetType(input1)));
            module.ClearTempMembers();
            Instance = module;
        }
        public static readonly TypeModule Instance;
        public static string ToString(Variable input1) => input1.ToString();
        public static Type GetType(Variable input1) => input1.ToObject().GetType();
    }

    public static class IntModule {
        public static IEnumerator frames(int input1) => FrameObject.Create(input1);

        private class FrameObject : IPooledCoroutine<FrameObject> {
            static Pool<FrameObject> _pool;
            public ref FrameObject NextNode => ref _next;
            private FrameObject _next;
            private int _frameCount;

            public static FrameObject Create(int frameCount) {
                if (!_pool.TryPop(out var item)) return new FrameObject(frameCount);
                item._frameCount = frameCount;
                return item;
            }

            private FrameObject(int frameCount) {
                _frameCount = frameCount;
            }

            public bool MoveNext() => 0 < --_frameCount;

            public void Reset() {
            }

            public object Current => null;

            public void Dispose() {
                _pool.TryPush(this);
            }
        }
    }
}