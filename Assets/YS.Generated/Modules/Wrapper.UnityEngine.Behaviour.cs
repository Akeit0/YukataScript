#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif
using Behaviour = UnityEngine.Behaviour;
using _Behaviour = YS.Variable<UnityEngine.Behaviour>;
using _bool = YS.Variable<bool>;

using static System.Runtime.CompilerServices.Unsafe;
using __V = YS.Variable;
namespace YS.Generated {
    public static partial  class Wrapper {
        static void UnityEngine_Behaviour_ctor(__V res) =>  As<_Behaviour>(res).value = new Behaviour();
       static void UnityEngine_Behaviour_get_enabled(__V res, __V  i1) => As<_bool>(res).value=As<_Behaviour>(i1).value.enabled;
       static void UnityEngine_Behaviour_set_enabled(__V  i1, __V  i2) => As<_Behaviour>(i1).value.enabled = As<_bool>(i2).value;
       static void UnityEngine_Behaviour_get_isActiveAndEnabled(__V res, __V  i1) => As<_bool>(res).value=As<_Behaviour>(i1).value.isActiveAndEnabled;
#if !AOT

        static TypeModule Create_UnityEngine_BehaviourModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Behaviour));
            module.RegisterConstructor(UnityEngine_Behaviour_ctor);
            module.RegisterPropertyGetter("enabled", UnityEngine_Behaviour_get_enabled);
            module.RegisterPropertySetter("enabled", UnityEngine_Behaviour_set_enabled);
            module.RegisterPropertyGetter("isActiveAndEnabled", UnityEngine_Behaviour_get_isActiveAndEnabled);
#else
        static unsafe TypeModule Create_UnityEngine_BehaviourModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Behaviour));
            module.RegisterConstructor(&UnityEngine_Behaviour_ctor);
            module.RegisterPropertyGetter("enabled", &UnityEngine_Behaviour_get_enabled);
            module.RegisterPropertySetter("enabled",& UnityEngine_Behaviour_set_enabled);
            module.RegisterPropertyGetter("isActiveAndEnabled", &UnityEngine_Behaviour_get_isActiveAndEnabled);
#endif
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}