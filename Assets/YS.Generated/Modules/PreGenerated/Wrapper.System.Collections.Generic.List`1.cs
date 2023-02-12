using  System.Collections.Generic;
using System;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_System_Collections_Generic_ListModule<T>(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(List<T>));
            if (baseType != null) module.BaseModule = baseType.GetModule();
            module.RegisterConstructor(result => result.SetValue( new List<T>()));
            module.RegisterConstructor(typeof(int),(result, input1) => result.SetValue( new List<T>(input1.As<int>())));
            module.RegisterConstructor(typeof(IEnumerable<T>),(result, input1) => result.SetValue( new List<T>(input1.As<IEnumerable<T>>())));
            module.RegisterMethod("get_Capacity", (result, input1) => result.SetValue(input1.As<List<T>>().Capacity));
            module.RegisterMethod("set_Capacity", (input1, input2) => input1.As<List<T>>().Capacity=input2.As<int>());
            module.RegisterMethod("get_Count", (result, input1) => result.SetValue(input1.As<List<T>>().Count));
            module.RegisterMethod("get_Item", (result, input1, input2) => result.SetValue(input1.As<List<T>>()[input2.As<int>()]));
            module.RegisterMethod("set_Item", (input1, input2, input3)  =>  input1.As<List<T>>()[input2.As<int>()]=input3.As<T>());
            module.RegisterMethod("Add", (input1, input2) => input1.As<List<T>>().Add(input2.As<T>()));
            module.RegisterMethod("AddRange", (input1, input2) => input1.As<List<T>>().AddRange(input2.As<IEnumerable<T>>()));
            module.RegisterMethod("AsReadOnly", (result, input1) => result.SetValue(input1.As<List<T>>().AsReadOnly()));
            module.RegisterMethod("BinarySearch", Types(typeof(int),typeof(int),typeof(T),typeof(IComparer<T>)),(result, input1, input2, input3, input4, input5) => result.SetValue(input1.As<List<T>>().BinarySearch(input2.As<int>(),input3.As<int>(),input4.As<T>(),input5.As<IComparer<T>>())));
            module.RegisterMethod("BinarySearch", Types(typeof(T)),(result, input1, input2) => result.SetValue(input1.As<List<T>>().BinarySearch(input2.As<T>())));
            module.RegisterMethod("BinarySearch", Types(typeof(T),typeof(IComparer<T>)),(result, input1, input2, input3) => result.SetValue(input1.As<List<T>>().BinarySearch(input2.As<T>(),input3.As<IComparer<T>>())));
            module.RegisterMethod("Clear", input1 => input1.As<List<T>>().Clear());
            module.RegisterMethod("Contains", (result, input1, input2) => result.SetValue(input1.As<List<T>>().Contains(input2.As<T>())));
            module.RegisterMethod("CopyTo", Types(typeof(T[])),(input1, input2) => input1.As<List<T>>().CopyTo(input2.As<T[]>()));
            module.RegisterMethod("CopyTo", Types(typeof(int),typeof(T[]),typeof(int),typeof(int)),(input1, input2, input3, input4, input5)  => input1.As<List<T>>().CopyTo(input2.As<int>(),input3.As<T[]>(),input4.As<int>(),input5.As<int>()));
            module.RegisterMethod("CopyTo", Types(typeof(T[]),typeof(int)),(input1, input2, input3)  =>  input1.As<List<T>>().CopyTo(input2.As<T[]>(),input3.As<int>()));
            module.RegisterMethod("Exists", (result, input1, input2) => result.SetValue(input1.As<List<T>>().Exists(input2.As<Predicate<T>>())));
            module.RegisterMethod("Find", (result, input1, input2) => result.SetValue(input1.As<List<T>>().Find(input2.As<Predicate<T>>())));
            module.RegisterMethod("FindAll", (result, input1, input2) => result.SetValue(input1.As<List<T>>().FindAll(input2.As<Predicate<T>>())));
            module.RegisterMethod("FindIndex", Types(typeof(Predicate<T>)),(result, input1, input2) => result.SetValue(input1.As<List<T>>().FindIndex(input2.As<Predicate<T>>())));
            module.RegisterMethod("FindIndex", Types(typeof(int),typeof(Predicate<T>)),(result, input1, input2, input3) => result.SetValue(input1.As<List<T>>().FindIndex(input2.As<int>(),input3.As<Predicate<T>>())));
            module.RegisterMethod("FindIndex", Types(typeof(int),typeof(int),typeof(Predicate<T>)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<List<T>>().FindIndex(input2.As<int>(),input3.As<int>(),input4.As<Predicate<T>>())));
            module.RegisterMethod("FindLast", (result, input1, input2) => result.SetValue(input1.As<List<T>>().FindLast(input2.As<Predicate<T>>())));
            module.RegisterMethod("FindLastIndex", Types(typeof(Predicate<T>)),(result, input1, input2) => result.SetValue(input1.As<List<T>>().FindLastIndex(input2.As<Predicate<T>>())));
            module.RegisterMethod("FindLastIndex", Types(typeof(int),typeof(Predicate<T>)),(result, input1, input2, input3) => result.SetValue(input1.As<List<T>>().FindLastIndex(input2.As<int>(),input3.As<Predicate<T>>())));
            module.RegisterMethod("FindLastIndex", Types(typeof(int),typeof(int),typeof(Predicate<T>)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<List<T>>().FindLastIndex(input2.As<int>(),input3.As<int>(),input4.As<Predicate<T>>())));
            module.RegisterMethod("ForEach", (input1, input2) => input1.As<List<T>>().ForEach(input2.As<Action<T>>()));
            module.RegisterMethod("GetEnumerator", (result, input1) => result.SetValue(input1.As<List<T>>().GetEnumerator()));
            module.RegisterMethod("GetRange", (result, input1, input2, input3) => result.SetValue(input1.As<List<T>>().GetRange(input2.As<int>(),input3.As<int>())));
            module.RegisterMethod("IndexOf", Types(typeof(T)),(result, input1, input2) => result.SetValue(input1.As<List<T>>().IndexOf(input2.As<T>())));
            module.RegisterMethod("IndexOf", Types(typeof(T),typeof(int)),(result, input1, input2, input3) => result.SetValue(input1.As<List<T>>().IndexOf(input2.As<T>(),input3.As<int>())));
            module.RegisterMethod("IndexOf", Types(typeof(T),typeof(int),typeof(int)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<List<T>>().IndexOf(input2.As<T>(),input3.As<int>(),input4.As<int>())));
            module.RegisterMethod("Insert", (input1, input2, input3)  =>  input1.As<List<T>>().Insert(input2.As<int>(),input3.As<T>()));
            module.RegisterMethod("InsertRange", (input1, input2, input3)  =>  input1.As<List<T>>().InsertRange(input2.As<int>(),input3.As<IEnumerable<T>>()));
            module.RegisterMethod("LastIndexOf", Types(typeof(T)),(result, input1, input2) => result.SetValue(input1.As<List<T>>().LastIndexOf(input2.As<T>())));
            module.RegisterMethod("LastIndexOf", Types(typeof(T),typeof(int)),(result, input1, input2, input3) => result.SetValue(input1.As<List<T>>().LastIndexOf(input2.As<T>(),input3.As<int>())));
            module.RegisterMethod("LastIndexOf", Types(typeof(T),typeof(int),typeof(int)),(result, input1, input2, input3, input4) => result.SetValue(input1.As<List<T>>().LastIndexOf(input2.As<T>(),input3.As<int>(),input4.As<int>())));
            module.RegisterMethod("Remove", (result, input1, input2) => result.SetValue(input1.As<List<T>>().Remove(input2.As<T>())));
            module.RegisterMethod("RemoveAll", (result, input1, input2) => result.SetValue(input1.As<List<T>>().RemoveAll(input2.As<Predicate<T>>())));
            module.RegisterMethod("RemoveAt", (input1, input2) => input1.As<List<T>>().RemoveAt(input2.As<int>()));
            module.RegisterMethod("RemoveRange", (input1, input2, input3)  =>  input1.As<List<T>>().RemoveRange(input2.As<int>(),input3.As<int>()));
            module.RegisterMethod("Reverse", Types(),input1 => input1.As<List<T>>().Reverse());
            module.RegisterMethod("Reverse", Types(typeof(int),typeof(int)),(input1, input2, input3)  =>  input1.As<List<T>>().Reverse(input2.As<int>(),input3.As<int>()));
            module.RegisterMethod("Sort", Types(),input1 => input1.As<List<T>>().Sort());
            module.RegisterMethod("Sort", Types(typeof(IComparer<T>)),(input1, input2) => input1.As<List<T>>().Sort(input2.As<IComparer<T>>()));
            module.RegisterMethod("Sort", Types(typeof(int),typeof(int),typeof(IComparer<T>)),(input1, input2, input3, input4)  => input1.As<List<T>>().Sort(input2.As<int>(),input3.As<int>(),input4.As<IComparer<T>>()));
            module.RegisterMethod("Sort", Types(typeof(Comparison<T>)),(input1, input2) => input1.As<List<T>>().Sort(input2.As<Comparison<T>>()));
            module.RegisterMethod("ToArray", (result, input1) => result.SetValue(input1.As<List<T>>().ToArray()));
            module.RegisterMethod("TrimExcess", input1 => input1.As<List<T>>().TrimExcess());
            module.RegisterMethod("TrueForAll", (result, input1, input2) => result.SetValue(input1.As<List<T>>().TrueForAll(input2.As<Predicate<T>>())));
            module.ClearTempMembers();
            return module;
        }
    }
}