using Time = UnityEngine.Time;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_TimeModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Time));
            module.RegisterConstructor(result => result.SetValue( new Time()));
            module.RegisterPropertyGetter("time", result => result.SetValue(Time.time));
            module.RegisterPropertyGetter("timeAsDouble", result => result.SetValue(Time.timeAsDouble));
            module.RegisterPropertyGetter("timeSinceLevelLoad", result => result.SetValue(Time.timeSinceLevelLoad));
            module.RegisterPropertyGetter("timeSinceLevelLoadAsDouble", result => result.SetValue(Time.timeSinceLevelLoadAsDouble));
            module.RegisterPropertyGetter("deltaTime", result => result.SetValue(Time.deltaTime));
            module.RegisterPropertyGetter("fixedTime", result => result.SetValue(Time.fixedTime));
            module.RegisterPropertyGetter("fixedTimeAsDouble", result => result.SetValue(Time.fixedTimeAsDouble));
            module.RegisterPropertyGetter("unscaledTime", result => result.SetValue(Time.unscaledTime));
            module.RegisterPropertyGetter("unscaledTimeAsDouble", result => result.SetValue(Time.unscaledTimeAsDouble));
            module.RegisterPropertyGetter("fixedUnscaledTime", result => result.SetValue(Time.fixedUnscaledTime));
            module.RegisterPropertyGetter("fixedUnscaledTimeAsDouble", result => result.SetValue(Time.fixedUnscaledTimeAsDouble));
            module.RegisterPropertyGetter("unscaledDeltaTime", result => result.SetValue(Time.unscaledDeltaTime));
            module.RegisterPropertyGetter("fixedUnscaledDeltaTime", result => result.SetValue(Time.fixedUnscaledDeltaTime));
            module.RegisterPropertyGetter("fixedDeltaTime", result => result.SetValue(Time.fixedDeltaTime));
            module.RegisterPropertySetter("fixedDeltaTime", input1 => Time.fixedDeltaTime=input1.As<float>());
            module.RegisterPropertyGetter("maximumDeltaTime", result => result.SetValue(Time.maximumDeltaTime));
            module.RegisterPropertySetter("maximumDeltaTime", input1 => Time.maximumDeltaTime=input1.As<float>());
            module.RegisterPropertyGetter("smoothDeltaTime", result => result.SetValue(Time.smoothDeltaTime));
            module.RegisterPropertyGetter("maximumParticleDeltaTime", result => result.SetValue(Time.maximumParticleDeltaTime));
            module.RegisterPropertySetter("maximumParticleDeltaTime", input1 => Time.maximumParticleDeltaTime=input1.As<float>());
            module.RegisterPropertyGetter("timeScale", result => result.SetValue(Time.timeScale));
            module.RegisterPropertySetter("timeScale", input1 => Time.timeScale=input1.As<float>());
            module.RegisterPropertyGetter("frameCount", result => result.SetValue(Time.frameCount));
            module.RegisterPropertyGetter("renderedFrameCount", result => result.SetValue(Time.renderedFrameCount));
            module.RegisterPropertyGetter("realtimeSinceStartup", result => result.SetValue(Time.realtimeSinceStartup));
            module.RegisterPropertyGetter("realtimeSinceStartupAsDouble", result => result.SetValue(Time.realtimeSinceStartupAsDouble));
            module.RegisterPropertyGetter("captureDeltaTime", result => result.SetValue(Time.captureDeltaTime));
            module.RegisterPropertySetter("captureDeltaTime", input1 => Time.captureDeltaTime=input1.As<float>());
            module.RegisterPropertyGetter("captureFramerate", result => result.SetValue(Time.captureFramerate));
            module.RegisterPropertySetter("captureFramerate", input1 => Time.captureFramerate=input1.As<int>());
            module.RegisterPropertyGetter("inFixedTimeStep", result => result.SetValue(Time.inFixedTimeStep));
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}