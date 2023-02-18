using AnimationCurve = UnityEngine.AnimationCurve;
using Keyframe = UnityEngine.Keyframe;
using WrapMode = UnityEngine.WrapMode;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_AnimationCurveModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(AnimationCurve));
            module.RegisterConstructor(typeof(Keyframe[]),(result, input1) => result.SetValue( new AnimationCurve(input1.As<Keyframe[]>())));
            module.RegisterConstructor(result => result.SetValue( new AnimationCurve()));
            module.RegisterPropertyGetter("keys", (result, input1) => result.SetValue(input1.As<AnimationCurve>().keys));
            module.RegisterPropertySetter("keys", (input1, input2) => input1.As<AnimationCurve>().keys=input2.As<Keyframe[]>());
            module.RegisterPropertyGetter("length", (result, input1) => result.SetValue(input1.As<AnimationCurve>().length));
            module.RegisterPropertyGetter("preWrapMode", (result, input1) => result.SetValue(input1.As<AnimationCurve>().preWrapMode));
            module.RegisterPropertySetter("preWrapMode", (input1, input2) => input1.As<AnimationCurve>().preWrapMode=input2.As<WrapMode>());
            module.RegisterPropertyGetter("postWrapMode", (result, input1) => result.SetValue(input1.As<AnimationCurve>().postWrapMode));
            module.RegisterPropertySetter("postWrapMode", (input1, input2) => input1.As<AnimationCurve>().postWrapMode=input2.As<WrapMode>());
            module.RegisterMethod("Evaluate", (result, input1, input2) => result.SetValue(input1.As<AnimationCurve>().Evaluate(input2.As<float>())));
            module.RegisterMethod("AddKey", Types(typeof(float),typeof(float)),(result, input1, input2, input3) => result.SetValue(input1.As<AnimationCurve>().AddKey(input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("AddKey", Types(typeof(Keyframe)),(result, input1, input2) => result.SetValue(input1.As<AnimationCurve>().AddKey(input2.As<Keyframe>())));
            module.RegisterMethod("MoveKey", (result, input1, input2, input3) => result.SetValue(input1.As<AnimationCurve>().MoveKey(input2.As<int>(),input3.As<Keyframe>())));
            module.RegisterMethod("RemoveKey", (input1, input2) => input1.As<AnimationCurve>().RemoveKey(input2.As<int>()));
            module.RegisterMethod("get_Item", (result, input1, input2) => result.SetValue(input1.As<AnimationCurve>()[input2.As<int>()]));
            module.RegisterMethod("SmoothTangents", (input1, input2, input3)  =>  input1.As<AnimationCurve>().SmoothTangents(input2.As<int>(),input3.As<float>()));
            module.RegisterMethod("Constant", (result, input1, input2, input3) => result.SetValue(AnimationCurve.Constant(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("Linear", (result, input1, input2, input3, input4) => result.SetValue(AnimationCurve.Linear(input1.As<float>(),input2.As<float>(),input3.As<float>(),input4.As<float>())));
            module.RegisterMethod("EaseInOut", (result, input1, input2, input3, input4) => result.SetValue(AnimationCurve.EaseInOut(input1.As<float>(),input2.As<float>(),input3.As<float>(),input4.As<float>())));
            module.RegisterMethod("Equals", Types(typeof(object)),(result, input1, input2) => result.SetValue(input1.As<AnimationCurve>().Equals(input2.As<object>())));
            module.RegisterMethod("Equals", Types(typeof(AnimationCurve)),(result, input1, input2) => result.SetValue(input1.As<AnimationCurve>().Equals(input2.As<AnimationCurve>())));
            module.RegisterMethod("GetHashCode", (result, input1) => result.SetValue(input1.As<AnimationCurve>().GetHashCode()));
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}