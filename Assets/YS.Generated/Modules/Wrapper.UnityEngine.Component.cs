using Component = UnityEngine.Component;
using Type = System.Type;
using List = System.Collections.Generic.List<UnityEngine.Component>;
using SendMessageOptions = UnityEngine.SendMessageOptions;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_ComponentModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Component));
            module.RegisterConstructor(result => result.SetValue( new Component()));
            module.RegisterPropertyGetter("transform", (result, input1) => result.SetValue(input1.As<Component>().transform));
            module.RegisterPropertyGetter("gameObject", (result, input1) => result.SetValue(input1.As<Component>().gameObject));
            module.RegisterPropertyGetter("tag", (result, input1) => result.SetValue(input1.As<Component>().tag));
            module.RegisterPropertySetter("tag", (input1, input2) => input1.As<Component>().tag=input2.As<string>());
            module.RegisterMethod("GetComponent", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<Component>().GetComponent(input2.As<Type>())));
            module.RegisterMethod("GetComponent", Types(typeof(string)),(result, input1, input2) => result.SetValue(input1.As<Component>().GetComponent(input2.As<string>())));
            module.RegisterMethod("TryGetComponent", (result, input1, input2, input3) => result.SetValue(input1.As<Component>().TryGetComponent(input2.As<Type>(),out input3.As<Component>())));
            module.RegisterMethod("GetComponentInChildren", Types(typeof(Type),typeof(bool)),(result, input1, input2, input3) => result.SetValue(input1.As<Component>().GetComponentInChildren(input2.As<Type>(),input3.As<bool>())));
            module.RegisterMethod("GetComponentInChildren", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<Component>().GetComponentInChildren(input2.As<Type>())));
            module.RegisterMethod("GetComponentsInChildren", Types(typeof(Type),typeof(bool)),(result, input1, input2, input3) => result.SetValue(input1.As<Component>().GetComponentsInChildren(input2.As<Type>(),input3.As<bool>())));
            module.RegisterMethod("GetComponentsInChildren", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<Component>().GetComponentsInChildren(input2.As<Type>())));
            module.RegisterMethod("GetComponentInParent", Types(typeof(Type),typeof(bool)),(result, input1, input2, input3) => result.SetValue(input1.As<Component>().GetComponentInParent(input2.As<Type>(),input3.As<bool>())));
            module.RegisterMethod("GetComponentInParent", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<Component>().GetComponentInParent(input2.As<Type>())));
            module.RegisterMethod("GetComponentsInParent", Types(typeof(Type),typeof(bool)),(result, input1, input2, input3) => result.SetValue(input1.As<Component>().GetComponentsInParent(input2.As<Type>(),input3.As<bool>())));
            module.RegisterMethod("GetComponentsInParent", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<Component>().GetComponentsInParent(input2.As<Type>())));
            module.RegisterMethod("GetComponents", Types(typeof(Type)),(result, input1, input2) => result.SetValue(input1.As<Component>().GetComponents(input2.As<Type>())));
            module.RegisterMethod("GetComponents", Types(typeof(Type),typeof(List)),(input1, input2, input3)  =>  input1.As<Component>().GetComponents(input2.As<Type>(),input3.As<List>()));
            module.RegisterMethod("CompareTag", (result, input1, input2) => result.SetValue(input1.As<Component>().CompareTag(input2.As<string>())));
            module.RegisterMethod("SendMessageUpwards", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),(input1, input2, input3, input4)  => input1.As<Component>().SendMessageUpwards(input2.As<string>(),input3.As<object>(),input4.As<SendMessageOptions>()));
            module.RegisterMethod("SendMessageUpwards", Types(typeof(string),typeof(object)),(input1, input2, input3)  =>  input1.As<Component>().SendMessageUpwards(input2.As<string>(),input3.As<object>()));
            module.RegisterMethod("SendMessageUpwards", Types(typeof(string)),(input1, input2) => input1.As<Component>().SendMessageUpwards(input2.As<string>()));
            module.RegisterMethod("SendMessageUpwards", Types(typeof(string),typeof(SendMessageOptions)),(input1, input2, input3)  =>  input1.As<Component>().SendMessageUpwards(input2.As<string>(),input3.As<SendMessageOptions>()));
            module.RegisterMethod("SendMessage", Types(typeof(string),typeof(object)),(input1, input2, input3)  =>  input1.As<Component>().SendMessage(input2.As<string>(),input3.As<object>()));
            module.RegisterMethod("SendMessage", Types(typeof(string)),(input1, input2) => input1.As<Component>().SendMessage(input2.As<string>()));
            module.RegisterMethod("SendMessage", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),(input1, input2, input3, input4)  => input1.As<Component>().SendMessage(input2.As<string>(),input3.As<object>(),input4.As<SendMessageOptions>()));
            module.RegisterMethod("SendMessage", Types(typeof(string),typeof(SendMessageOptions)),(input1, input2, input3)  =>  input1.As<Component>().SendMessage(input2.As<string>(),input3.As<SendMessageOptions>()));
            module.RegisterMethod("BroadcastMessage", Types(typeof(string),typeof(object),typeof(SendMessageOptions)),(input1, input2, input3, input4)  => input1.As<Component>().BroadcastMessage(input2.As<string>(),input3.As<object>(),input4.As<SendMessageOptions>()));
            module.RegisterMethod("BroadcastMessage", Types(typeof(string),typeof(object)),(input1, input2, input3)  =>  input1.As<Component>().BroadcastMessage(input2.As<string>(),input3.As<object>()));
            module.RegisterMethod("BroadcastMessage", Types(typeof(string)),(input1, input2) => input1.As<Component>().BroadcastMessage(input2.As<string>()));
            module.RegisterMethod("BroadcastMessage", Types(typeof(string),typeof(SendMessageOptions)),(input1, input2, input3)  =>  input1.As<Component>().BroadcastMessage(input2.As<string>(),input3.As<SendMessageOptions>()));
            module.ClearTempMembers();
                          if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}