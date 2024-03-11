#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif
using Time = UnityEngine.Time;
using _Time = YS.Variable<UnityEngine.Time>;
using _float = YS.Variable<float>;
using _double = YS.Variable<double>;
using _int = YS.Variable<int>;
using _bool = YS.Variable<bool>;

using static System.Runtime.CompilerServices.Unsafe;
using __V = YS.Variable;
namespace YS.Generated {
    public static partial  class Wrapper {
        static void UnityEngine_Time_ctor(__V res) =>  As<_Time>(res).value = new Time();
       static void UnityEngine_Time_get_time(__V res) =>  As<_float>(res).value=Time.time;
       static void UnityEngine_Time_get_timeAsDouble(__V res) =>  As<_double>(res).value=Time.timeAsDouble;
       static void UnityEngine_Time_get_timeSinceLevelLoad(__V res) =>  As<_float>(res).value=Time.timeSinceLevelLoad;
       static void UnityEngine_Time_get_timeSinceLevelLoadAsDouble(__V res) =>  As<_double>(res).value=Time.timeSinceLevelLoadAsDouble;
       static void UnityEngine_Time_get_deltaTime(__V res) =>  As<_float>(res).value=Time.deltaTime;
       static void UnityEngine_Time_get_fixedTime(__V res) =>  As<_float>(res).value=Time.fixedTime;
       static void UnityEngine_Time_get_fixedTimeAsDouble(__V res) =>  As<_double>(res).value=Time.fixedTimeAsDouble;
       static void UnityEngine_Time_get_unscaledTime(__V res) =>  As<_float>(res).value=Time.unscaledTime;
       static void UnityEngine_Time_get_unscaledTimeAsDouble(__V res) =>  As<_double>(res).value=Time.unscaledTimeAsDouble;
       static void UnityEngine_Time_get_fixedUnscaledTime(__V res) =>  As<_float>(res).value=Time.fixedUnscaledTime;
       static void UnityEngine_Time_get_fixedUnscaledTimeAsDouble(__V res) =>  As<_double>(res).value=Time.fixedUnscaledTimeAsDouble;
       static void UnityEngine_Time_get_unscaledDeltaTime(__V res) =>  As<_float>(res).value=Time.unscaledDeltaTime;
       static void UnityEngine_Time_get_fixedUnscaledDeltaTime(__V res) =>  As<_float>(res).value=Time.fixedUnscaledDeltaTime;
       static void UnityEngine_Time_get_fixedDeltaTime(__V res) =>  As<_float>(res).value=Time.fixedDeltaTime;
       static void UnityEngine_Time_set_fixedDeltaTime(__V  i1) => Time.fixedDeltaTime = As<_float>(i1).value;
       static void UnityEngine_Time_get_maximumDeltaTime(__V res) =>  As<_float>(res).value=Time.maximumDeltaTime;
       static void UnityEngine_Time_set_maximumDeltaTime(__V  i1) => Time.maximumDeltaTime = As<_float>(i1).value;
       static void UnityEngine_Time_get_smoothDeltaTime(__V res) =>  As<_float>(res).value=Time.smoothDeltaTime;
       static void UnityEngine_Time_get_maximumParticleDeltaTime(__V res) =>  As<_float>(res).value=Time.maximumParticleDeltaTime;
       static void UnityEngine_Time_set_maximumParticleDeltaTime(__V  i1) => Time.maximumParticleDeltaTime = As<_float>(i1).value;
       static void UnityEngine_Time_get_timeScale(__V res) =>  As<_float>(res).value=Time.timeScale;
       static void UnityEngine_Time_set_timeScale(__V  i1) => Time.timeScale = As<_float>(i1).value;
       static void UnityEngine_Time_get_frameCount(__V res) =>  As<_int>(res).value=Time.frameCount;
       static void UnityEngine_Time_get_renderedFrameCount(__V res) =>  As<_int>(res).value=Time.renderedFrameCount;
       static void UnityEngine_Time_get_realtimeSinceStartup(__V res) =>  As<_float>(res).value=Time.realtimeSinceStartup;
       static void UnityEngine_Time_get_realtimeSinceStartupAsDouble(__V res) =>  As<_double>(res).value=Time.realtimeSinceStartupAsDouble;
       static void UnityEngine_Time_get_captureDeltaTime(__V res) =>  As<_float>(res).value=Time.captureDeltaTime;
       static void UnityEngine_Time_set_captureDeltaTime(__V  i1) => Time.captureDeltaTime = As<_float>(i1).value;
       static void UnityEngine_Time_get_captureFramerate(__V res) =>  As<_int>(res).value=Time.captureFramerate;
       static void UnityEngine_Time_set_captureFramerate(__V  i1) => Time.captureFramerate = As<_int>(i1).value;
       static void UnityEngine_Time_get_inFixedTimeStep(__V res) =>  As<_bool>(res).value=Time.inFixedTimeStep;
#if !AOT

