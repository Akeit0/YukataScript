using System;
using YS.Modules;
using Array = System.Array;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_System_ArrayModule<T>(global::System.Type baseType = null) {
            var module = new TypeModule(typeof(T[]),true);
            VM.DelegateLibrary.RegisterArrayEnumerator<T>();
            module.BaseModule = typeof(Array).GetModule();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            module.RegisterConstructor(typeof(int),(result, input1) => result.SetValue( new T[input1.As<int>()]));
            module.RegisterMethod(new MethodData(typeof(T[]),MethodType.Instance,"get_Item", typeof(T),new ParamDescription[]{new ("index",typeof(int),PassType.Value)}, (Action<Variable, Variable, Variable>)((result, input1, input2) => result.SetValue(input1.As<int[]>()[input2.As<int>()]))));
            module.RegisterMethod(new MethodData(typeof(T[]),MethodType.Instance,"set_Item", typeof(T),new ParamDescription[]{new ("index",typeof(int),PassType.Value),new ("value",typeof(T),PassType.Value)},(Action<Variable, Variable, Variable>)( (input1, input2, input3) => input1.As<T[]>()[input2.As<int>()]=input3.As<T>())));
            return module;
        }
        
    }
}