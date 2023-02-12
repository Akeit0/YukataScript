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
            foreach (var methodInfo in UnityEditor.TypeCache.GetMethodsWithAttribute(typeof(Attributes.ReflectionCallAttribute))) {
                module.RegisterMethod(methodInfo);
            }
#else
            
#endif
            
            module.RegisterMethod(nameof(print), print);
            module.RegisterMethod(nameof(ms),Types(typeof(int)), (result,input1)=>result.SetValue(input1.As<int>().ms()));
            module.RegisterMethod(nameof(ms),Types(typeof(float)), (result,input1)=>result.SetValue(input1.As<float>().ms()));
            module.RegisterMethod(nameof(s),Types(typeof(int)), (result,input1)=>result.SetValue(input1.As<int>().s()));
            module.RegisterMethod(nameof(s),Types(typeof(float)), (result,input1)=>result.SetValue(input1.As<float>().s()));
            module.RegisterMethod(nameof(s),Types(typeof(double)), (result,input1)=>result.SetValue(input1.As<double>().s()));
            Instance = module;
        }
        public static void Activate(){}
        public static readonly TypeModule Instance;
        public static void print(Variable input1) => Debug.Log(input1);
        public static TimeSpan ms(this int input) => TimeSpan.FromMilliseconds(input);
        public static TimeSpan s(this int input) => TimeSpan.FromSeconds(input);
        public static TimeSpan ms(this float input) => TimeSpan.FromMilliseconds(input);
        public static TimeSpan s(this float input) => TimeSpan.FromSeconds(input);
        
        public static TimeSpan s(this double input) => TimeSpan.FromSeconds(input);

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