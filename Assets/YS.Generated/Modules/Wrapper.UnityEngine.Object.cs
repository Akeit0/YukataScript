#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif
using Object = UnityEngine.Object;
using _Object = YS.Variable<UnityEngine.Object>;
using _string = YS.Variable<string>;
using HideFlags = UnityEngine.HideFlags;
using _HideFlags = YS.Variable<UnityEngine.HideFlags>;
using _int = YS.Variable<int>;
using _bool = YS.Variable<bool>;
using _object = YS.Variable<object>;
using Vector3 = UnityEngine.Vector3;
using _Vector3 = YS.Variable<UnityEngine.Vector3>;
using Quaternion = UnityEngine.Quaternion;
using _Quaternion = YS.Variable<UnityEngine.Quaternion>;
using Transform = UnityEngine.Transform;
using _Transform = YS.Variable<UnityEngine.Transform>;
using _float = YS.Variable<float>;
using Type = System.Type;
using _Type = YS.Variable<System.Type>;
using FindObjectsSortMode = UnityEngine.FindObjectsSortMode;
using _FindObjectsSortMode = YS.Variable<UnityEngine.FindObjectsSortMode>;
using FindObjectsInactive = UnityEngine.FindObjectsInactive;
using _FindObjectsInactive = YS.Variable<UnityEngine.FindObjectsInactive>;

