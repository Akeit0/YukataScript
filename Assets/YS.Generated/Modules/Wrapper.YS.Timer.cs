#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif
using Timer = YS.Timer;
using _Timer = YS.Variable<YS.Timer>;
using _float = YS.Variable<float>;
using _bool = YS.Variable<bool>;
using UniTime = YS.UniTime;
using _UniTime = YS.Variable<YS.UniTime>;
using _double = YS.Variable<double>;
using EasingType = YS.EasingType;
using _EasingType = YS.Variable<YS.EasingType>;
using AnimationCurve = UnityEngine.AnimationCurve;
using _AnimationCurve = YS.Variable<UnityEngine.AnimationCurve>;

using static System.Runtime.CompilerServices.Unsafe;
using __V = YS.Variable;
namespace YS.Generated {
    public static partial  class Wrapper {
        static void YS_Timer_ctor(__V res,__V  i1, __V  i2) => As<_Timer>(res).value = new Timer(As<_float>(i1).value,As<_bool>(i2).value);
        static void YS_Timer_ctor_(__V res, __V  i1) => As<_Timer>(res).value = new Timer(As<_UniTime>(i1).value);
       static void YS_Timer_get_StartTime(__V res, __V  i1) => As<_double>(res).value=As<_Timer>(i1).value.StartTime;
       static void YS_Timer_get_TargetTime(__V res, __V  i1) => As<_double>(res).value=As<_Timer>(i1).value.TargetTime;
       static void YS_Timer_get_IgnoreScale(__V res, __V  i1) => As<_bool>(res).value=As<_Timer>(i1).value.IgnoreScale;
       static void YS_Timer_get_IsExpired(__V res, __V  i1) => As<_bool>(res).value=As<_Timer>(i1).value.IsExpired;
       static void YS_Timer_get_Elapsed(__V res, __V  i1) => As<_float>(res).value=As<_Timer>(i1).value.Elapsed;
       static void YS_Timer_get_ElapsedRatio(__V res, __V  i1) => As<_float>(res).value=As<_Timer>(i1).value.ElapsedRatio;
       static void YS_Timer_get_Remain(__V res, __V  i1) => As<_float>(res).value=As<_Timer>(i1).value.Remain;
       static void YS_Timer_get_RemainRatio(__V res, __V  i1) => As<_float>(res).value=As<_Timer>(i1).value.RemainRatio;
       static void YS_Timer_GetEasedElapsedRatio(__V res,__V  i1, __V  i2) => As<_float>(res).value=As<_Timer>(i1).value.GetEasedElapsedRatio(As<_EasingType>(i2).value);
       static void YS_Timer_GetCurvedElapsedRatio(__V res,__V  i1, __V  i2) => As<_float>(res).value=As<_Timer>(i1).value.GetCurvedElapsedRatio(As<_AnimationCurve>(i2).value);
       static void YS_Timer_GetEasedRemainRatio(__V res,__V  i1, __V  i2) => As<_float>(res).value=As<_Timer>(i1).value.GetEasedRemainRatio(As<_EasingType>(i2).value);
       static void YS_Timer_GetCurvedRemainRatio(__V res,__V  i1, __V  i2) => As<_float>(res).value=As<_Timer>(i1).value.GetCurvedRemainRatio(As<_AnimationCurve>(i2).value);
#if !AOT

        static TypeModule Create_YS_TimerModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Timer));
            module.RegisterConstructor(typeof(float),typeof(bool),YS_Timer_ctor);
            module.RegisterConstructor(typeof(UniTime),YS_Timer_ctor_);
            module.RegisterFieldGetter("StartTime", YS_Timer_get_StartTime);
            module.RegisterFieldGetter("TargetTime", YS_Timer_get_TargetTime);
            module.RegisterFieldGetter("IgnoreScale", YS_Timer_get_IgnoreScale);
            module.RegisterPropertyGetter("IsExpired", YS_Timer_get_IsExpired);
            module.RegisterPropertyGetter("Elapsed", YS_Timer_get_Elapsed);
            module.RegisterPropertyGetter("ElapsedRatio", YS_Timer_get_ElapsedRatio);
            module.RegisterPropertyGetter("Remain", YS_Timer_get_Remain);
            module.RegisterPropertyGetter("RemainRatio", YS_Timer_get_RemainRatio);
            module.RegisterMethod3("GetEasedElapsedRatio",YS_Timer_GetEasedElapsedRatio);
            module.RegisterMethod3("GetCurvedElapsedRatio",YS_Timer_GetCurvedElapsedRatio);
            module.RegisterMethod3("GetEasedRemainRatio",YS_Timer_GetEasedRemainRatio);
            module.RegisterMethod3("GetCurvedRemainRatio",YS_Timer_GetCurvedRemainRatio);
#else
        static unsafe TypeModule Create_YS_TimerModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Timer));
            module.RegisterConstructor(typeof(float),typeof(bool),&YS_Timer_ctor);
            module.RegisterConstructor(typeof(UniTime),&YS_Timer_ctor_);
            module.RegisterFieldGetter("StartTime", &YS_Timer_get_StartTime);
            module.RegisterFieldGetter("TargetTime", &YS_Timer_get_TargetTime);
            module.RegisterFieldGetter("IgnoreScale", &YS_Timer_get_IgnoreScale);
            module.RegisterPropertyGetter("IsExpired", &YS_Timer_get_IsExpired);
            module.RegisterPropertyGetter("Elapsed", &YS_Timer_get_Elapsed);
            module.RegisterPropertyGetter("ElapsedRatio", &YS_Timer_get_ElapsedRatio);
            module.RegisterPropertyGetter("Remain", &YS_Timer_get_Remain);
            module.RegisterPropertyGetter("RemainRatio", &YS_Timer_get_RemainRatio);
            module.RegisterMethod3("GetEasedElapsedRatio",&YS_Timer_GetEasedElapsedRatio);
            module.RegisterMethod3("GetCurvedElapsedRatio",&YS_Timer_GetCurvedElapsedRatio);
            module.RegisterMethod3("GetEasedRemainRatio",&YS_Timer_GetEasedRemainRatio);
            module.RegisterMethod3("GetCurvedRemainRatio",&YS_Timer_GetCurvedRemainRatio);
#endif
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}