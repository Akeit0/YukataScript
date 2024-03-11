#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif
using Enum = System.Enum;
using _Enum = YS.Variable<System.Enum>;
using _object = YS.Variable<object>;
using Type = System.Type;
using _Type = YS.Variable<System.Type>;
using _string = YS.Variable<string>;
using _bool = YS.Variable<bool>;
using Array = System.Array;
using _Array = YS.Variable<System.Array>;
using _sbyte = YS.Variable<sbyte>;
using _short = YS.Variable<short>;
using _int = YS.Variable<int>;
using _byte = YS.Variable<byte>;
using _ushort = YS.Variable<ushort>;
using _uint = YS.Variable<uint>;
using _long = YS.Variable<long>;
using _ulong = YS.Variable<ulong>;
using TypeCode = System.TypeCode;
using _TypeCode = YS.Variable<System.TypeCode>;

using static System.Runtime.CompilerServices.Unsafe;
using __V = YS.Variable;
namespace YS.Generated {
    public static partial  class Wrapper {
       static void System_Enum_Parse(__V res,__V  i1, __V  i2) => As<_object>(res).value=Enum.Parse(As<_Type>(i1).value,As<_string>(i2).value);
       static void System_Enum_Parse_(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_object>(res).value=Enum.Parse(As<_Type>(i1).value,As<_string>(i2).value,As<_bool>(i3).value);
       static void System_Enum_GetUnderlyingType(__V res, __V  i1) => As<_Type>(res).value=Enum.GetUnderlyingType(As<_Type>(i1).value);
       static void System_Enum_GetValues(__V res, __V  i1) => As<_Array>(res).value=Enum.GetValues(As<_Type>(i1).value);
       static void System_Enum_GetName(__V res,__V  i1, __V  i2) => As<_string>(res).value=Enum.GetName(As<_Type>(i1).value,As<_object>(i2).value);
       static void System_Enum_GetNames(__V res, __V  i1) => As<Variable<string[]>>(res).value=Enum.GetNames(As<_Type>(i1).value);
       static void System_Enum_ToObject(__V res,__V  i1, __V  i2) => As<_object>(res).value=Enum.ToObject(As<_Type>(i1).value,As<_object>(i2).value);
       static void System_Enum_ToObject_(__V res,__V  i1, __V  i2) => As<_object>(res).value=Enum.ToObject(As<_Type>(i1).value,As<_sbyte>(i2).value);
       static void System_Enum_ToObject__(__V res,__V  i1, __V  i2) => As<_object>(res).value=Enum.ToObject(As<_Type>(i1).value,As<_short>(i2).value);
       static void System_Enum_ToObject___(__V res,__V  i1, __V  i2) => As<_object>(res).value=Enum.ToObject(As<_Type>(i1).value,As<_int>(i2).value);
       static void System_Enum_ToObject____(__V res,__V  i1, __V  i2) => As<_object>(res).value=Enum.ToObject(As<_Type>(i1).value,As<_byte>(i2).value);
       static void System_Enum_ToObject_____(__V res,__V  i1, __V  i2) => As<_object>(res).value=Enum.ToObject(As<_Type>(i1).value,As<_ushort>(i2).value);
       static void System_Enum_ToObject______(__V res,__V  i1, __V  i2) => As<_object>(res).value=Enum.ToObject(As<_Type>(i1).value,As<_uint>(i2).value);
       static void System_Enum_ToObject_______(__V res,__V  i1, __V  i2) => As<_object>(res).value=Enum.ToObject(As<_Type>(i1).value,As<_long>(i2).value);
       static void System_Enum_ToObject________(__V res,__V  i1, __V  i2) => As<_object>(res).value=Enum.ToObject(As<_Type>(i1).value,As<_ulong>(i2).value);
       static void System_Enum_IsDefined(__V res,__V  i1, __V  i2) => As<_bool>(res).value=Enum.IsDefined(As<_Type>(i1).value,As<_object>(i2).value);
       static void System_Enum_Format(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_string>(res).value=Enum.Format(As<_Type>(i1).value,As<_object>(i2).value,As<_string>(i3).value);
       static void System_Enum_Equals(__V res,__V  i1, __V  i2) => As<_bool>(res).value=As<_Enum>(i1).value.Equals(As<_object>(i2).value);
       static void System_Enum_GetHashCode(__V res, __V  i1) => As<_int>(res).value=As<_Enum>(i1).value.GetHashCode();
       static void System_Enum_ToString(__V res, __V  i1) => As<_string>(res).value=As<_Enum>(i1).value.ToString();
       static void System_Enum_ToString_(__V res,__V  i1, __V  i2) => As<_string>(res).value=As<_Enum>(i1).value.ToString(As<_string>(i2).value);
       static void System_Enum_CompareTo(__V res,__V  i1, __V  i2) => As<_int>(res).value=As<_Enum>(i1).value.CompareTo(As<_object>(i2).value);
       static void System_Enum_HasFlag(__V res,__V  i1, __V  i2) => As<_bool>(res).value=As<_Enum>(i1).value.HasFlag(As<_Enum>(i2).value);
       static void System_Enum_GetTypeCode(__V res, __V  i1) => As<_TypeCode>(res).value=As<_Enum>(i1).value.GetTypeCode();
       static void System_Enum_TryParse(__V res,__V  i1, __V  i2, __V  i3, __V  i4)  => As<_bool>(res).value=Enum.TryParse(As<_Type>(i1).value,As<_string>(i2).value,As<_bool>(i3).value,out As<_object>(i4).value);
       static void System_Enum_TryParse_(__V res,__V  i1, __V  i2, __V  i3)  =>  As<_bool>(res).value=Enum.TryParse(As<_Type>(i1).value,As<_string>(i2).value,out As<_object>(i3).value);
#if !AOT

        static TypeModule Create_System_EnumModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Enum));
            module.RegisterMethod3("Parse", Types(typeof(Type),typeof(string)),System_Enum_Parse);
            module.RegisterMethod4("Parse", Types(typeof(Type),typeof(string),typeof(bool)),System_Enum_Parse_);
            module.RegisterMethod2("GetUnderlyingType",System_Enum_GetUnderlyingType);
            module.RegisterMethod2("GetValues",System_Enum_GetValues);
            module.RegisterMethod3("GetName",System_Enum_GetName);
            module.RegisterMethod2("GetNames",System_Enum_GetNames);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(object)),System_Enum_ToObject);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(sbyte)),System_Enum_ToObject_);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(short)),System_Enum_ToObject__);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(int)),System_Enum_ToObject___);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(byte)),System_Enum_ToObject____);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(ushort)),System_Enum_ToObject_____);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(uint)),System_Enum_ToObject______);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(long)),System_Enum_ToObject_______);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(ulong)),System_Enum_ToObject________);
            module.RegisterMethod3("IsDefined",System_Enum_IsDefined);
            module.RegisterMethod4("Format",System_Enum_Format);
            module.RegisterMethod3("Equals",System_Enum_Equals);
            module.RegisterMethod2("GetHashCode",System_Enum_GetHashCode);
            module.RegisterMethod2("ToString", Types(),System_Enum_ToString);
            module.RegisterMethod3("ToString", Types(typeof(string)),System_Enum_ToString_);
            module.RegisterMethod3("CompareTo",System_Enum_CompareTo);
            module.RegisterMethod3("HasFlag",System_Enum_HasFlag);
            module.RegisterMethod2("GetTypeCode",System_Enum_GetTypeCode);
            module.RegisterMethod5("TryParse", Types(typeof(Type),typeof(string),typeof(bool),typeof(object).MakeByRefType()),System_Enum_TryParse);
            module.RegisterMethod4("TryParse", Types(typeof(Type),typeof(string),typeof(object).MakeByRefType()),System_Enum_TryParse_);