using static System.Runtime.CompilerServices.Unsafe;
using __V = YS.Variable;
namespace YS.Generated {
    public static partial  class Wrapper {
        static void UnityEngine_Object_ctor(__V res) =>  As<_Object>(res).value = new Object();
       static void UnityEngine_Object_get_name(__V res, __V  i1) => As<_string>(res).value=As<_Object>(i1).value.name;
       static void UnityEngine_Object_set_name(__V  i1, __V  i2) => As<_Object>(i1).value.name = As<_string>(i2).value;
       static void UnityEngine_Object_get_hideFlags(__V res, __V  i1) => As<_HideFlags>(res).value=As<_Object>(i1).value.hideFlags;
       static void UnityEngine_Object_set_hideFlags(__V  i1, __V  i2) => As<_Object>(i1).value.hideFlags = As<_HideFlags>(i2).value;
       static void UnityEngine_Object_GetInstanceID(__V res, __V  i1) => As<_int>(res).value=As<_Object>(i1).value.GetInstanceID();
       static void UnityEngine_Object_GetHashCode(__V res, __V  i1) => As<_int>(res).value=As<_Object>(i1).value.GetHashCode();
       static void UnityEngine_Object_Equals(__V res,__V  i1, __V  i2) => As<_bool>(res).value=As<_Object>(i1).value.Equals(As<_object>(i2).value);
       static void UnityEngine_Object_Instantiate(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_Object>(res).value=Object.Instantiate(As<_Object>(i1).value,As<_Vector3>(i2).value,As<_Quaternion>(i3).value);
       static void UnityEngine_Object_Instantiate_(__V res,__V  i1, __V  i2, __V  i3, __V  i4)  => As<_Object>(res).value=Object.Instantiate(As<_Object>(i1).value,As<_Vector3>(i2).value,As<_Quaternion>(i3).value,As<_Transform>(i4).value);
       static void UnityEngine_Object_Instantiate__(__V res, __V  i1) => As<_Object>(res).value=Object.Instantiate(As<_Object>(i1).value);
       static void UnityEngine_Object_Instantiate___(__V res,__V  i1, __V  i2) => As<_Object>(res).value=Object.Instantiate(As<_Object>(i1).value,As<_Transform>(i2).value);
       static void UnityEngine_Object_Instantiate____(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_Object>(res).value=Object.Instantiate(As<_Object>(i1).value,As<_Transform>(i2).value,As<_bool>(i3).value);
       static void UnityEngine_Object_Destroy(__V  i1, __V  i2) => Object.Destroy(As<_Object>(i1).value,As<_float>(i2).value);
       static void UnityEngine_Object_Destroy_(__V  i1) => Object.Destroy(As<_Object>(i1).value);
       static void UnityEngine_Object_DestroyImmediate(__V  i1, __V  i2) => Object.DestroyImmediate(As<_Object>(i1).value,As<_bool>(i2).value);
       static void UnityEngine_Object_DestroyImmediate_(__V  i1) => Object.DestroyImmediate(As<_Object>(i1).value);
       static void UnityEngine_Object_FindObjectsOfType(__V res, __V  i1) => As<Variable<Object[]>>(res).value=Object.FindObjectsOfType(As<_Type>(i1).value);
       static void UnityEngine_Object_FindObjectsOfType_(__V res,__V  i1, __V  i2) => As<Variable<Object[]>>(res).value=Object.FindObjectsOfType(As<_Type>(i1).value,As<_bool>(i2).value);
       static void UnityEngine_Object_FindObjectsByType(__V res,__V  i1, __V  i2) => As<Variable<Object[]>>(res).value=Object.FindObjectsByType(As<_Type>(i1).value,As<_FindObjectsSortMode>(i2).value);
       static void UnityEngine_Object_FindObjectsByType_(__V res,__V  i1, __V  i2, __V  i3)  =>  As<Variable<Object[]>>(res).value=Object.FindObjectsByType(As<_Type>(i1).value,As<_FindObjectsInactive>(i2).value,As<_FindObjectsSortMode>(i3).value);
       static void UnityEngine_Object_DontDestroyOnLoad(__V  i1) => Object.DontDestroyOnLoad(As<_Object>(i1).value);
       static void UnityEngine_Object_FindObjectOfType(__V res, __V  i1) => As<_Object>(res).value=Object.FindObjectOfType(As<_Type>(i1).value);
       static void UnityEngine_Object_FindObjectOfType_(__V res,__V  i1, __V  i2) => As<_Object>(res).value=Object.FindObjectOfType(As<_Type>(i1).value,As<_bool>(i2).value);
       static void UnityEngine_Object_FindFirstObjectByType(__V res, __V  i1) => As<_Object>(res).value=Object.FindFirstObjectByType(As<_Type>(i1).value);
       static void UnityEngine_Object_FindFirstObjectByType_(__V res,__V  i1, __V  i2) => As<_Object>(res).value=Object.FindFirstObjectByType(As<_Type>(i1).value,As<_FindObjectsInactive>(i2).value);
       static void UnityEngine_Object_FindAnyObjectByType(__V res, __V  i1) => As<_Object>(res).value=Object.FindAnyObjectByType(As<_Type>(i1).value);
       static void UnityEngine_Object_FindAnyObjectByType_(__V res,__V  i1, __V  i2) => As<_Object>(res).value=Object.FindAnyObjectByType(As<_Type>(i1).value,As<_FindObjectsInactive>(i2).value);
       static void UnityEngine_Object_ToString(__V res, __V  i1) => As<_string>(res).value=As<_Object>(i1).value.ToString();
       static void UnityEngine_Object_op_Equality(__V res,__V  i1, __V  i2) => As<_bool>(res).value=As<_Object>(i1).value==As<_Object>(i2).value;
       static void UnityEngine_Object_op_Inequality(__V res,__V  i1, __V  i2) => As<_bool>(res).value=As<_Object>(i1).value!=As<_Object>(i2).value;
#if !AOT

        static TypeModule Create_UnityEngine_ObjectModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Object));
            module.RegisterConstructor(UnityEngine_Object_ctor);
            module.RegisterPropertyGetter("name", UnityEngine_Object_get_name);
            module.RegisterPropertySetter("name", UnityEngine_Object_set_name);
            module.RegisterPropertyGetter("hideFlags", UnityEngine_Object_get_hideFlags);
            module.RegisterPropertySetter("hideFlags", UnityEngine_Object_set_hideFlags);
            module.RegisterMethod2("GetInstanceID",UnityEngine_Object_GetInstanceID);
            module.RegisterMethod2("GetHashCode",UnityEngine_Object_GetHashCode);
            module.RegisterMethod3("Equals",UnityEngine_Object_Equals);
            module.RegisterMethod4("Instantiate", Types(typeof(Object),typeof(Vector3),typeof(Quaternion)),UnityEngine_Object_Instantiate);
            module.RegisterMethod5("Instantiate", Types(typeof(Object),typeof(Vector3),typeof(Quaternion),typeof(Transform)),UnityEngine_Object_Instantiate_);
            module.RegisterMethod2("Instantiate", Types(typeof(Object)),UnityEngine_Object_Instantiate__);
            module.RegisterMethod3("Instantiate", Types(typeof(Object),typeof(Transform)),UnityEngine_Object_Instantiate___);
            module.RegisterMethod4("Instantiate", Types(typeof(Object),typeof(Transform),typeof(bool)),UnityEngine_Object_Instantiate____);
            module.RegisterMethod2("Destroy", Types(typeof(Object),typeof(float)),UnityEngine_Object_Destroy);
            module.RegisterMethod1("Destroy", Types(typeof(Object)),UnityEngine_Object_Destroy_);
            module.RegisterMethod2("DestroyImmediate", Types(typeof(Object),typeof(bool)),UnityEngine_Object_DestroyImmediate);
            module.RegisterMethod1("DestroyImmediate", Types(typeof(Object)),UnityEngine_Object_DestroyImmediate_);
            module.RegisterMethod2("FindObjectsOfType", Types(typeof(Type)),UnityEngine_Object_FindObjectsOfType);
            module.RegisterMethod3("FindObjectsOfType", Types(typeof(Type),typeof(bool)),UnityEngine_Object_FindObjectsOfType_);
            module.RegisterMethod3("FindObjectsByType", Types(typeof(Type),typeof(FindObjectsSortMode)),UnityEngine_Object_FindObjectsByType);
            module.RegisterMethod4("FindObjectsByType", Types(typeof(Type),typeof(FindObjectsInactive),typeof(FindObjectsSortMode)),UnityEngine_Object_FindObjectsByType_);
            module.RegisterMethod1("DontDestroyOnLoad",UnityEngine_Object_DontDestroyOnLoad);
            module.RegisterMethod2("FindObjectOfType", Types(typeof(Type)),UnityEngine_Object_FindObjectOfType);
            module.RegisterMethod3("FindObjectOfType", Types(typeof(Type),typeof(bool)),UnityEngine_Object_FindObjectOfType_);
            module.RegisterMethod2("FindFirstObjectByType", Types(typeof(Type)),UnityEngine_Object_FindFirstObjectByType);
            module.RegisterMethod3("FindFirstObjectByType", Types(typeof(Type),typeof(FindObjectsInactive)),UnityEngine_Object_FindFirstObjectByType_);
            module.RegisterMethod2("FindAnyObjectByType", Types(typeof(Type)),UnityEngine_Object_FindAnyObjectByType);
            module.RegisterMethod3("FindAnyObjectByType", Types(typeof(Type),typeof(FindObjectsInactive)),UnityEngine_Object_FindAnyObjectByType_);
            module.RegisterMethod2("ToString",UnityEngine_Object_ToString);
            module.RegisterMethod3("op_Equality", UnityEngine_Object_op_Equality);
            module.RegisterMethod3("op_Inequality", UnityEngine_Object_op_Inequality);
#else
        static unsafe TypeModule Create_UnityEngine_ObjectModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Object));
            module.RegisterConstructor(&UnityEngine_Object_ctor);
            module.RegisterPropertyGetter("name", &UnityEngine_Object_get_name);
            module.RegisterPropertySetter("name",& UnityEngine_Object_set_name);
            module.RegisterPropertyGetter("hideFlags", &UnityEngine_Object_get_hideFlags);
            module.RegisterPropertySetter("hideFlags",& UnityEngine_Object_set_hideFlags);
            module.RegisterMethod2("GetInstanceID",&UnityEngine_Object_GetInstanceID);
            module.RegisterMethod2("GetHashCode",&UnityEngine_Object_GetHashCode);
            module.RegisterMethod3("Equals",&UnityEngine_Object_Equals);
            module.RegisterMethod4("Instantiate", Types(typeof(Object),typeof(Vector3),typeof(Quaternion)),&UnityEngine_Object_Instantiate);
            module.RegisterMethod5("Instantiate", Types(typeof(Object),typeof(Vector3),typeof(Quaternion),typeof(Transform)),&UnityEngine_Object_Instantiate_);
            module.RegisterMethod2("Instantiate", Types(typeof(Object)),&UnityEngine_Object_Instantiate__);
            module.RegisterMethod3("Instantiate", Types(typeof(Object),typeof(Transform)),&UnityEngine_Object_Instantiate___);
            module.RegisterMethod4("Instantiate", Types(typeof(Object),typeof(Transform),typeof(bool)),&UnityEngine_Object_Instantiate____);
            module.RegisterMethod2("Destroy", Types(typeof(Object),typeof(float)),&UnityEngine_Object_Destroy);
            module.RegisterMethod1("Destroy", Types(typeof(Object)),&UnityEngine_Object_Destroy_);
            module.RegisterMethod2("DestroyImmediate", Types(typeof(Object),typeof(bool)),&UnityEngine_Object_DestroyImmediate);
            module.RegisterMethod1("DestroyImmediate", Types(typeof(Object)),&UnityEngine_Object_DestroyImmediate_);
            module.RegisterMethod2("FindObjectsOfType", Types(typeof(Type)),&UnityEngine_Object_FindObjectsOfType);
            module.RegisterMethod3("FindObjectsOfType", Types(typeof(Type),typeof(bool)),&UnityEngine_Object_FindObjectsOfType_);
            module.RegisterMethod3("FindObjectsByType", Types(typeof(Type),typeof(FindObjectsSortMode)),&UnityEngine_Object_FindObjectsByType);
            module.RegisterMethod4("FindObjectsByType", Types(typeof(Type),typeof(FindObjectsInactive),typeof(FindObjectsSortMode)),&UnityEngine_Object_FindObjectsByType_);
            module.RegisterMethod1("DontDestroyOnLoad",&UnityEngine_Object_DontDestroyOnLoad);
            module.RegisterMethod2("FindObjectOfType", Types(typeof(Type)),&UnityEngine_Object_FindObjectOfType);
            module.RegisterMethod3("FindObjectOfType", Types(typeof(Type),typeof(bool)),&UnityEngine_Object_FindObjectOfType_);
            module.RegisterMethod2("FindFirstObjectByType", Types(typeof(Type)),&UnityEngine_Object_FindFirstObjectByType);
            module.RegisterMethod3("FindFirstObjectByType", Types(typeof(Type),typeof(FindObjectsInactive)),&UnityEngine_Object_FindFirstObjectByType_);
            module.RegisterMethod2("FindAnyObjectByType", Types(typeof(Type)),&UnityEngine_Object_FindAnyObjectByType);
            module.RegisterMethod3("FindAnyObjectByType", Types(typeof(Type),typeof(FindObjectsInactive)),&UnityEngine_Object_FindAnyObjectByType_);
            module.RegisterMethod2("ToString",&UnityEngine_Object_ToString);
            module.RegisterMethod3("op_Equality", &UnityEngine_Object_op_Equality);
            module.RegisterMethod3("op_Inequality", &UnityEngine_Object_op_Inequality);
#endif
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}