        static TypeModule Create_UnityEngine_TimeModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Time));
            module.RegisterConstructor(UnityEngine_Time_ctor);
            module.RegisterPropertyGetter("time", UnityEngine_Time_get_time);
            module.RegisterPropertyGetter("timeAsDouble", UnityEngine_Time_get_timeAsDouble);
            module.RegisterPropertyGetter("timeSinceLevelLoad", UnityEngine_Time_get_timeSinceLevelLoad);
            module.RegisterPropertyGetter("timeSinceLevelLoadAsDouble", UnityEngine_Time_get_timeSinceLevelLoadAsDouble);
            module.RegisterPropertyGetter("deltaTime", UnityEngine_Time_get_deltaTime);
            module.RegisterPropertyGetter("fixedTime", UnityEngine_Time_get_fixedTime);
            module.RegisterPropertyGetter("fixedTimeAsDouble", UnityEngine_Time_get_fixedTimeAsDouble);
            module.RegisterPropertyGetter("unscaledTime", UnityEngine_Time_get_unscaledTime);
            module.RegisterPropertyGetter("unscaledTimeAsDouble", UnityEngine_Time_get_unscaledTimeAsDouble);
            module.RegisterPropertyGetter("fixedUnscaledTime", UnityEngine_Time_get_fixedUnscaledTime);
            module.RegisterPropertyGetter("fixedUnscaledTimeAsDouble", UnityEngine_Time_get_fixedUnscaledTimeAsDouble);
            module.RegisterPropertyGetter("unscaledDeltaTime", UnityEngine_Time_get_unscaledDeltaTime);
            module.RegisterPropertyGetter("fixedUnscaledDeltaTime", UnityEngine_Time_get_fixedUnscaledDeltaTime);
            module.RegisterPropertyGetter("fixedDeltaTime", UnityEngine_Time_get_fixedDeltaTime);
            module.RegisterPropertySetter("fixedDeltaTime", UnityEngine_Time_set_fixedDeltaTime);
            module.RegisterPropertyGetter("maximumDeltaTime", UnityEngine_Time_get_maximumDeltaTime);
            module.RegisterPropertySetter("maximumDeltaTime", UnityEngine_Time_set_maximumDeltaTime);
            module.RegisterPropertyGetter("smoothDeltaTime", UnityEngine_Time_get_smoothDeltaTime);
            module.RegisterPropertyGetter("maximumParticleDeltaTime", UnityEngine_Time_get_maximumParticleDeltaTime);
            module.RegisterPropertySetter("maximumParticleDeltaTime", UnityEngine_Time_set_maximumParticleDeltaTime);
            module.RegisterPropertyGetter("timeScale", UnityEngine_Time_get_timeScale);
            module.RegisterPropertySetter("timeScale", UnityEngine_Time_set_timeScale);
            module.RegisterPropertyGetter("frameCount", UnityEngine_Time_get_frameCount);
            module.RegisterPropertyGetter("renderedFrameCount", UnityEngine_Time_get_renderedFrameCount);
            module.RegisterPropertyGetter("realtimeSinceStartup", UnityEngine_Time_get_realtimeSinceStartup);
            module.RegisterPropertyGetter("realtimeSinceStartupAsDouble", UnityEngine_Time_get_realtimeSinceStartupAsDouble);
            module.RegisterPropertyGetter("captureDeltaTime", UnityEngine_Time_get_captureDeltaTime);
            module.RegisterPropertySetter("captureDeltaTime", UnityEngine_Time_set_captureDeltaTime);
            module.RegisterPropertyGetter("captureFramerate", UnityEngine_Time_get_captureFramerate);
            module.RegisterPropertySetter("captureFramerate", UnityEngine_Time_set_captureFramerate);
            module.RegisterPropertyGetter("inFixedTimeStep", UnityEngine_Time_get_inFixedTimeStep);
