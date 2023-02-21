#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;
using YS.Collections;
using YS.Instructions;
using YS.Modules;
namespace YS.VM {
  
    public static unsafe class DelegateLibrary {

        public static Dictionary<Type, ushort> AwaiterIdDictionary=new (){
            {typeof(UniTime), 0},
            {typeof(IEnumerator), 1},
        };

        public static  List<IAwaiterSource> Awaiters = 
         new()   {new UniTimeAwaitSource(),new EnumeratorSource()
             
         };
        
        public static Dictionary<Type, ushort> EnumeratorIdDictionary=new (){
            {typeof(IntRange), 0}
        };

        public static List<IEnumeratorSource> Enumerators = 
           new() {new IntRangeEnumerator()};

        public static void RegisterArrayEnumerator<T>() {
            EnumeratorIdDictionary[typeof(T[])] = (ushort)Enumerators.Count;
            Enumerators.Add(new ArrayEnumerator<T>());
        }

      
        public static (object Delegate, MethodData MethodData)FromID(MethodID functionId) {
            var length = functionId.InstructionId;
            var index = functionId.Index;
            // switch (length) {
            //     case 0: return Delegates[index];
            //     case 1: return Delegate1s[index];
            //     case 2: return Delegate2s[index];
            //     case 3: return Delegate3s[index];
            //     case 4: return Delegate4s[index];
            //     case 5: return Delegate5s[index];
            //     case 6: return Delegate6s[index];
            // }
            return length == 7 ? MethodInfos[index] : default;
        }
        public static (MethodInfo MethodInfo,MethodData Data)[] MethodInfos=new (MethodInfo,MethodData)[16];
       static int methodInfoCount;
       public static MethodData[] DelegateDataArray=new MethodData[16];
       static int action0Count;
       public static int ActionCount => action0Count;
#if AOT
        public static delegate*<void> [] DelegatePtrArray=new delegate*<void>[16];
        public static delegate*<void> GetPtr0(ushort index) => DelegatePtrArray[index];
#else
        public static IntPtr [] DelegatePtrArray=new IntPtr[16];
       
        public static delegate*<void> GetPtr0(ushort index) => (delegate*<void> )DelegatePtrArray[index];
#endif
        
      
       
       
       public static (IntPtr Action,MethodData Data)[] Delegate1s=new (IntPtr,MethodData)[16];
       static int action1Count;
       public static delegate*<Variable,void> GetPtr1(ushort index) => (delegate*<Variable,void>)Delegate1s[index].Action;
       public static int Action1Count => action1Count;
       public static (IntPtr Action,MethodData Data)[] Delegate2s=new (IntPtr,MethodData)[16];
       static int action2Count;
       public static delegate*<Variable,Variable,void> GetPtr2(ushort index) => (delegate*<Variable,Variable,void>)Delegate2s[index].Action;
       public static int Action2Count => action1Count;
       public static (IntPtr Action,MethodData Data)[] Delegate3s=new ( IntPtr,MethodData)[16];
       static int action3Count;
       public static delegate*<Variable,Variable,Variable,void> GetPtr3(ushort index) => (delegate*<Variable,Variable,Variable,void>)Delegate3s[index].Action;
       
       public static (IntPtr Action,MethodData Data)[] Delegate4s=new (IntPtr,MethodData)[16];
       static int action4Count;
       public static delegate*<Variable,Variable,Variable,Variable,void> GetPtr4(ushort index) => (delegate*<Variable,Variable,Variable,Variable,void>)Delegate4s[index].Action;
       public static (IntPtr Action,MethodData Data)[] Delegate5s=new (IntPtr,MethodData)[16];
       static int action5Count;
       public static delegate*<Variable,Variable,Variable,Variable,Variable,void> GetPtr5(ushort index) => (delegate*<Variable,Variable,Variable,Variable,Variable,void>)Delegate5s[index].Action;

       public static (IntPtr Action,MethodData Data)[] Delegate6s=new (IntPtr,MethodData)[16];
      
       static int action6Count;
       public static delegate*<Variable,Variable,Variable,Variable,Variable,Variable,void> GetPtr6(ushort index) => (delegate*<Variable,Variable,Variable,Variable,Variable,Variable,void>)Delegate6s[index].Action;

        
       public static MethodID Add(MethodData function,MethodInfo methodInfo) {
           CollectionsUtility.Add(ref MethodInfos,ref methodInfoCount, (methodInfo,function));
           return new MethodID ( (ushort) (methodInfoCount - 1),7);
        }
       
#if AOT
       public static ushort Add(MethodData data,delegate*<void> pointer) {
            Add(ref DelegateDataArray, action0Count,  data);
            if (DelegatePtrArray.Length == action0Count) {
                var newArray = new delegate*<void>[action0Count * 2];
                for (int i = 0; i < action0Count; i++) {
                    newArray[i] = DelegatePtrArray[i];
                }
                DelegatePtrArray = newArray;
            }
            return (ushort) (action0Count++);
       }  
#endif
        public static ushort Add(MethodData data,IntPtr ptr,int argCount) {
            switch (argCount) {
#if !AOT
                case 0: {
                    Add(ref DelegateDataArray, action0Count,  data);
                    Add(ref DelegatePtrArray, action0Count,  ptr);
                    return (ushort) (action0Count++);
                }
#endif
                case 1: Add(ref Delegate1s, action1Count, (ptr, data));
                    return (ushort) (action1Count++ );
                case 2:Add(ref Delegate2s, action2Count, (ptr, data));
                    return (ushort) (action2Count++ );
                case 3: Add(ref Delegate3s, action3Count, (ptr, data));
                    return (ushort) (action3Count++ );
                case 4: Add(ref Delegate4s, action4Count, (ptr, data));
                    return (ushort) (action4Count++ );
                case 5:Add(ref Delegate5s, action5Count, (ptr, data));
                    return (ushort) (action5Count++ );
                case 6:Add(ref Delegate6s, action6Count, (ptr, data));
                    return (ushort) (action6Count++ );
            }
            return default;
        }
        static void Add<T>(ref T[] array, int  count, T element,float factor=2) {
            if(array.Length==count) {
                Array.Resize(ref array,(int) (array.Length * factor));
            }
            array[count] = element;
        }
        
    }
  
    
}