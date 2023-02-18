using Timer = YS.Timer;
using UniTime = YS.UniTime;
using EasingType = YS.EasingType;
using AnimationCurve = UnityEngine.AnimationCurve;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_YS_TimerModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Timer));
            module.RegisterConstructor(typeof(float),typeof(bool),(result, input1, input2) => result.SetValue( new Timer(input1.As<float>(),input2.As<bool>())));
            module.RegisterConstructor(typeof(UniTime),(result, input1) => result.SetValue( new Timer(input1.As<UniTime>())));
            module.RegisterFieldGetter("StartTime", (result, input1) => result.SetValue(input1.As<Timer>().StartTime));
            module.RegisterFieldGetter("TargetTime", (result, input1) => result.SetValue(input1.As<Timer>().TargetTime));
            module.RegisterFieldGetter("IgnoreScale", (result, input1) => result.SetValue(input1.As<Timer>().IgnoreScale));
            module.RegisterPropertyGetter("IsExpired", (result, input1) => result.SetValue(input1.As<Timer>().IsExpired));
            module.RegisterPropertyGetter("Elapsed", (result, input1) => result.SetValue(input1.As<Timer>().Elapsed));
            module.RegisterPropertyGetter("ElapsedRatio", (result, input1) => result.SetValue(input1.As<Timer>().ElapsedRatio));
            module.RegisterPropertyGetter("Remain", (result, input1) => result.SetValue(input1.As<Timer>().Remain));
            module.RegisterPropertyGetter("RemainRatio", (result, input1) => result.SetValue(input1.As<Timer>().RemainRatio));
            module.RegisterMethod("GetEasedElapsedRatio", (result, input1, input2) => result.SetValue(input1.As<Timer>().GetEasedElapsedRatio(input2.As<EasingType>())));
            module.RegisterMethod("GetCurvedElapsedRatio", (result, input1, input2) => result.SetValue(input1.As<Timer>().GetCurvedElapsedRatio(input2.As<AnimationCurve>())));
            module.RegisterMethod("GetEasedRemainRatio", (result, input1, input2) => result.SetValue(input1.As<Timer>().GetEasedRemainRatio(input2.As<EasingType>())));
            module.RegisterMethod("GetCurvedRemainRatio", (result, input1, input2) => result.SetValue(input1.As<Timer>().GetCurvedRemainRatio(input2.As<AnimationCurve>())));
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}