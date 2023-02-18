using static YS.Modules.ModuleLibrary;
namespace YS.Generated {
    public static partial class Wrapper {
        static void RegisterModules() {
             Register(new Variable<UnityEngine.MonoBehaviour>(),()=>Create_UnityEngine_MonoBehaviourModule(typeof(UnityEngine.Behaviour)));
             Register(new Variable<UnityEngine.Behaviour>(),()=>Create_UnityEngine_BehaviourModule(typeof(UnityEngine.Component)));
             Register(new Variable<UnityEngine.Component>(),()=>Create_UnityEngine_ComponentModule(typeof(UnityEngine.Object)));
             Register(new Variable<UnityEngine.Object>(),()=>Create_UnityEngine_ObjectModule());
             Register(new Variable<UnityEngine.Vector3>(),()=>Create_UnityEngine_Vector3Module());
             Register(new Variable<UnityEngine.Mathf>(),()=>Create_UnityEngine_MathfModule());
             Register(new Variable<UnityEngine.Transform>(),()=>Create_UnityEngine_TransformModule(typeof(UnityEngine.Component)));
             Register(new Variable<UnityEngine.GameObject>(),()=>Create_UnityEngine_GameObjectModule(typeof(UnityEngine.Object)));
             Register(new Variable<System.Enum>(),()=>Create_System_EnumModule());
             Register(new Variable<UnityEngine.Color>(),()=>Create_UnityEngine_ColorModule());
             Register(new Variable<UnityEngine.AnimationCurve>(),()=>Create_UnityEngine_AnimationCurveModule());
             Register(new Variable<YS.Timer>(),()=>Create_YS_TimerModule());
             Register(new Variable<YS.EasingType>(),()=>Create_YS_EasingTypeModule(typeof(System.Enum)));
             Register(new Variable<UnityEngine.Time>(),()=>Create_UnityEngine_TimeModule());
             Register(new Variable<UnityEngine.SpriteRenderer>(),()=>Create_UnityEngine_SpriteRendererModule(typeof(UnityEngine.Renderer)));
             Register(new Variable<UnityEngine.Renderer>(),()=>Create_UnityEngine_RendererModule(typeof(UnityEngine.Component)));
        }
    }
}
