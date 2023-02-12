using GameObject = UnityEngine.GameObject;
using Type = System.Type;
using PrimitiveType = UnityEngine.PrimitiveType;
using List = System.Collections.Generic.List<UnityEngine.Component>;
using Component = UnityEngine.Component;
using SendMessageOptions = UnityEngine.SendMessageOptions;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_GameObjectModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(GameObject));
            module.RegisterConstructor(typeof(string),(result, input1) => result.SetValue( new GameObject(input1.As<string>())));
            module.RegisterConstructor(result => result.SetValue( new GameObject()));
            module.RegisterConstructor(typeof(string),typeof(Type[]),(result, input1, input2) => result.SetValue( new GameObject(input1.As<string>(),input2.As<Type[]>())));
            module.RegisterPropertyGetter("transform", (result, input1) => result.SetValue(input1.As<GameObject>().transform));
            module.RegisterPropertyGetter("layer", (result, input1) => result.SetValue(input1.As<GameObject>().layer));
            module.RegisterPropertySetter("layer", (input1, input2) => input1.As<GameObject>().layer=input2.As<int>());
            module.RegisterPropertyGetter("activeSelf", (result, input1) => result.SetValue(input1.As<GameObject>().activeSelf));
            module.RegisterPropertyGetter("activeInHierarchy", (result, input1) => result.SetValue(input1.As<GameObject>().activeInHierarchy));
            module.RegisterPropertyGetter("isStatic", (result, input1) => result.SetValue(input1.As<GameObject>().isStatic));
            module.RegisterPropertySetter("isStatic", (input1, input2) => input1.As<GameObject>().isStatic=input2.As<bool>());
            module.RegisterPropertyGetter("tag", (result, input1) => result.SetValue(input1.As<GameObject>().tag));
            module.RegisterPropertySetter("tag", (input1, input2) => input1.As<GameObject>().tag=input2.As<string>());
            module.RegisterPropertyGetter("scene", (result, input1) => result.SetValue(input1.As<GameObject>().scene));
            module.RegisterPropertyGetter("sceneCullingMask", (result, input1) => result.SetValue(input1.As<GameObject>().sceneCullingMask));
            module.RegisterPropertyGetter("gameObject", (result, input1) => result.SetValue(input1.As<GameObject>().gameObject));
            module.RegisterMethod("CreatePrimitive", (result, input1) => result.SetValue(GameObject.CreatePrimitive(input1.As<PrimitiveType>())));
            module.RegisterMethod("GetComponent", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<GameObject>().GetComponent(input2.As<Type>())));
            module.RegisterMethod("GetComponent", Types(typeof(string)),(result, input1, input2) => result.SetValue(input1.As<GameObject>().GetComponent(input2.As<string>())));
            module.RegisterMethod("GetComponentInChildren", Types(typeof(Type),typeof(bool)),(result, input1, input2, input3) => result.SetValue(input1.As<GameObject>().GetComponentInChildren(input2.As<Type>(),input3.As<bool>())));
            module.RegisterMethod("GetComponentInChildren", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<GameObject>().GetComponentInChildren(input2.As<Type>())));
            module.RegisterMethod("GetComponentInParent", Types(typeof(Type),typeof(bool)),(result, input1, input2, input3) => result.SetValue(input1.As<GameObject>().GetComponentInParent(input2.As<Type>(),input3.As<bool>())));
            module.RegisterMethod("GetComponentInParent", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<GameObject>().GetComponentInParent(input2.As<Type>())));
            module.RegisterMethod("GetComponents", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<GameObject>().GetComponents(input2.As<Type>())));
            module.RegisterMethod("GetComponents", Types(typeof(Type),typeof(List)),(input1, input2, input3)  =>  input1.As<GameObject>().GetComponents(input2.As<Type>(),input3.As<List>()));
            module.RegisterMethod("GetComponentsInChildren", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<GameObject>().GetComponentsInChildren(input2.As<Type>())));
            module.RegisterMethod("GetComponentsInChildren", Types(typeof(Type),typeof(bool)),(result, input1, input2, input3) => result.SetValue(input1.As<GameObject>().GetComponentsInChildren(input2.As<Type>(),input3.As<bool>())));
            module.RegisterMethod("GetComponentsInParent", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<GameObject>().GetComponentsInParent(input2.As<Type>())));
            module.RegisterMethod("GetComponentsInParent", Types(typeof(Type),typeof(bool)),(result, input1, input2, input3) => result.SetValue(input1.As<GameObject>().GetComponentsInParent(input2.As<Type>(),input3.As<bool>())));
            module.RegisterMethod("TryGetComponent", (result, input1, input2, input3) => result.SetValue(input1.As<GameObject>().TryGetComponent(input2.As<Type>(),out input3.As<Component>())));
            module.RegisterMethod("FindWithTag", (result, input1) => result.SetValue(GameObject.FindWithTag(input1.As<string>())));
            module.RegisterMethod("SendMessageUpwards", Types(typeof(string),typeof(SendMessageOptions)),(input1, input2, input3)  =>  input1.As<GameObject>().SendMessageUpwards(input2.As<string>(),input3.As<SendMessageOptions>()));
            module.RegisterMethod("SendMessageUpwards", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),(input1, input2, input3, input4)  => input1.As<GameObject>().SendMessageUpwards(input2.As<string>(),input3.As<object>(),input4.As<SendMessageOptions>()));
            module.RegisterMethod("SendMessageUpwards", Types(typeof(string),typeof(object)),(input1, input2, input3)  =>  input1.As<GameObject>().SendMessageUpwards(input2.As<string>(),input3.As<object>()));
            module.RegisterMethod("SendMessageUpwards", Types(typeof(string)),(input1, input2) => input1.As<GameObject>().SendMessageUpwards(input2.As<string>()));
            module.RegisterMethod("SendMessage", Types(typeof(string),typeof(SendMessageOptions)),(input1, input2, input3)  =>  input1.As<GameObject>().SendMessage(input2.As<string>(),input3.As<SendMessageOptions>()));
            module.RegisterMethod("SendMessage", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),(input1, input2, input3, input4)  => input1.As<GameObject>().SendMessage(input2.As<string>(),input3.As<object>(),input4.As<SendMessageOptions>()));
            module.RegisterMethod("SendMessage", Types(typeof(string),typeof(object)),(input1, input2, input3)  =>  input1.As<GameObject>().SendMessage(input2.As<string>(),input3.As<object>()));
            module.RegisterMethod("SendMessage", Types(typeof(string)),(input1, input2) => input1.As<GameObject>().SendMessage(input2.As<string>()));
            module.RegisterMethod("BroadcastMessage", Types(typeof(string),typeof(SendMessageOptions)),(input1, input2, input3)  =>  input1.As<GameObject>().BroadcastMessage(input2.As<string>(),input3.As<SendMessageOptions>()));
            module.RegisterMethod("BroadcastMessage", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),(input1, input2, input3, input4)  => input1.As<GameObject>().BroadcastMessage(input2.As<string>(),input3.As<object>(),input4.As<SendMessageOptions>()));
            module.RegisterMethod("BroadcastMessage", Types(typeof(string),typeof(object)),(input1, input2, input3)  =>  input1.As<GameObject>().BroadcastMessage(input2.As<string>(),input3.As<object>()));
            module.RegisterMethod("BroadcastMessage", Types(typeof(string)),(input1, input2) => input1.As<GameObject>().BroadcastMessage(input2.As<string>()));
            module.RegisterMethod("AddComponent", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<GameObject>().AddComponent(input2.As<Type>())));
            module.RegisterMethod("SetActive", (input1, input2) => input1.As<GameObject>().SetActive(input2.As<bool>()));
            module.RegisterMethod("CompareTag", (result, input1, input2) => result.SetValue(input1.As<GameObject>().CompareTag(input2.As<string>())));
            module.RegisterMethod("FindGameObjectWithTag", (result, input1) => result.SetValue(GameObject.FindGameObjectWithTag(input1.As<string>())));
            module.RegisterMethod("FindGameObjectsWithTag", (result, input1) => result.SetValue(GameObject.FindGameObjectsWithTag(input1.As<string>())));
            module.RegisterMethod("Find", (result, input1) => result.SetValue(GameObject.Find(input1.As<string>())));
            module.ClearTempMembers();
                          if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}