using Array = System.Array;
using Type = System.Type;
using IComparer = System.Collections.IComparer;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_System_ArrayModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Array));
            if (baseType != null) module.BaseModule = baseType.GetModule();
            module.RegisterMethod("get_LongLength", (result, input1) => result.SetValue(input1.As<Array>().LongLength));
            module.RegisterMethod("get_IsFixedSize", (result, input1) => result.SetValue(input1.As<Array>().IsFixedSize));
            module.RegisterMethod("get_IsReadOnly", (result, input1) => result.SetValue(input1.As<Array>().IsReadOnly));
            module.RegisterMethod("get_IsSynchronized", (result, input1) => result.SetValue(input1.As<Array>().IsSynchronized));
            module.RegisterMethod("get_SyncRoot", (result, input1) => result.SetValue(input1.As<Array>().SyncRoot));
            module.RegisterMethod("get_Length", (result, input1) => result.SetValue(input1.As<Array>().Length));
            module.RegisterMethod("get_Rank", (result, input1) => result.SetValue(input1.As<Array>().Rank));
            module.RegisterMethod("CreateInstance", Types(typeof(Type),typeof(long[])),(result, input1, input2) => result.SetValue(Array.CreateInstance(input1.As<Type>(),input2.As<long[]>())));
            module.RegisterMethod("CreateInstance", Types(typeof(Type),typeof(int)),(result, input1, input2) => result.SetValue(Array.CreateInstance(input1.As<Type>(),input2.As<int>())));
            module.RegisterMethod("CreateInstance", Types(typeof(Type),typeof(int),typeof(int)),(result, input1, input2, input3) => result.SetValue(Array.CreateInstance(input1.As<Type>(),input2.As<int>(),input3.As<int>())));
            module.RegisterMethod("CreateInstance", Types(typeof(Type),typeof(int),typeof(int),typeof(int)),(result, input1, input2, input3, input4) => result.SetValue(Array.CreateInstance(input1.As<Type>(),input2.As<int>(),input3.As<int>(),input4.As<int>())));
            module.RegisterMethod("CreateInstance", Types(typeof(Type),typeof(int[])),(result, input1, input2) => result.SetValue(Array.CreateInstance(input1.As<Type>(),input2.As<int[]>())));
            module.RegisterMethod("CreateInstance", Types(typeof(Type),typeof(int[]),typeof(int[])),(result, input1, input2, input3) => result.SetValue(Array.CreateInstance(input1.As<Type>(),input2.As<int[]>(),input3.As<int[]>())));
            module.RegisterMethod("CopyTo", Types(typeof(Array),typeof(int)),(input1, input2, input3)  =>  input1.As<Array>().CopyTo(input2.As<Array>(),input3.As<int>()));
            module.RegisterMethod("CopyTo", Types(typeof(Array),typeof(long)),(input1, input2, input3)  =>  input1.As<Array>().CopyTo(input2.As<Array>(),input3.As<long>()));
            module.RegisterMethod("Clone", (result, input1) => result.SetValue(input1.As<Array>().Clone()));
            module.RegisterMethod("BinarySearch", Types(typeof(Array),typeof(object)),(result, input1, input2) => result.SetValue(Array.BinarySearch(input1.As<Array>(),input2.As<object>())));
            module.RegisterMethod("BinarySearch", Types(typeof(Array),typeof(int),typeof(int),typeof(object)),(result, input1, input2, input3, input4) => result.SetValue(Array.BinarySearch(input1.As<Array>(),input2.As<int>(),input3.As<int>(),input4.As<object>())));
            module.RegisterMethod("BinarySearch", Types(typeof(Array),typeof(object),typeof(IComparer)),(result, input1, input2, input3) => result.SetValue(Array.BinarySearch(input1.As<Array>(),input2.As<object>(),input3.As<IComparer>())));
            module.RegisterMethod("BinarySearch", Types(typeof(Array),typeof(int),typeof(int),typeof(object),typeof(IComparer)),(result, input1, input2, input3, input4, input5) => result.SetValue(Array.BinarySearch(input1.As<Array>(),input2.As<int>(),input3.As<int>(),input4.As<object>(),input5.As<IComparer>())));
            module.RegisterMethod("Copy", Types(typeof(Array),typeof(Array),typeof(long)),(input1, input2, input3)  =>  Array.Copy(input1.As<Array>(),input2.As<Array>(),input3.As<long>()));
            module.RegisterMethod("Copy", Types(typeof(Array),typeof(long),typeof(Array),typeof(long),typeof(long)),(input1, input2, input3, input4, input5)  => Array.Copy(input1.As<Array>(),input2.As<long>(),input3.As<Array>(),input4.As<long>(),input5.As<long>()));
            module.RegisterMethod("Copy", Types(typeof(Array),typeof(Array),typeof(int)),(input1, input2, input3)  =>  Array.Copy(input1.As<Array>(),input2.As<Array>(),input3.As<int>()));
            module.RegisterMethod("Copy", Types(typeof(Array),typeof(int),typeof(Array),typeof(int),typeof(int)),(input1, input2, input3, input4, input5)  => Array.Copy(input1.As<Array>(),input2.As<int>(),input3.As<Array>(),input4.As<int>(),input5.As<int>()));
            module.RegisterMethod("GetLongLength", (result, input1, input2) => result.SetValue(input1.As<Array>().GetLongLength(input2.As<int>())));
            module.RegisterMethod("GetValue", Types(typeof(long)),(result, input1, input2) => result.SetValue(input1.As<Array>().GetValue(input2.As<long>())));
            module.RegisterMethod("GetValue", Types(typeof(long),typeof(long)),(result, input1, input2, input3) => result.SetValue(input1.As<Array>().GetValue(input2.As<long>(),input3.As<long>())));
            module.RegisterMethod("GetValue", Types(typeof(long),typeof(long),typeof(long)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<Array>().GetValue(input2.As<long>(),input3.As<long>(),input4.As<long>())));
            module.RegisterMethod("GetValue", Types(typeof(long[])),(result, input1, input2) => result.SetValue(input1.As<Array>().GetValue(input2.As<long[]>())));
            module.RegisterMethod("GetValue", Types(typeof(int[])),(result, input1, input2) => result.SetValue(input1.As<Array>().GetValue(input2.As<int[]>())));
            module.RegisterMethod("GetValue", Types(typeof(int)),(result, input1, input2) => result.SetValue(input1.As<Array>().GetValue(input2.As<int>())));
            module.RegisterMethod("GetValue", Types(typeof(int),typeof(int)),(result, input1, input2, input3) => result.SetValue(input1.As<Array>().GetValue(input2.As<int>(),input3.As<int>())));
            module.RegisterMethod("GetValue", Types(typeof(int),typeof(int),typeof(int)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<Array>().GetValue(input2.As<int>(),input3.As<int>(),input4.As<int>())));
            module.RegisterMethod("IndexOf", Types(typeof(Array),typeof(object)),(result, input1, input2) => result.SetValue(Array.IndexOf(input1.As<Array>(),input2.As<object>())));
            module.RegisterMethod("IndexOf", Types(typeof(Array),typeof(object),typeof(int)),(result, input1, input2, input3) => result.SetValue(Array.IndexOf(input1.As<Array>(),input2.As<object>(),input3.As<int>())));
            module.RegisterMethod("IndexOf", Types(typeof(Array),typeof(object),typeof(int),typeof(int)),(result, input1, input2, input3, input4) => result.SetValue(Array.IndexOf(input1.As<Array>(),input2.As<object>(),input3.As<int>(),input4.As<int>())));
            module.RegisterMethod("LastIndexOf", Types(typeof(Array),typeof(object)),(result, input1, input2) => result.SetValue(Array.LastIndexOf(input1.As<Array>(),input2.As<object>())));
            module.RegisterMethod("LastIndexOf", Types(typeof(Array),typeof(object),typeof(int)),(result, input1, input2, input3) => result.SetValue(Array.LastIndexOf(input1.As<Array>(),input2.As<object>(),input3.As<int>())));
            module.RegisterMethod("LastIndexOf", Types(typeof(Array),typeof(object),typeof(int),typeof(int)),(result, input1, input2, input3, input4) => result.SetValue(Array.LastIndexOf(input1.As<Array>(),input2.As<object>(),input3.As<int>(),input4.As<int>())));
            module.RegisterMethod("Reverse", Types(typeof(Array)),input1 => Array.Reverse(input1.As<Array>()));
            module.RegisterMethod("Reverse", Types(typeof(Array),typeof(int),typeof(int)),(input1, input2, input3)  =>  Array.Reverse(input1.As<Array>(),input2.As<int>(),input3.As<int>()));
            module.RegisterMethod("SetValue", Types(typeof(object),typeof(long)),(input1, input2, input3)  =>  input1.As<Array>().SetValue(input2.As<object>(),input3.As<long>()));
            module.RegisterMethod("SetValue", Types(typeof(object),typeof(long),typeof(long)),(input1, input2, input3, input4)  => input1.As<Array>().SetValue(input2.As<object>(),input3.As<long>(),input4.As<long>()));
            module.RegisterMethod("SetValue", Types(typeof(object),typeof(long),typeof(long),typeof(long)),(input1, input2, input3, input4, input5)  => input1.As<Array>().SetValue(input2.As<object>(),input3.As<long>(),input4.As<long>(),input5.As<long>()));
            module.RegisterMethod("SetValue", Types(typeof(object),typeof(long[])),(input1, input2, input3)  =>  input1.As<Array>().SetValue(input2.As<object>(),input3.As<long[]>()));
            module.RegisterMethod("SetValue", Types(typeof(object),typeof(int[])),(input1, input2, input3)  =>  input1.As<Array>().SetValue(input2.As<object>(),input3.As<int[]>()));
            module.RegisterMethod("SetValue", Types(typeof(object),typeof(int)),(input1, input2, input3)  =>  input1.As<Array>().SetValue(input2.As<object>(),input3.As<int>()));
            module.RegisterMethod("SetValue", Types(typeof(object),typeof(int),typeof(int)),(input1, input2, input3, input4)  => input1.As<Array>().SetValue(input2.As<object>(),input3.As<int>(),input4.As<int>()));
            module.RegisterMethod("SetValue", Types(typeof(object),typeof(int),typeof(int),typeof(int)),(input1, input2, input3, input4, input5)  => input1.As<Array>().SetValue(input2.As<object>(),input3.As<int>(),input4.As<int>(),input5.As<int>()));
            module.RegisterMethod("Sort", Types(typeof(Array)),input1 => Array.Sort(input1.As<Array>()));
            module.RegisterMethod("Sort", Types(typeof(Array),typeof(int),typeof(int)),(input1, input2, input3)  =>  Array.Sort(input1.As<Array>(),input2.As<int>(),input3.As<int>()));
            module.RegisterMethod("Sort", Types(typeof(Array),typeof(IComparer)),(input1, input2) => Array.Sort(input1.As<Array>(),input2.As<IComparer>()));
            module.RegisterMethod("Sort", Types(typeof(Array),typeof(int),typeof(int),typeof(IComparer)),(input1, input2, input3, input4)  => Array.Sort(input1.As<Array>(),input2.As<int>(),input3.As<int>(),input4.As<IComparer>()));
            module.RegisterMethod("Sort", Types(typeof(Array),typeof(Array)),(input1, input2) => Array.Sort(input1.As<Array>(),input2.As<Array>()));
            module.RegisterMethod("Sort", Types(typeof(Array),typeof(Array),typeof(IComparer)),(input1, input2, input3)  =>  Array.Sort(input1.As<Array>(),input2.As<Array>(),input3.As<IComparer>()));
            module.RegisterMethod("Sort", Types(typeof(Array),typeof(Array),typeof(int),typeof(int)),(input1, input2, input3, input4)  => Array.Sort(input1.As<Array>(),input2.As<Array>(),input3.As<int>(),input4.As<int>()));
            module.RegisterMethod("Sort", Types(typeof(Array),typeof(Array),typeof(int),typeof(int),typeof(IComparer)),(input1, input2, input3, input4, input5)  => Array.Sort(input1.As<Array>(),input2.As<Array>(),input3.As<int>(),input4.As<int>(),input5.As<IComparer>()));
            module.RegisterMethod("GetEnumerator", (result, input1) => result.SetValue(input1.As<Array>().GetEnumerator()));
            module.RegisterMethod("GetLength", (result, input1, input2) => result.SetValue(input1.As<Array>().GetLength(input2.As<int>())));
            module.RegisterMethod("GetLowerBound", (result, input1, input2) => result.SetValue(input1.As<Array>().GetLowerBound(input2.As<int>())));
            module.RegisterMethod("GetUpperBound", (result, input1, input2) => result.SetValue(input1.As<Array>().GetUpperBound(input2.As<int>())));
            module.RegisterMethod("Clear", (input1, input2, input3)  =>  Array.Clear(input1.As<Array>(),input2.As<int>(),input3.As<int>()));
            module.RegisterMethod("ConstrainedCopy", (input1, input2, input3, input4, input5)  => Array.ConstrainedCopy(input1.As<Array>(),input2.As<int>(),input3.As<Array>(),input4.As<int>(),input5.As<int>()));
            module.RegisterMethod("Initialize", input1 => input1.As<Array>().Initialize());
            module.ClearTempMembers();
            return module;
        }
    }
}