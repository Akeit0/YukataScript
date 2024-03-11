#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif
using Component = UnityEngine.Component;
using _Component = YS.Variable<UnityEngine.Component>;
using Transform = UnityEngine.Transform;
using _Transform = YS.Variable<UnityEngine.Transform>;
using GameObject = UnityEngine.GameObject;
using _GameObject = YS.Variable<UnityEngine.GameObject>;
using _string = YS.Variable<string>;
using Type = System.Type;
using _Type = YS.Variable<System.Type>;
using _bool = YS.Variable<bool>;
using List = System.Collections.Generic.List<UnityEngine.Component>;
using _List = YS.Variable<System.Collections.Generic.List<UnityEngine.Component>>;
using _object = YS.Variable<object>;
using SendMessageOptions = UnityEngine.SendMessageOptions;
using _SendMessageOptions = YS.Variable<UnityEngine.SendMessageOptions>;

using static System.Runtime.CompilerServices.Unsafe;
using __V = YS.Variable;
namespace YS.Generated {
    public static partial  class Wrapper {
        static void UnityEngine_Component_ctor(__V res) =>  As<_Component>(res).value = new Component();
       static void UnityEngine_Component_get_transform(__V res, __V  i1) => As<_Transform>(res).value=As<_Component>(i1).value.transform;
       static void UnityEngine_Component_get_gameObject(__V res, __V  i1) => As<_GameObject>(res).value=As<_Component>(i1).value.gameObject;
       static void UnityEngine_Component_get_tag(__V res, __V  i1) => As<_string>(res).value=As<_Component>(i1).value.tag;
       static void UnityEngine_Component_set_tag(__V  i1, __V  i2) => As<_Component>(i1).value.tag = As<_string>(i2).value;
       static void UnityEngine_Component_GetComponent(__V res,__V  i1, __V  i2) => As<_Component>(res).value=As<_Component>(i1).value.GetComponent(As<_Type>(i2).value);
       static void UnityEngine_Component_GetComponent_(__V res,__V  i1, __V  i2) => As<_Component>(res).value=As<_Component>(i1).value.GetComponent(As<_string>(i2).value);
       static void UnityEngine_Component_TryGetComponent(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_bool>(res).value=As<_Component>(i1).value.TryGetComponent(As<_Type>(i2).value,out As<_Component>(i3).value);
       static void UnityEngine_Component_GetComponentInChildren(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_Component>(res).value=As<_Component>(i1).value.GetComponentInChildren(As<_Type>(i2).value,As<_bool>(i3).value);
       static void UnityEngine_Component_GetComponentInChildren_(__V res,__V  i1, __V  i2) => As<_Component>(res).value=As<_Component>(i1).value.GetComponentInChildren(As<_Type>(i2).value);
       static void UnityEngine_Component_GetComponentsInChildren(__V res,__V  i1, __V  i2, __V  i3)  =>  As<Variable<Component[]>>(res).value=As<_Component>(i1).value.GetComponentsInChildren(As<_Type>(i2).value,As<_bool>(i3).value);
       static void UnityEngine_Component_GetComponentsInChildren_(__V res,__V  i1, __V  i2) => As<Variable<Component[]>>(res).value=As<_Component>(i1).value.GetComponentsInChildren(As<_Type>(i2).value);
       static void UnityEngine_Component_GetComponentInParent(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_Component>(res).value=As<_Component>(i1).value.GetComponentInParent(As<_Type>(i2).value,As<_bool>(i3).value);
       static void UnityEngine_Component_GetComponentInParent_(__V res,__V  i1, __V  i2) => As<_Component>(res).value=As<_Component>(i1).value.GetComponentInParent(As<_Type>(i2).value);
       static void UnityEngine_Component_GetComponentsInParent(__V res,__V  i1, __V  i2, __V  i3)  =>  As<Variable<Component[]>>(res).value=As<_Component>(i1).value.GetComponentsInParent(As<_Type>(i2).value,As<_bool>(i3).value);
       static void UnityEngine_Component_GetComponentsInParent_(__V res,__V  i1, __V  i2) => As<Variable<Component[]>>(res).value=As<_Component>(i1).value.GetComponentsInParent(As<_Type>(i2).value);
       static void UnityEngine_Component_GetComponents(__V res,__V  i1, __V  i2) => As<Variable<Component[]>>(res).value=As<_Component>(i1).value.GetComponents(As<_Type>(i2).value);
       static void UnityEngine_Component_GetComponents_(__V  i1, __V  i2, __V  i3)  =>  As<_Component>(i1).value.GetComponents(As<_Type>(i2).value,As<_List>(i3).value);
       static void UnityEngine_Component_CompareTag(__V res,__V  i1, __V  i2) => As<_bool>(res).value=As<_Component>(i1).value.CompareTag(As<_string>(i2).value);
       static void UnityEngine_Component_SendMessageUpwards(__V  i1, __V  i2, __V  i3, __V  i4)  => As<_Component>(i1).value.SendMessageUpwards(As<_string>(i2).value,As<_object>(i3).value,As<_SendMessageOptions>(i4).value);
       static void UnityEngine_Component_SendMessageUpwards_(__V  i1, __V  i2, __V  i3)  =>  As<_Component>(i1).value.SendMessageUpwards(As<_string>(i2).value,As<_object>(i3).value);
       static void UnityEngine_Component_SendMessageUpwards__(__V  i1, __V  i2) => As<_Component>(i1).value.SendMessageUpwards(As<_string>(i2).value);
       static void UnityEngine_Component_SendMessageUpwards___(__V  i1, __V  i2, __V  i3)  =>  As<_Component>(i1).value.SendMessageUpwards(As<_string>(i2).value,As<_SendMessageOptions>(i3).value);
       static void UnityEngine_Component_SendMessage(__V  i1, __V  i2, __V  i3)  =>  As<_Component>(i1).value.SendMessage(As<_string>(i2).value,As<_object>(i3).value);
       static void UnityEngine_Component_SendMessage_(__V  i1, __V  i2) => As<_Component>(i1).value.SendMessage(As<_string>(i2).value);
       static void UnityEngine_Component_SendMessage__(__V  i1, __V  i2, __V  i3, __V  i4)  => As<_Component>(i1).value.SendMessage(As<_string>(i2).value,As<_object>(i3).value,As<_SendMessageOptions>(i4).value);
       static void UnityEngine_Component_SendMessage___(__V  i1, __V  i2, __V  i3)  =>  As<_Component>(i1).value.SendMessage(As<_string>(i2).value,As<_SendMessageOptions>(i3).value);
       static void UnityEngine_Component_BroadcastMessage(__V  i1, __V  i2, __V  i3, __V  i4)  => As<_Component>(i1).value.BroadcastMessage(As<_string>(i2).value,As<_object>(i3).value,As<_SendMessageOptions>(i4).value);
       static void UnityEngine_Component_BroadcastMessage_(__V  i1, __V  i2, __V  i3)  =>  As<_Component>(i1).value.BroadcastMessage(As<_string>(i2).value,As<_object>(i3).value);
       static void UnityEngine_Component_BroadcastMessage__(__V  i1, __V  i2) => As<_Component>(i1).value.BroadcastMessage(As<_string>(i2).value);
       static void UnityEngine_Component_BroadcastMessage___(__V  i1, __V  i2, __V  i3)  =>  As<_Component>(i1).value.BroadcastMessage(As<_string>(i2).value,As<_SendMessageOptions>(i3).value);
#if !AOT

        static TypeModule Create_UnityEngine_ComponentModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Component));
            module.RegisterConstructor(UnityEngine_Component_ctor);
            module.RegisterPropertyGetter("transform", UnityEngine_Component_get_transform);
            module.RegisterPropertyGetter("gameObject", UnityEngine_Component_get_gameObject);
            module.RegisterPropertyGetter("tag", UnityEngine_Component_get_tag);
            module.RegisterPropertySetter("tag", UnityEngine_Component_set_tag);
            module.RegisterMethod3("GetComponent", Types(typeof(Type)),UnityEngine_Component_GetComponent);
            module.RegisterMethod3("GetComponent", Types(typeof(string)),UnityEngine_Component_GetComponent_);
            module.RegisterMethod4("TryGetComponent",UnityEngine_Component_TryGetComponent);
            module.RegisterMethod4("GetComponentInChildren", Types(typeof(Type),typeof(bool)),UnityEngine_Component_GetComponentInChildren);
            module.RegisterMethod3("GetComponentInChildren", Types(typeof(Type)),UnityEngine_Component_GetComponentInChildren_);
            module.RegisterMethod4("GetComponentsInChildren", Types(typeof(Type),typeof(bool)),UnityEngine_Component_GetComponentsInChildren);
            module.RegisterMethod3("GetComponentsInChildren", Types(typeof(Type)),UnityEngine_Component_GetComponentsInChildren_);
            module.RegisterMethod4("GetComponentInParent", Types(typeof(Type),typeof(bool)),UnityEngine_Component_GetComponentInParent);
            module.RegisterMethod3("GetComponentInParent", Types(typeof(Type)),UnityEngine_Component_GetComponentInParent_);
            module.RegisterMethod4("GetComponentsInParent", Types(typeof(Type),typeof(bool)),UnityEngine_Component_GetComponentsInParent);
            module.RegisterMethod3("GetComponentsInParent", Types(typeof(Type)),UnityEngine_Component_GetComponentsInParent_);
            module.RegisterMethod3("GetComponents", Types(typeof(Type)),UnityEngine_Component_GetComponents);
            module.RegisterMethod3("GetComponents", Types(typeof(Type),typeof(List)),UnityEngine_Component_GetComponents_);
            module.RegisterMethod3("CompareTag",UnityEngine_Component_CompareTag);
            module.RegisterMethod4("SendMessageUpwards", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),UnityEngine_Component_SendMessageUpwards);
            module.RegisterMethod3("SendMessageUpwards", Types(typeof(string),typeof(object)),UnityEngine_Component_SendMessageUpwards_);
            module.RegisterMethod2("SendMessageUpwards", Types(typeof(string)),UnityEngine_Component_SendMessageUpwards__);
            module.RegisterMethod3("SendMessageUpwards", Types(typeof(string),typeof(SendMessageOptions)),UnityEngine_Component_SendMessageUpwards___);
            module.RegisterMethod3("SendMessage", Types(typeof(string),typeof(object)),UnityEngine_Component_SendMessage);
            module.RegisterMethod2("SendMessage", Types(typeof(string)),UnityEngine_Component_SendMessage_);
            module.RegisterMethod4("SendMessage", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),UnityEngine_Component_SendMessage__);
            module.RegisterMethod3("SendMessage", Types(typeof(string),typeof(SendMessageOptions)),UnityEngine_Component_SendMessage___);
            module.RegisterMethod4("BroadcastMessage", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),UnityEngine_Component_BroadcastMessage);
            module.RegisterMethod3("BroadcastMessage", Types(typeof(string),typeof(object)),UnityEngine_Component_BroadcastMessage_);
            module.RegisterMethod2("BroadcastMessage", Types(typeof(string)),UnityEngine_Component_BroadcastMessage__);
            module.RegisterMethod3("BroadcastMessage", Types(typeof(string),typeof(SendMessageOptions)),UnityEngine_Component_BroadcastMessage___);
