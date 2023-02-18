using EasingType = YS.EasingType;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_YS_EasingTypeModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(EasingType));
            module.RegisterConst("Linear",new Variable<EasingType>(EasingType.Linear));
            module.RegisterConst("InSine",new Variable<EasingType>(EasingType.InSine));
            module.RegisterConst("OutSine",new Variable<EasingType>(EasingType.OutSine));
            module.RegisterConst("InOutSine",new Variable<EasingType>(EasingType.InOutSine));
            module.RegisterConst("InQuad",new Variable<EasingType>(EasingType.InQuad));
            module.RegisterConst("OutQuad",new Variable<EasingType>(EasingType.OutQuad));
            module.RegisterConst("InOutQuad",new Variable<EasingType>(EasingType.InOutQuad));
            module.RegisterConst("InCubic",new Variable<EasingType>(EasingType.InCubic));
            module.RegisterConst("OutCubic",new Variable<EasingType>(EasingType.OutCubic));
            module.RegisterConst("InOutCubic",new Variable<EasingType>(EasingType.InOutCubic));
            module.RegisterConst("InQuart",new Variable<EasingType>(EasingType.InQuart));
            module.RegisterConst("OutQuart",new Variable<EasingType>(EasingType.OutQuart));
            module.RegisterConst("InOutQuart",new Variable<EasingType>(EasingType.InOutQuart));
            module.RegisterConst("InQuint",new Variable<EasingType>(EasingType.InQuint));
            module.RegisterConst("OutQuint",new Variable<EasingType>(EasingType.OutQuint));
            module.RegisterConst("InOutQuint",new Variable<EasingType>(EasingType.InOutQuint));
            module.RegisterConst("InExpo",new Variable<EasingType>(EasingType.InExpo));
            module.RegisterConst("OutExpo",new Variable<EasingType>(EasingType.OutExpo));
            module.RegisterConst("InOutExpo",new Variable<EasingType>(EasingType.InOutExpo));
            module.RegisterConst("InCirc",new Variable<EasingType>(EasingType.InCirc));
            module.RegisterConst("OutCirc",new Variable<EasingType>(EasingType.OutCirc));
            module.RegisterConst("InOutCirc",new Variable<EasingType>(EasingType.InOutCirc));
            module.RegisterConst("InElastic",new Variable<EasingType>(EasingType.InElastic));
            module.RegisterConst("OutElastic",new Variable<EasingType>(EasingType.OutElastic));
            module.RegisterConst("InOutElastic",new Variable<EasingType>(EasingType.InOutElastic));
            module.RegisterConst("InBack",new Variable<EasingType>(EasingType.InBack));
            module.RegisterConst("OutBack",new Variable<EasingType>(EasingType.OutBack));
            module.RegisterConst("InOutBack",new Variable<EasingType>(EasingType.InOutBack));
            module.RegisterConst("InBounce",new Variable<EasingType>(EasingType.InBounce));
            module.RegisterConst("OutBounce",new Variable<EasingType>(EasingType.OutBounce));
            module.RegisterConst("InOutBounce",new Variable<EasingType>(EasingType.InOutBounce));
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}