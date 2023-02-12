using MonoBehaviour = UnityEngine.MonoBehaviour;
using IEnumerator = System.Collections.IEnumerator;
using Coroutine = UnityEngine.Coroutine;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_MonoBehaviourModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(MonoBehaviour));
            module.RegisterConstructor(result => result.SetValue( new MonoBehaviour()));
            module.RegisterPropertyGetter("useGUILayout", (result, input1) => result.SetValue(input1.As<MonoBehaviour>().useGUILayout));
            module.RegisterPropertySetter("useGUILayout", (input1, input2) => input1.As<MonoBehaviour>().useGUILayout=input2.As<bool>());
            module.RegisterMethod("IsInvoking", Types(),(result, input1) => result.SetValue(input1.As<MonoBehaviour>().IsInvoking()));
            module.RegisterMethod("IsInvoking", Types(typeof(string)),(result, input1, input2) => result.SetValue(input1.As<MonoBehaviour>().IsInvoking(input2.As<string>())));
            module.RegisterMethod("CancelInvoke", Types(),input1 => input1.As<MonoBehaviour>().CancelInvoke());
            module.RegisterMethod("CancelInvoke", Types(typeof(string)),(input1, input2) => input1.As<MonoBehaviour>().CancelInvoke(input2.As<string>()));
            module.RegisterMethod("Invoke", (input1, input2, input3)  =>  input1.As<MonoBehaviour>().Invoke(input2.As<string>(),input3.As<float>()));
            module.RegisterMethod("InvokeRepeating", (input1, input2, input3, input4)  => input1.As<MonoBehaviour>().InvokeRepeating(input2.As<string>(),input3.As<float>(),input4.As<float>()));
            module.RegisterMethod("StartCoroutine", Types(typeof(string)),(result, input1, input2) => result.SetValue(input1.As<MonoBehaviour>().StartCoroutine(input2.As<string>())));
            module.RegisterMethod("StartCoroutine", Types(typeof(string),typeof(object)),(result, input1, input2, input3) => result.SetValue(input1.As<MonoBehaviour>().StartCoroutine(input2.As<string>(),input3.As<object>())));
            module.RegisterMethod("StartCoroutine", Types(typeof(IEnumerator)),(result, input1, input2) => result.SetValue(input1.As<MonoBehaviour>().StartCoroutine(input2.As<IEnumerator>())));
            module.RegisterMethod("StopCoroutine", Types(typeof(IEnumerator)),(input1, input2) => input1.As<MonoBehaviour>().StopCoroutine(input2.As<IEnumerator>()));
            module.RegisterMethod("StopCoroutine", Types(typeof(Coroutine)),(input1, input2) => input1.As<MonoBehaviour>().StopCoroutine(input2.As<Coroutine>()));
            module.RegisterMethod("StopCoroutine", Types(typeof(string)),(input1, input2) => input1.As<MonoBehaviour>().StopCoroutine(input2.As<string>()));
            module.RegisterMethod("StopAllCoroutines", input1 => input1.As<MonoBehaviour>().StopAllCoroutines());
            module.RegisterMethod("print", input1 => MonoBehaviour.print(input1.As<object>()));
            module.ClearTempMembers();
                          if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}