#else
        static unsafe TypeModule Create_UnityEngine_ComponentModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Component));
            module.RegisterConstructor(&UnityEngine_Component_ctor);
            module.RegisterPropertyGetter("transform", &UnityEngine_Component_get_transform);
            module.RegisterPropertyGetter("gameObject", &UnityEngine_Component_get_gameObject);
            module.RegisterPropertyGetter("tag", &UnityEngine_Component_get_tag);
            module.RegisterPropertySetter("tag",& UnityEngine_Component_set_tag);
            module.RegisterMethod3("GetComponent", Types(typeof(Type)),&UnityEngine_Component_GetComponent);
            module.RegisterMethod3("GetComponent", Types(typeof(string)),&UnityEngine_Component_GetComponent_);
            module.RegisterMethod4("TryGetComponent",&UnityEngine_Component_TryGetComponent);
            module.RegisterMethod4("GetComponentInChildren", Types(typeof(Type),typeof(bool)),&UnityEngine_Component_GetComponentInChildren);
            module.RegisterMethod3("GetComponentInChildren", Types(typeof(Type)),&UnityEngine_Component_GetComponentInChildren_);
            module.RegisterMethod4("GetComponentsInChildren", Types(typeof(Type),typeof(bool)),&UnityEngine_Component_GetComponentsInChildren);
            module.RegisterMethod3("GetComponentsInChildren", Types(typeof(Type)),&UnityEngine_Component_GetComponentsInChildren_);
            module.RegisterMethod4("GetComponentInParent", Types(typeof(Type),typeof(bool)),&UnityEngine_Component_GetComponentInParent);
            module.RegisterMethod3("GetComponentInParent", Types(typeof(Type)),&UnityEngine_Component_GetComponentInParent_);
            module.RegisterMethod4("GetComponentsInParent", Types(typeof(Type),typeof(bool)),&UnityEngine_Component_GetComponentsInParent);
            module.RegisterMethod3("GetComponentsInParent", Types(typeof(Type)),&UnityEngine_Component_GetComponentsInParent_);
            module.RegisterMethod3("GetComponents", Types(typeof(Type)),&UnityEngine_Component_GetComponents);
            module.RegisterMethod3("GetComponents", Types(typeof(Type),typeof(List)),&UnityEngine_Component_GetComponents_);
            module.RegisterMethod3("CompareTag",&UnityEngine_Component_CompareTag);
            module.RegisterMethod4("SendMessageUpwards", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),&UnityEngine_Component_SendMessageUpwards);
            module.RegisterMethod3("SendMessageUpwards", Types(typeof(string),typeof(object)),&UnityEngine_Component_SendMessageUpwards_);
            module.RegisterMethod2("SendMessageUpwards", Types(typeof(string)),&UnityEngine_Component_SendMessageUpwards__);
            module.RegisterMethod3("SendMessageUpwards", Types(typeof(string),typeof(SendMessageOptions)),&UnityEngine_Component_SendMessageUpwards___);
            module.RegisterMethod3("SendMessage", Types(typeof(string),typeof(object)),&UnityEngine_Component_SendMessage);
            module.RegisterMethod2("SendMessage", Types(typeof(string)),&UnityEngine_Component_SendMessage_);
            module.RegisterMethod4("SendMessage", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),&UnityEngine_Component_SendMessage__);
            module.RegisterMethod3("SendMessage", Types(typeof(string),typeof(SendMessageOptions)),&UnityEngine_Component_SendMessage___);
            module.RegisterMethod4("BroadcastMessage", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),&UnityEngine_Component_BroadcastMessage);
            module.RegisterMethod3("BroadcastMessage", Types(typeof(string),typeof(object)),&UnityEngine_Component_BroadcastMessage_);
            module.RegisterMethod2("BroadcastMessage", Types(typeof(string)),&UnityEngine_Component_BroadcastMessage__);
            module.RegisterMethod3("BroadcastMessage", Types(typeof(string),typeof(SendMessageOptions)),&UnityEngine_Component_BroadcastMessage___);
#endif
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}