#else
        static unsafe TypeModule Create_System_EnumModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Enum));
            module.RegisterMethod3("Parse", Types(typeof(Type),typeof(string)),&System_Enum_Parse);
            module.RegisterMethod4("Parse", Types(typeof(Type),typeof(string),typeof(bool)),&System_Enum_Parse_);
            module.RegisterMethod2("GetUnderlyingType",&System_Enum_GetUnderlyingType);
            module.RegisterMethod2("GetValues",&System_Enum_GetValues);
            module.RegisterMethod3("GetName",&System_Enum_GetName);
            module.RegisterMethod2("GetNames",&System_Enum_GetNames);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(object)),&System_Enum_ToObject);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(sbyte)),&System_Enum_ToObject_);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(short)),&System_Enum_ToObject__);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(int)),&System_Enum_ToObject___);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(byte)),&System_Enum_ToObject____);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(ushort)),&System_Enum_ToObject_____);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(uint)),&System_Enum_ToObject______);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(long)),&System_Enum_ToObject_______);
            module.RegisterMethod3("ToObject", Types(typeof(Type),typeof(ulong)),&System_Enum_ToObject________);
            module.RegisterMethod3("IsDefined",&System_Enum_IsDefined);
            module.RegisterMethod4("Format",&System_Enum_Format);
            module.RegisterMethod3("Equals",&System_Enum_Equals);
            module.RegisterMethod2("GetHashCode",&System_Enum_GetHashCode);
            module.RegisterMethod2("ToString", Types(),&System_Enum_ToString);
            module.RegisterMethod3("ToString", Types(typeof(string)),&System_Enum_ToString_);
            module.RegisterMethod3("CompareTo",&System_Enum_CompareTo);
            module.RegisterMethod3("HasFlag",&System_Enum_HasFlag);
            module.RegisterMethod2("GetTypeCode",&System_Enum_GetTypeCode);
            module.RegisterMethod5("TryParse", Types(typeof(Type),typeof(string),typeof(bool),typeof(object).MakeByRefType()),&System_Enum_TryParse);
            module.RegisterMethod4("TryParse", Types(typeof(Type),typeof(string),typeof(object).MakeByRefType()),&System_Enum_TryParse_);
#endif
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}