#else
        static unsafe TypeModule Create_UnityEngine_TimeModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Time));
            module.RegisterConstructor(&UnityEngine_Time_ctor);
            module.RegisterPropertyGetter("time", &UnityEngine_Time_get_time);
            module.RegisterPropertyGetter("timeAsDouble", &UnityEngine_Time_get_timeAsDouble);
            module.RegisterPropertyGetter("timeSinceLevelLoad", &UnityEngine_Time_get_timeSinceLevelLoad);
            module.RegisterPropertyGetter("timeSinceLevelLoadAsDouble", &UnityEngine_Time_get_timeSinceLevelLoadAsDouble);
            module.RegisterPropertyGetter("deltaTime", &UnityEngine_Time_get_deltaTime);
            module.RegisterPropertyGetter("fixedTime", &UnityEngine_Time_get_fixedTime);
            module.RegisterPropertyGetter("fixedTimeAsDouble", &UnityEngine_Time_get_fixedTimeAsDouble);
            module.RegisterPropertyGetter("unscaledTime", &UnityEngine_Time_get_unscaledTime);
            module.RegisterPropertyGetter("unscaledTimeAsDouble", &UnityEngine_Time_get_unscaledTimeAsDouble);
            module.RegisterPropertyGetter("fixedUnscaledTime", &UnityEngine_Time_get_fixedUnscaledTime);
            module.RegisterPropertyGetter("fixedUnscaledTimeAsDouble", &UnityEngine_Time_get_fixedUnscaledTimeAsDouble);
            module.RegisterPropertyGetter("unscaledDeltaTime", &UnityEngine_Time_get_unscaledDeltaTime);
            module.RegisterPropertyGetter("fixedUnscaledDeltaTime", &UnityEngine_Time_get_fixedUnscaledDeltaTime);
            module.RegisterPropertyGetter("fixedDeltaTime", &UnityEngine_Time_get_fixedDeltaTime);
            module.RegisterPropertySetter("fixedDeltaTime",& UnityEngine_Time_set_fixedDeltaTime);
            module.RegisterPropertyGetter("maximumDeltaTime", &UnityEngine_Time_get_maximumDeltaTime);
            module.RegisterPropertySetter("maximumDeltaTime",& UnityEngine_Time_set_maximumDeltaTime);
            module.RegisterPropertyGetter("smoothDeltaTime", &UnityEngine_Time_get_smoothDeltaTime);
            module.RegisterPropertyGetter("maximumParticleDeltaTime", &UnityEngine_Time_get_maximumParticleDeltaTime);
            module.RegisterPropertySetter("maximumParticleDeltaTime",& UnityEngine_Time_set_maximumParticleDeltaTime);
            module.RegisterPropertyGetter("timeScale", &UnityEngine_Time_get_timeScale);
            module.RegisterPropertySetter("timeScale",& UnityEngine_Time_set_timeScale);
            module.RegisterPropertyGetter("frameCount", &UnityEngine_Time_get_frameCount);
            module.RegisterPropertyGetter("renderedFrameCount", &UnityEngine_Time_get_renderedFrameCount);
            module.RegisterPropertyGetter("realtimeSinceStartup", &UnityEngine_Time_get_realtimeSinceStartup);
            module.RegisterPropertyGetter("realtimeSinceStartupAsDouble", &UnityEngine_Time_get_realtimeSinceStartupAsDouble);
            module.RegisterPropertyGetter("captureDeltaTime", &UnityEngine_Time_get_captureDeltaTime);
            module.RegisterPropertySetter("captureDeltaTime",& UnityEngine_Time_set_captureDeltaTime);
            module.RegisterPropertyGetter("captureFramerate", &UnityEngine_Time_get_captureFramerate);
            module.RegisterPropertySetter("captureFramerate",& UnityEngine_Time_set_captureFramerate);
            module.RegisterPropertyGetter("inFixedTimeStep", &UnityEngine_Time_get_inFixedTimeStep);
#endif
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}