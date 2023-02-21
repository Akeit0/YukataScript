#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif

using System;
using System.Collections;
using UnityEngine;
using YS.Async;
using YS.Collections;
using YS.Instructions;
using static YS.Util;
using static System.Runtime.CompilerServices.Unsafe;
namespace YS.Modules {
    public static class GlobalModule {

        static unsafe  GlobalModule() {
            var module = new TypeModule(typeof(GlobalModule));
#if UNITY_EDITOR
            foreach (var methodInfo in UnityEditor.TypeCache.GetMethodsWithAttribute(typeof(ReflectionCallAttribute))) {
                module.RegisterMethod(methodInfo);
            }
#else
            
#endif
            #if AOT
            module.RegisterMethod1(nameof(print), &print);
            module.RegisterMethod3(nameof(ms),Types(typeof(int),typeof(bool)), &_ms);
            module.RegisterMethod3(nameof(ms),Types(typeof(float),typeof(bool)),&_ms_);
            module.RegisterMethod3(nameof(s),Types(typeof(int),typeof(bool)), &_s);
            module.RegisterMethod3(nameof(s),Types(typeof(float),typeof(bool)), &_s_);
            module.RegisterMethod3(nameof(s),Types(typeof(double),typeof(bool)), &_s__);
            #else
            module.RegisterMethod1(nameof(print), print);
            module.RegisterMethod3(nameof(ms),Types(typeof(int),typeof(bool)), _ms);
            module.RegisterMethod3(nameof(ms),Types(typeof(float),typeof(bool)),_ms_);
            module.RegisterMethod3(nameof(s),Types(typeof(int),typeof(bool)), _s);
            module.RegisterMethod3(nameof(s),Types(typeof(float),typeof(bool)), _s_);
            module.RegisterMethod3(nameof(s),Types(typeof(double),typeof(bool)), _s__);
            #endif
            module.ClearTempMembers();
            Instance = module;
        }
        public static void Activate(){}
        public static readonly TypeModule Instance;
        public static void print(Variable input1) => Debug.Log(input1);
        public static UniTime ms(this int input,bool ignoreScale=false) =>new UniTime(input/1000f,ignoreScale) ;
        public static UniTime s(this int input,bool ignoreScale=false) =>new UniTime(input,ignoreScale) ;
        public static void _ms(Variable res,Variable i1,Variable i2) =>As<Variable<UniTime>>(res).value=new UniTime(As<Variable<int>>(i1).value/1000f,As<Variable<bool>>(i2).value) ;
        public static void _s(Variable res,Variable i1,Variable i2) =>As<Variable<UniTime>>(res).value=new UniTime(As<Variable<int>>(i1).value,As<Variable<bool>>(i2).value) ;
       
        public static UniTime ms(this float input,bool ignoreScale=false) => new UniTime(input/1000f,ignoreScale) ;
        public static UniTime s(this float input,bool ignoreScale=false) =>new UniTime(input,ignoreScale) ;
        public static void _ms_(Variable res,Variable i1,Variable i2) =>As<Variable<UniTime>>(res).value=new UniTime(As<Variable<float>>(i1).value/1000f,As<Variable<bool>>(i2).value) ;
        public static void _s_(Variable res,Variable i1,Variable i2) =>As<Variable<UniTime>>(res).value=new UniTime(As<Variable<float>>(i1).value,As<Variable<bool>>(i2).value) ;

        
        public static UniTime s(this double input,bool ignoreScale=false) =>new UniTime((float)input,ignoreScale) ;
        public static void _s__(Variable res,Variable i1,Variable i2) =>As<Variable<UniTime>>(res).value=new UniTime((float)As<Variable<double>>(i1).value,As<Variable<bool>>(i2).value) ;

    }


    public class VariableModule {
        static unsafe VariableModule() {
            var module = new TypeModule(typeof(VariableModule));
            #if AOT
            module.RegisterMethod2("ToString", Types(typeof(Variable)),
                &_ToString);
            module.RegisterMethod2("GetType", Types(typeof(Variable)),
                &_GetType);
            #else
            module.RegisterMethod2("ToString", Types(typeof(Variable)),
                _ToString);
            module.RegisterMethod2("GetType", Types(typeof(Variable)),
                _GetType);
            #endif
            module.ClearTempMembers();
            Instance = module;
        }
        public static readonly TypeModule Instance;
        public static string ToString(Variable input1) => input1.ToString();
        public static void _ToString(Variable res,Variable input1) =>res.As<string>()= input1.ToString();
        public static Type GetType(Variable input1) => input1.ToObject().GetType();
        public static void _GetType(Variable res,Variable input1) =>res.As<Type>()= input1.GetType();
    }
}