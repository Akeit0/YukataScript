#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif
using MonoBehaviour = UnityEngine.MonoBehaviour;
using _MonoBehaviour = YS.Variable<UnityEngine.MonoBehaviour>;
using _bool = YS.Variable<bool>;
using _string = YS.Variable<string>;
using _float = YS.Variable<float>;
using Coroutine = UnityEngine.Coroutine;
using _Coroutine = YS.Variable<UnityEngine.Coroutine>;
using _object = YS.Variable<object>;
using IEnumerator = System.Collections.IEnumerator;
using _IEnumerator = YS.Variable<System.Collections.IEnumerator>;

using static System.Runtime.CompilerServices.Unsafe;
using __V = YS.Variable;
namespace YS.Generated {
    public static partial  class Wrapper {
        static void UnityEngine_MonoBehaviour_ctor(__V res) =>  As<_MonoBehaviour>(res).value = new MonoBehaviour();
       static void UnityEngine_MonoBehaviour_get_useGUILayout(__V res, __V  i1) => As<_bool>(res).value=As<_MonoBehaviour>(i1).value.useGUILayout;
       static void UnityEngine_MonoBehaviour_set_useGUILayout(__V  i1, __V  i2) => As<_MonoBehaviour>(i1).value.useGUILayout = As<_bool>(i2).value;
       static void UnityEngine_MonoBehaviour_IsInvoking(__V res, __V  i1) => As<_bool>(res).value=As<_MonoBehaviour>(i1).value.IsInvoking();
       static void UnityEngine_MonoBehaviour_IsInvoking_(__V res,__V  i1, __V  i2) => As<_bool>(res).value=As<_MonoBehaviour>(i1).value.IsInvoking(As<_string>(i2).value);
       static void UnityEngine_MonoBehaviour_CancelInvoke(__V  i1) => As<_MonoBehaviour>(i1).value.CancelInvoke();
       static void UnityEngine_MonoBehaviour_CancelInvoke_(__V  i1, __V  i2) => As<_MonoBehaviour>(i1).value.CancelInvoke(As<_string>(i2).value);
       static void UnityEngine_MonoBehaviour_Invoke(__V  i1, __V  i2, __V  i3)  =>  As<_MonoBehaviour>(i1).value.Invoke(As<_string>(i2).value,As<_float>(i3).value);
       static void UnityEngine_MonoBehaviour_InvokeRepeating(__V  i1, __V  i2, __V  i3, __V  i4)  => As<_MonoBehaviour>(i1).value.InvokeRepeating(As<_string>(i2).value,As<_float>(i3).value,As<_float>(i4).value);
       static void UnityEngine_MonoBehaviour_StartCoroutine(__V res,__V  i1, __V  i2) => As<_Coroutine>(res).value=As<_MonoBehaviour>(i1).value.StartCoroutine(As<_string>(i2).value);
       static void UnityEngine_MonoBehaviour_StartCoroutine_(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_Coroutine>(res).value=As<_MonoBehaviour>(i1).value.StartCoroutine(As<_string>(i2).value,As<_object>(i3).value);
       static void UnityEngine_MonoBehaviour_StartCoroutine__(__V res,__V  i1, __V  i2) => As<_Coroutine>(res).value=As<_MonoBehaviour>(i1).value.StartCoroutine(As<_IEnumerator>(i2).value);
       static void UnityEngine_MonoBehaviour_StopCoroutine(__V  i1, __V  i2) => As<_MonoBehaviour>(i1).value.StopCoroutine(As<_IEnumerator>(i2).value);
       static void UnityEngine_MonoBehaviour_StopCoroutine_(__V  i1, __V  i2) => As<_MonoBehaviour>(i1).value.StopCoroutine(As<_Coroutine>(i2).value);
       static void UnityEngine_MonoBehaviour_StopCoroutine__(__V  i1, __V  i2) => As<_MonoBehaviour>(i1).value.StopCoroutine(As<_string>(i2).value);
       static void UnityEngine_MonoBehaviour_StopAllCoroutines(__V  i1) => As<_MonoBehaviour>(i1).value.StopAllCoroutines();
       static void UnityEngine_MonoBehaviour_print(__V  i1) => MonoBehaviour.print(As<_object>(i1).value);
#if !AOT

        static TypeModule Create_UnityEngine_MonoBehaviourModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(MonoBehaviour));
            module.RegisterConstructor(UnityEngine_MonoBehaviour_ctor);
            module.RegisterPropertyGetter("useGUILayout", UnityEngine_MonoBehaviour_get_useGUILayout);
            module.RegisterPropertySetter("useGUILayout", UnityEngine_MonoBehaviour_set_useGUILayout);
            module.RegisterMethod2("IsInvoking", Types(),UnityEngine_MonoBehaviour_IsInvoking);
            module.RegisterMethod3("IsInvoking", Types(typeof(string)),UnityEngine_MonoBehaviour_IsInvoking_);
            module.RegisterMethod1("CancelInvoke", Types(),UnityEngine_MonoBehaviour_CancelInvoke);
            module.RegisterMethod2("CancelInvoke", Types(typeof(string)),UnityEngine_MonoBehaviour_CancelInvoke_);
            module.RegisterMethod3("Invoke",UnityEngine_MonoBehaviour_Invoke);
            module.RegisterMethod4("InvokeRepeating",UnityEngine_MonoBehaviour_InvokeRepeating);
            module.RegisterMethod3("StartCoroutine", Types(typeof(string)),UnityEngine_MonoBehaviour_StartCoroutine);
            module.RegisterMethod4("StartCoroutine", Types(typeof(string),typeof(object)),UnityEngine_MonoBehaviour_StartCoroutine_);
            module.RegisterMethod3("StartCoroutine", Types(typeof(IEnumerator)),UnityEngine_MonoBehaviour_StartCoroutine__);
            module.RegisterMethod2("StopCoroutine", Types(typeof(IEnumerator)),UnityEngine_MonoBehaviour_StopCoroutine);
            module.RegisterMethod2("StopCoroutine", Types(typeof(Coroutine)),UnityEngine_MonoBehaviour_StopCoroutine_);
            module.RegisterMethod2("StopCoroutine", Types(typeof(string)),UnityEngine_MonoBehaviour_StopCoroutine__);
            module.RegisterMethod1("StopAllCoroutines",UnityEngine_MonoBehaviour_StopAllCoroutines);
            module.RegisterMethod1("print",UnityEngine_MonoBehaviour_print);
#else
        static unsafe TypeModule Create_UnityEngine_MonoBehaviourModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(MonoBehaviour));
            module.RegisterConstructor(&UnityEngine_MonoBehaviour_ctor);
            module.RegisterPropertyGetter("useGUILayout", &UnityEngine_MonoBehaviour_get_useGUILayout);
            module.RegisterPropertySetter("useGUILayout",& UnityEngine_MonoBehaviour_set_useGUILayout);
            module.RegisterMethod2("IsInvoking", Types(),&UnityEngine_MonoBehaviour_IsInvoking);
            module.RegisterMethod3("IsInvoking", Types(typeof(string)),&UnityEngine_MonoBehaviour_IsInvoking_);
            module.RegisterMethod1("CancelInvoke", Types(),&UnityEngine_MonoBehaviour_CancelInvoke);
            module.RegisterMethod2("CancelInvoke", Types(typeof(string)),&UnityEngine_MonoBehaviour_CancelInvoke_);
            module.RegisterMethod3("Invoke",&UnityEngine_MonoBehaviour_Invoke);
            module.RegisterMethod4("InvokeRepeating",&UnityEngine_MonoBehaviour_InvokeRepeating);
            module.RegisterMethod3("StartCoroutine", Types(typeof(string)),&UnityEngine_MonoBehaviour_StartCoroutine);
            module.RegisterMethod4("StartCoroutine", Types(typeof(string),typeof(object)),&UnityEngine_MonoBehaviour_StartCoroutine_);
            module.RegisterMethod3("StartCoroutine", Types(typeof(IEnumerator)),&UnityEngine_MonoBehaviour_StartCoroutine__);
            module.RegisterMethod2("StopCoroutine", Types(typeof(IEnumerator)),&UnityEngine_MonoBehaviour_StopCoroutine);
            module.RegisterMethod2("StopCoroutine", Types(typeof(Coroutine)),&UnityEngine_MonoBehaviour_StopCoroutine_);
            module.RegisterMethod2("StopCoroutine", Types(typeof(string)),&UnityEngine_MonoBehaviour_StopCoroutine__);
            module.RegisterMethod1("StopAllCoroutines",&UnityEngine_MonoBehaviour_StopAllCoroutines);
            module.RegisterMethod1("print",&UnityEngine_MonoBehaviour_print);
#endif
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}