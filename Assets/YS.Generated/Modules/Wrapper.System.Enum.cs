using Enum = System.Enum;
using Type = System.Type;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_System_EnumModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Enum));
            module.RegisterMethod("Parse", Types(typeof(Type),typeof(string)),(result, input1, input2) => result.SetValue(Enum.Parse(input1.As<Type>(),input2.As<string>())));
            module.RegisterMethod("Parse", Types(typeof(Type),typeof(string),typeof(bool)),(result, input1, input2, input3) => result.SetValue(Enum.Parse(input1.As<Type>(),input2.As<string>(),input3.As<bool>())));
            module.RegisterMethod("GetUnderlyingType", (result, input1) => result.SetValue(Enum.GetUnderlyingType(input1.As<Type>())));
            module.RegisterMethod("GetValues", (result, input1) => result.SetValue(Enum.GetValues(input1.As<Type>())));
            module.RegisterMethod("GetName", (result, input1, input2) => result.SetValue(Enum.GetName(input1.As<Type>(),input2.As<object>())));
            module.RegisterMethod("GetNames", (result, input1) => result.SetValue(Enum.GetNames(input1.As<Type>())));
            module.RegisterMethod("ToObject", Types(typeof(Type),typeof(object)),(result, input1, input2) => result.SetValue(Enum.ToObject(input1.As<Type>(),input2.As<object>())));
            module.RegisterMethod("ToObject", Types(typeof(Type),typeof(sbyte)),(result, input1, input2) => result.SetValue(Enum.ToObject(input1.As<Type>(),input2.As<sbyte>())));
            module.RegisterMethod("ToObject", Types(typeof(Type),typeof(short)),(result, input1, input2) => result.SetValue(Enum.ToObject(input1.As<Type>(),input2.As<short>())));
            module.RegisterMethod("ToObject", Types(typeof(Type),typeof(int)),(result, input1, input2) => result.SetValue(Enum.ToObject(input1.As<Type>(),input2.As<int>())));
            module.RegisterMethod("ToObject", Types(typeof(Type),typeof(byte)),(result, input1, input2) => result.SetValue(Enum.ToObject(input1.As<Type>(),input2.As<byte>())));
            module.RegisterMethod("ToObject", Types(typeof(Type),typeof(ushort)),(result, input1, input2) => result.SetValue(Enum.ToObject(input1.As<Type>(),input2.As<ushort>())));
            module.RegisterMethod("ToObject", Types(typeof(Type),typeof(uint)),(result, input1, input2) => result.SetValue(Enum.ToObject(input1.As<Type>(),input2.As<uint>())));
            module.RegisterMethod("ToObject", Types(typeof(Type),typeof(long)),(result, input1, input2) => result.SetValue(Enum.ToObject(input1.As<Type>(),input2.As<long>())));
            module.RegisterMethod("ToObject", Types(typeof(Type),typeof(ulong)),(result, input1, input2) => result.SetValue(Enum.ToObject(input1.As<Type>(),input2.As<ulong>())));
            module.RegisterMethod("IsDefined", (result, input1, input2) => result.SetValue(Enum.IsDefined(input1.As<Type>(),input2.As<object>())));
            module.RegisterMethod("Format", (result, input1, input2, input3) => result.SetValue(Enum.Format(input1.As<Type>(),input2.As<object>(),input3.As<string>())));
            module.RegisterMethod("Equals", (result, input1, input2) => result.SetValue(input1.As<Enum>().Equals(input2.As<object>())));
            module.RegisterMethod("GetHashCode", (result, input1) => result.SetValue(input1.As<Enum>().GetHashCode()));
            module.RegisterMethod("ToString", Types(),(result, input1) => result.SetValue(input1.As<Enum>().ToString()));
            module.RegisterMethod("ToString", Types(typeof(string)),(result, input1, input2) => result.SetValue(input1.As<Enum>().ToString(input2.As<string>())));
            module.RegisterMethod("CompareTo", (result, input1, input2) => result.SetValue(input1.As<Enum>().CompareTo(input2.As<object>())));
            module.RegisterMethod("HasFlag", (result, input1, input2) => result.SetValue(input1.As<Enum>().HasFlag(input2.As<Enum>())));
            module.RegisterMethod("GetTypeCode", (result, input1) => result.SetValue(input1.As<Enum>().GetTypeCode()));
            module.RegisterMethod("TryParse", Types(typeof(Type),typeof(string),typeof(bool),typeof(object).MakeByRefType()),(result, input1, input2, input3, input4) => result.SetValue(Enum.TryParse(input1.As<Type>(),input2.As<string>(),input3.As<bool>(),out input4.As<object>())));
            module.RegisterMethod("TryParse", Types(typeof(Type),typeof(string),typeof(object).MakeByRefType()),(result, input1, input2, input3) => result.SetValue(Enum.TryParse(input1.As<Type>(),input2.As<string>(),out input3.As<object>())));
            module.ClearTempMembers();
                          if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}