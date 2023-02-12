using Transform = UnityEngine.Transform;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Space = UnityEngine.Space;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_TransformModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Transform));
            module.RegisterPropertyGetter("position", (result, input1) => result.SetValue(input1.As<Transform>().position));
            module.RegisterPropertySetter("position", (input1, input2) => input1.As<Transform>().position=input2.As<Vector3>());
            module.RegisterPropertyGetter("localPosition", (result, input1) => result.SetValue(input1.As<Transform>().localPosition));
            module.RegisterPropertySetter("localPosition", (input1, input2) => input1.As<Transform>().localPosition=input2.As<Vector3>());
            module.RegisterPropertyGetter("eulerAngles", (result, input1) => result.SetValue(input1.As<Transform>().eulerAngles));
            module.RegisterPropertySetter("eulerAngles", (input1, input2) => input1.As<Transform>().eulerAngles=input2.As<Vector3>());
            module.RegisterPropertyGetter("localEulerAngles", (result, input1) => result.SetValue(input1.As<Transform>().localEulerAngles));
            module.RegisterPropertySetter("localEulerAngles", (input1, input2) => input1.As<Transform>().localEulerAngles=input2.As<Vector3>());
            module.RegisterPropertyGetter("right", (result, input1) => result.SetValue(input1.As<Transform>().right));
            module.RegisterPropertySetter("right", (input1, input2) => input1.As<Transform>().right=input2.As<Vector3>());
            module.RegisterPropertyGetter("up", (result, input1) => result.SetValue(input1.As<Transform>().up));
            module.RegisterPropertySetter("up", (input1, input2) => input1.As<Transform>().up=input2.As<Vector3>());
            module.RegisterPropertyGetter("forward", (result, input1) => result.SetValue(input1.As<Transform>().forward));
            module.RegisterPropertySetter("forward", (input1, input2) => input1.As<Transform>().forward=input2.As<Vector3>());
            module.RegisterPropertyGetter("rotation", (result, input1) => result.SetValue(input1.As<Transform>().rotation));
            module.RegisterPropertySetter("rotation", (input1, input2) => input1.As<Transform>().rotation=input2.As<Quaternion>());
            module.RegisterPropertyGetter("localRotation", (result, input1) => result.SetValue(input1.As<Transform>().localRotation));
            module.RegisterPropertySetter("localRotation", (input1, input2) => input1.As<Transform>().localRotation=input2.As<Quaternion>());
            module.RegisterPropertyGetter("localScale", (result, input1) => result.SetValue(input1.As<Transform>().localScale));
            module.RegisterPropertySetter("localScale", (input1, input2) => input1.As<Transform>().localScale=input2.As<Vector3>());
            module.RegisterPropertyGetter("parent", (result, input1) => result.SetValue(input1.As<Transform>().parent));
            module.RegisterPropertySetter("parent", (input1, input2) => input1.As<Transform>().parent=input2.As<Transform>());
            module.RegisterPropertyGetter("worldToLocalMatrix", (result, input1) => result.SetValue(input1.As<Transform>().worldToLocalMatrix));
            module.RegisterPropertyGetter("localToWorldMatrix", (result, input1) => result.SetValue(input1.As<Transform>().localToWorldMatrix));
            module.RegisterPropertyGetter("root", (result, input1) => result.SetValue(input1.As<Transform>().root));
            module.RegisterPropertyGetter("childCount", (result, input1) => result.SetValue(input1.As<Transform>().childCount));
            module.RegisterPropertyGetter("lossyScale", (result, input1) => result.SetValue(input1.As<Transform>().lossyScale));
            module.RegisterPropertyGetter("hasChanged", (result, input1) => result.SetValue(input1.As<Transform>().hasChanged));
            module.RegisterPropertySetter("hasChanged", (input1, input2) => input1.As<Transform>().hasChanged=input2.As<bool>());
            module.RegisterPropertyGetter("hierarchyCapacity", (result, input1) => result.SetValue(input1.As<Transform>().hierarchyCapacity));
            module.RegisterPropertySetter("hierarchyCapacity", (input1, input2) => input1.As<Transform>().hierarchyCapacity=input2.As<int>());
            module.RegisterPropertyGetter("hierarchyCount", (result, input1) => result.SetValue(input1.As<Transform>().hierarchyCount));
            module.RegisterMethod("SetParent", Types(typeof(Transform)),(input1, input2) => input1.As<Transform>().SetParent(input2.As<Transform>()));
            module.RegisterMethod("SetParent", Types(typeof(Transform),typeof(bool)),(input1, input2, input3)  =>  input1.As<Transform>().SetParent(input2.As<Transform>(),input3.As<bool>()));
            module.RegisterMethod("SetPositionAndRotation", (input1, input2, input3)  =>  input1.As<Transform>().SetPositionAndRotation(input2.As<Vector3>(),input3.As<Quaternion>()));
            module.RegisterMethod("Translate", Types(typeof(Vector3),typeof(Space)),(input1, input2, input3)  =>  input1.As<Transform>().Translate(input2.As<Vector3>(),input3.As<Space>()));
            module.RegisterMethod("Translate", Types(typeof(Vector3)),(input1, input2) => input1.As<Transform>().Translate(input2.As<Vector3>()));
            module.RegisterMethod("Translate", Types(typeof(float),typeof(float),typeof(float),typeof(Space)),(input1, input2, input3, input4, input5)  => input1.As<Transform>().Translate(input2.As<float>(),input3.As<float>(),input4.As<float>(),input5.As<Space>()));
            module.RegisterMethod("Translate", Types(typeof(float),typeof(float),typeof(float)),(input1, input2, input3, input4)  => input1.As<Transform>().Translate(input2.As<float>(),input3.As<float>(),input4.As<float>()));
            module.RegisterMethod("Translate", Types(typeof(Vector3),typeof(Transform)),(input1, input2, input3)  =>  input1.As<Transform>().Translate(input2.As<Vector3>(),input3.As<Transform>()));
            module.RegisterMethod("Translate", Types(typeof(float),typeof(float),typeof(float),typeof(Transform)),(input1, input2, input3, input4, input5)  => input1.As<Transform>().Translate(input2.As<float>(),input3.As<float>(),input4.As<float>(),input5.As<Transform>()));
            module.RegisterMethod("Rotate", Types(typeof(Vector3),typeof(Space)),(input1, input2, input3)  =>  input1.As<Transform>().Rotate(input2.As<Vector3>(),input3.As<Space>()));
            module.RegisterMethod("Rotate", Types(typeof(Vector3)),(input1, input2) => input1.As<Transform>().Rotate(input2.As<Vector3>()));
            module.RegisterMethod("Rotate", Types(typeof(float),typeof(float),typeof(float),typeof(Space)),(input1, input2, input3, input4, input5)  => input1.As<Transform>().Rotate(input2.As<float>(),input3.As<float>(),input4.As<float>(),input5.As<Space>()));
            module.RegisterMethod("Rotate", Types(typeof(float),typeof(float),typeof(float)),(input1, input2, input3, input4)  => input1.As<Transform>().Rotate(input2.As<float>(),input3.As<float>(),input4.As<float>()));
            module.RegisterMethod("Rotate", Types(typeof(Vector3),typeof(float),typeof(Space)),(input1, input2, input3, input4)  => input1.As<Transform>().Rotate(input2.As<Vector3>(),input3.As<float>(),input4.As<Space>()));
            module.RegisterMethod("Rotate", Types(typeof(Vector3),typeof(float)),(input1, input2, input3)  =>  input1.As<Transform>().Rotate(input2.As<Vector3>(),input3.As<float>()));
            module.RegisterMethod("RotateAround", Types(typeof(Vector3),typeof(Vector3),typeof(float)),(input1, input2, input3, input4)  => input1.As<Transform>().RotateAround(input2.As<Vector3>(),input3.As<Vector3>(),input4.As<float>()));
            module.RegisterMethod("LookAt", Types(typeof(Transform),typeof(Vector3)),(input1, input2, input3)  =>  input1.As<Transform>().LookAt(input2.As<Transform>(),input3.As<Vector3>()));
            module.RegisterMethod("LookAt", Types(typeof(Transform)),(input1, input2) => input1.As<Transform>().LookAt(input2.As<Transform>()));
            module.RegisterMethod("LookAt", Types(typeof(Vector3),typeof(Vector3)),(input1, input2, input3)  =>  input1.As<Transform>().LookAt(input2.As<Vector3>(),input3.As<Vector3>()));
            module.RegisterMethod("LookAt", Types(typeof(Vector3)),(input1, input2) => input1.As<Transform>().LookAt(input2.As<Vector3>()));
            module.RegisterMethod("TransformDirection", Types(typeof(Vector3)),(result, input1, input2) => result.SetValue(input1.As<Transform>().TransformDirection(input2.As<Vector3>())));
            module.RegisterMethod("TransformDirection", Types(typeof(float),typeof(float),typeof(float)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<Transform>().TransformDirection(input2.As<float>(),input3.As<float>(),input4.As<float>())));
            module.RegisterMethod("InverseTransformDirection", Types(typeof(Vector3)),(result, input1, input2) => result.SetValue(input1.As<Transform>().InverseTransformDirection(input2.As<Vector3>())));
            module.RegisterMethod("InverseTransformDirection", Types(typeof(float),typeof(float),typeof(float)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<Transform>().InverseTransformDirection(input2.As<float>(),input3.As<float>(),input4.As<float>())));
            module.RegisterMethod("TransformVector", Types(typeof(Vector3)),(result, input1, input2) => result.SetValue(input1.As<Transform>().TransformVector(input2.As<Vector3>())));
            module.RegisterMethod("TransformVector", Types(typeof(float),typeof(float),typeof(float)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<Transform>().TransformVector(input2.As<float>(),input3.As<float>(),input4.As<float>())));
            module.RegisterMethod("InverseTransformVector", Types(typeof(Vector3)),(result, input1, input2) => result.SetValue(input1.As<Transform>().InverseTransformVector(input2.As<Vector3>())));
            module.RegisterMethod("InverseTransformVector", Types(typeof(float),typeof(float),typeof(float)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<Transform>().InverseTransformVector(input2.As<float>(),input3.As<float>(),input4.As<float>())));
            module.RegisterMethod("TransformPoint", Types(typeof(Vector3)),(result, input1, input2) => result.SetValue(input1.As<Transform>().TransformPoint(input2.As<Vector3>())));
            module.RegisterMethod("TransformPoint", Types(typeof(float),typeof(float),typeof(float)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<Transform>().TransformPoint(input2.As<float>(),input3.As<float>(),input4.As<float>())));
            module.RegisterMethod("InverseTransformPoint", Types(typeof(Vector3)),(result, input1, input2) => result.SetValue(input1.As<Transform>().InverseTransformPoint(input2.As<Vector3>())));
            module.RegisterMethod("InverseTransformPoint", Types(typeof(float),typeof(float),typeof(float)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<Transform>().InverseTransformPoint(input2.As<float>(),input3.As<float>(),input4.As<float>())));
            module.RegisterMethod("DetachChildren", input1 => input1.As<Transform>().DetachChildren());
            module.RegisterMethod("SetAsFirstSibling", input1 => input1.As<Transform>().SetAsFirstSibling());
            module.RegisterMethod("SetAsLastSibling", input1 => input1.As<Transform>().SetAsLastSibling());
            module.RegisterMethod("SetSiblingIndex", (input1, input2) => input1.As<Transform>().SetSiblingIndex(input2.As<int>()));
            module.RegisterMethod("GetSiblingIndex", (result, input1) => result.SetValue(input1.As<Transform>().GetSiblingIndex()));
            module.RegisterMethod("Find", (result, input1, input2) => result.SetValue(input1.As<Transform>().Find(input2.As<string>())));
            module.RegisterMethod("IsChildOf", (result, input1, input2) => result.SetValue(input1.As<Transform>().IsChildOf(input2.As<Transform>())));
            module.RegisterMethod("GetEnumerator", (result, input1) => result.SetValue(input1.As<Transform>().GetEnumerator()));
            module.RegisterMethod("GetChild", (result, input1, input2) => result.SetValue(input1.As<Transform>().GetChild(input2.As<int>())));
            module.ClearTempMembers();
                          if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}