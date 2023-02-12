using System;
using UnityEngine;
using UnityEngine.Scripting;
using YS.Modules;

namespace YS.Generated {
    [Preserve]
    public static partial class Wrapper {
        [RuntimeInitializeOnLoadMethod]
        public static void Init(){}
        
        public static bool Registered;
     
        static Wrapper() {
            RegisterModules();
            Registered = true;
        }
        static Type[] Types() => Util.Types();
        static Type[] Types(Type arg1) => Util.Types(arg1);
        static Type[] Types(Type arg1, Type arg2) => Util.Types(arg1, arg2);
        static Type[] Types(Type arg1,Type arg2,Type arg3) => Util.Types(arg1, arg2,arg3);
        static Type[] Types(Type arg1,Type arg2,Type arg3,Type arg4)=> Util.Types(arg1, arg2,arg3,arg4);
        static Type[] Types(Type arg1,Type arg2,Type arg3,Type arg4,Type arg5)=> Util.Types(arg1, arg2,arg3,arg4,arg5);
        static Type[] Types(Type arg1,Type arg2,Type arg3,Type arg4,Type arg5,Type arg6)=> Util.Types(arg1, arg2,arg3,arg4,arg5, arg6);

        static TypeModule GetModule(this Type type) => ModuleLibrary.GetModule(type);

    }
}