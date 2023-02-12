using Object = UnityEngine.Object;
using HideFlags = UnityEngine.HideFlags;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Transform = UnityEngine.Transform;
using Type = System.Type;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_ObjectModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Object));
            module.RegisterConstructor(result => result.SetValue( new Object()));
            module.RegisterPropertyGetter("name", (result, input1) => result.SetValue(input1.As<Object>().name));
            module.RegisterPropertySetter("name", (input1, input2) => input1.As<Object>().name=input2.As<string>());
            module.RegisterPropertyGetter("hideFlags", (result, input1) => result.SetValue(input1.As<Object>().hideFlags));
            module.RegisterPropertySetter("hideFlags", (input1, input2) => input1.As<Object>().hideFlags=input2.As<HideFlags>());
            module.RegisterMethod("GetInstanceID", (result, input1) => result.SetValue(input1.As<Object>().GetInstanceID()));
            module.RegisterMethod("GetHashCode", (result, input1) => result.SetValue(input1.As<Object>().GetHashCode()));
            module.RegisterMethod("Equals", (result, input1, input2) => result.SetValue(input1.As<Object>().Equals(input2.As<object>())));
            module.RegisterMethod("Instantiate", Types(typeof(Object),typeof(Vector3),typeof(Quaternion)),(result, input1, input2, input3) => result.SetValue(Object.Instantiate(input1.As<Object>(),input2.As<Vector3>(),input3.As<Quaternion>())));
            module.RegisterMethod("Instantiate", Types(typeof(Object),typeof(Vector3),typeof(Quaternion),typeof(Transform)),(result, input1, input2, input3, input4) => result.SetValue(Object.Instantiate(input1.As<Object>(),input2.As<Vector3>(),input3.As<Quaternion>(),input4.As<Transform>())));
            module.RegisterMethod("Instantiate", Types(typeof(Object)),(result, input1) => result.SetValue(Object.Instantiate(input1.As<Object>())));
            module.RegisterMethod("Instantiate", Types(typeof(Object),typeof(Transform)),(result, input1, input2) => result.SetValue(Object.Instantiate(input1.As<Object>(),input2.As<Transform>())));
            module.RegisterMethod("Instantiate", Types(typeof(Object),typeof(Transform),typeof(bool)),(result, input1, input2, input3) => result.SetValue(Object.Instantiate(input1.As<Object>(),input2.As<Transform>(),input3.As<bool>())));
            module.RegisterMethod("Destroy", Types(typeof(Object),typeof(float)),(input1, input2) => Object.Destroy(input1.As<Object>(),input2.As<float>()));
            module.RegisterMethod("Destroy", Types(typeof(Object)),input1 => Object.Destroy(input1.As<Object>()));
            module.RegisterMethod("DestroyImmediate", Types(typeof(Object),typeof(bool)),(input1, input2) => Object.DestroyImmediate(input1.As<Object>(),input2.As<bool>()));
            module.RegisterMethod("DestroyImmediate", Types(typeof(Object)),input1 => Object.DestroyImmediate(input1.As<Object>()));
            module.RegisterMethod("FindObjectsOfType", Types(typeof(Type)),(result, input1) => result.SetValue(Object.FindObjectsOfType(input1.As<Type>())));
            module.RegisterMethod("FindObjectsOfType", Types(typeof(Type),typeof(bool)),(result, input1, input2) => result.SetValue(Object.FindObjectsOfType(input1.As<Type>(),input2.As<bool>())));
            module.RegisterMethod("DontDestroyOnLoad", input1 => Object.DontDestroyOnLoad(input1.As<Object>()));
            module.RegisterMethod("FindObjectOfType", Types(typeof(Type)),(result, input1) => result.SetValue(Object.FindObjectOfType(input1.As<Type>())));
            module.RegisterMethod("FindObjectOfType", Types(typeof(Type),typeof(bool)),(result, input1, input2) => result.SetValue(Object.FindObjectOfType(input1.As<Type>(),input2.As<bool>())));
            module.RegisterMethod("ToString", (result, input1) => result.SetValue(input1.As<Object>().ToString()));
            module.RegisterMethod("op_Equality", (result, input1, input2) => result.SetValue(input1.As<Object>()==input2.As<Object>()));
            module.RegisterMethod("op_Inequality", (result, input1, input2) => result.SetValue(input1.As<Object>()!=input2.As<Object>()));
            module.ClearTempMembers();
                          if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}