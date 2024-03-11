#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif
using AnimationCurve = UnityEngine.AnimationCurve;
using _AnimationCurve = YS.Variable<UnityEngine.AnimationCurve>;
using Keyframe = UnityEngine.Keyframe;
using _Keyframe = YS.Variable<UnityEngine.Keyframe>;
using _int = YS.Variable<int>;
using WrapMode = UnityEngine.WrapMode;
using _WrapMode = YS.Variable<UnityEngine.WrapMode>;
using _float = YS.Variable<float>;
using _bool = YS.Variable<bool>;
using _object = YS.Variable<object>;

using static System.Runtime.CompilerServices.Unsafe;
using __V = YS.Variable;
namespace YS.Generated {
    public static partial  class Wrapper {
        static void UnityEngine_AnimationCurve_ctor(__V res, __V  i1) => As<_AnimationCurve>(res).value = new AnimationCurve(As<Variable<Keyframe[]>>(i1).value);
        static void UnityEngine_AnimationCurve_ctor_(__V res) =>  As<_AnimationCurve>(res).value = new AnimationCurve();
       static void UnityEngine_AnimationCurve_get_keys(__V res, __V  i1) => As<Variable<Keyframe[]>>(res).value=As<_AnimationCurve>(i1).value.keys;
       static void UnityEngine_AnimationCurve_set_keys(__V  i1, __V  i2) => As<_AnimationCurve>(i1).value.keys = As<Variable<Keyframe[]>>(i2).value;
       static void UnityEngine_AnimationCurve_get_length(__V res, __V  i1) => As<_int>(res).value=As<_AnimationCurve>(i1).value.length;
       static void UnityEngine_AnimationCurve_get_preWrapMode(__V res, __V  i1) => As<_WrapMode>(res).value=As<_AnimationCurve>(i1).value.preWrapMode;
       static void UnityEngine_AnimationCurve_set_preWrapMode(__V  i1, __V  i2) => As<_AnimationCurve>(i1).value.preWrapMode = As<_WrapMode>(i2).value;
       static void UnityEngine_AnimationCurve_get_postWrapMode(__V res, __V  i1) => As<_WrapMode>(res).value=As<_AnimationCurve>(i1).value.postWrapMode;
       static void UnityEngine_AnimationCurve_set_postWrapMode(__V  i1, __V  i2) => As<_AnimationCurve>(i1).value.postWrapMode = As<_WrapMode>(i2).value;
       static void UnityEngine_AnimationCurve_Evaluate(__V res,__V  i1, __V  i2) => As<_float>(res).value=As<_AnimationCurve>(i1).value.Evaluate(As<_float>(i2).value);
       static void UnityEngine_AnimationCurve_AddKey(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_int>(res).value=As<_AnimationCurve>(i1).value.AddKey(As<_float>(i2).value,As<_float>(i3).value);
       static void UnityEngine_AnimationCurve_AddKey_(__V res,__V  i1, __V  i2) => As<_int>(res).value=As<_AnimationCurve>(i1).value.AddKey(As<_Keyframe>(i2).value);
       static void UnityEngine_AnimationCurve_MoveKey(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_int>(res).value=As<_AnimationCurve>(i1).value.MoveKey(As<_int>(i2).value,As<_Keyframe>(i3).value);
       static void UnityEngine_AnimationCurve_RemoveKey(__V  i1, __V  i2) => As<_AnimationCurve>(i1).value.RemoveKey(As<_int>(i2).value);
       static void UnityEngine_AnimationCurve_get_Item(__V res,__V  i1, __V  i2) => As<_Keyframe>(res).value=As<_AnimationCurve>(i1).value[As<_int>(i2).value];
       static void UnityEngine_AnimationCurve_SmoothTangents(__V  i1, __V  i2, __V  i3)  =>  As<_AnimationCurve>(i1).value.SmoothTangents(As<_int>(i2).value,As<_float>(i3).value);
       static void UnityEngine_AnimationCurve_Constant(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_AnimationCurve>(res).value=AnimationCurve.Constant(As<_float>(i1).value,As<_float>(i2).value,As<_float>(i3).value);
       static void UnityEngine_AnimationCurve_Linear(__V res,__V  i1, __V  i2, __V  i3, __V  i4)  => As<_AnimationCurve>(res).value=AnimationCurve.Linear(As<_float>(i1).value,As<_float>(i2).value,As<_float>(i3).value,As<_float>(i4).value);
       static void UnityEngine_AnimationCurve_EaseInOut(__V res,__V  i1, __V  i2, __V  i3, __V  i4)  => As<_AnimationCurve>(res).value=AnimationCurve.EaseInOut(As<_float>(i1).value,As<_float>(i2).value,As<_float>(i3).value,As<_float>(i4).value);
       static void UnityEngine_AnimationCurve_Equals(__V res,__V  i1, __V  i2) => As<_bool>(res).value=As<_AnimationCurve>(i1).value.Equals(As<_object>(i2).value);
       static void UnityEngine_AnimationCurve_Equals_(__V res,__V  i1, __V  i2) => As<_bool>(res).value=As<_AnimationCurve>(i1).value.Equals(As<_AnimationCurve>(i2).value);
       static void UnityEngine_AnimationCurve_GetHashCode(__V res, __V  i1) => As<_int>(res).value=As<_AnimationCurve>(i1).value.GetHashCode();
#if !AOT

        static TypeModule Create_UnityEngine_AnimationCurveModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(AnimationCurve));
            module.RegisterConstructor(typeof(Keyframe[]),UnityEngine_AnimationCurve_ctor);
            module.RegisterConstructor(UnityEngine_AnimationCurve_ctor_);
            module.RegisterPropertyGetter("keys", UnityEngine_AnimationCurve_get_keys);
            module.RegisterPropertySetter("keys", UnityEngine_AnimationCurve_set_keys);
            module.RegisterPropertyGetter("length", UnityEngine_AnimationCurve_get_length);
            module.RegisterPropertyGetter("preWrapMode", UnityEngine_AnimationCurve_get_preWrapMode);
            module.RegisterPropertySetter("preWrapMode", UnityEngine_AnimationCurve_set_preWrapMode);
            module.RegisterPropertyGetter("postWrapMode", UnityEngine_AnimationCurve_get_postWrapMode);
            module.RegisterPropertySetter("postWrapMode", UnityEngine_AnimationCurve_set_postWrapMode);
            module.RegisterMethod3("Evaluate",UnityEngine_AnimationCurve_Evaluate);
            module.RegisterMethod4("AddKey", Types(typeof(float),typeof(float)),UnityEngine_AnimationCurve_AddKey);
            module.RegisterMethod3("AddKey", Types(typeof(Keyframe)),UnityEngine_AnimationCurve_AddKey_);
            module.RegisterMethod4("MoveKey",UnityEngine_AnimationCurve_MoveKey);
            module.RegisterMethod2("RemoveKey",UnityEngine_AnimationCurve_RemoveKey);
            module.RegisterMethod3("get_Item", UnityEngine_AnimationCurve_get_Item);
            module.RegisterMethod3("SmoothTangents",UnityEngine_AnimationCurve_SmoothTangents);
            module.RegisterMethod4("Constant",UnityEngine_AnimationCurve_Constant);
            module.RegisterMethod5("Linear",UnityEngine_AnimationCurve_Linear);
            module.RegisterMethod5("EaseInOut",UnityEngine_AnimationCurve_EaseInOut);
            module.RegisterMethod3("Equals", Types(typeof(object)),UnityEngine_AnimationCurve_Equals);
            module.RegisterMethod3("Equals", Types(typeof(AnimationCurve)),UnityEngine_AnimationCurve_Equals_);
            module.RegisterMethod2("GetHashCode",UnityEngine_AnimationCurve_GetHashCode);
#else
        static unsafe TypeModule Create_UnityEngine_AnimationCurveModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(AnimationCurve));
            module.RegisterConstructor(typeof(Keyframe[]),&UnityEngine_AnimationCurve_ctor);
            module.RegisterConstructor(&UnityEngine_AnimationCurve_ctor_);
            module.RegisterPropertyGetter("keys", &UnityEngine_AnimationCurve_get_keys);
            module.RegisterPropertySetter("keys",& UnityEngine_AnimationCurve_set_keys);
            module.RegisterPropertyGetter("length", &UnityEngine_AnimationCurve_get_length);
            module.RegisterPropertyGetter("preWrapMode", &UnityEngine_AnimationCurve_get_preWrapMode);
            module.RegisterPropertySetter("preWrapMode",& UnityEngine_AnimationCurve_set_preWrapMode);
            module.RegisterPropertyGetter("postWrapMode", &UnityEngine_AnimationCurve_get_postWrapMode);
            module.RegisterPropertySetter("postWrapMode",& UnityEngine_AnimationCurve_set_postWrapMode);
            module.RegisterMethod3("Evaluate",&UnityEngine_AnimationCurve_Evaluate);
            module.RegisterMethod4("AddKey", Types(typeof(float),typeof(float)),&UnityEngine_AnimationCurve_AddKey);
            module.RegisterMethod3("AddKey", Types(typeof(Keyframe)),&UnityEngine_AnimationCurve_AddKey_);
            module.RegisterMethod4("MoveKey",&UnityEngine_AnimationCurve_MoveKey);
            module.RegisterMethod2("RemoveKey",&UnityEngine_AnimationCurve_RemoveKey);
            module.RegisterMethod3("get_Item", &UnityEngine_AnimationCurve_get_Item);
            module.RegisterMethod3("SmoothTangents",&UnityEngine_AnimationCurve_SmoothTangents);
            module.RegisterMethod4("Constant",&UnityEngine_AnimationCurve_Constant);
            module.RegisterMethod5("Linear",&UnityEngine_AnimationCurve_Linear);
            module.RegisterMethod5("EaseInOut",&UnityEngine_AnimationCurve_EaseInOut);
            module.RegisterMethod3("Equals", Types(typeof(object)),&UnityEngine_AnimationCurve_Equals);
            module.RegisterMethod3("Equals", Types(typeof(AnimationCurve)),&UnityEngine_AnimationCurve_Equals_);
            module.RegisterMethod2("GetHashCode",&UnityEngine_AnimationCurve_GetHashCode);
#endif
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}