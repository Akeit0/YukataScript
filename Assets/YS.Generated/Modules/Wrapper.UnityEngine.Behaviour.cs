using Behaviour = UnityEngine.Behaviour;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_BehaviourModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Behaviour));
            module.RegisterConstructor(result => result.SetValue( new Behaviour()));
            module.RegisterPropertyGetter("enabled", (result, input1) => result.SetValue(input1.As<Behaviour>().enabled));
            module.RegisterPropertySetter("enabled", (input1, input2) => input1.As<Behaviour>().enabled=input2.As<bool>());
            module.RegisterPropertyGetter("isActiveAndEnabled", (result, input1) => result.SetValue(input1.As<Behaviour>().isActiveAndEnabled));
            module.ClearTempMembers();
                          if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}