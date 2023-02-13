using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

using YS.Collections;
using YS.Instructions;
using YS.Modules;

namespace YS.VM {
  
    public static class DelegateLibrary {

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
            switch (length) {
                case 0: return Delegates[index];
                case 1: return Delegate1s[index];
                case 2: return Delegate2s[index];
                case 3: return Delegate3s[index];
                case 4: return Delegate4s[index];
                case 5: return Delegate5s[index];
                case 6: return Delegate6s[index];
            }
            return length == MethodInfoInvoker.Id ? MethodInfos[index] : default;
        }
        public static (MethodInfo MethodInfo,MethodData Data)[] MethodInfos=new (MethodInfo,MethodData)[16];
       static int methodInfoCount;
        
        public static (Action Action,MethodData Data)[] Delegates=new (Action,MethodData)[16];
       static int action0Count;
       public static int ActionCount => action0Count;
       public static (Action<Variable> Action,MethodData Data)[] Delegate1s=new (Action<Variable>,MethodData)[16];
       static int action1Count;
       public static int Action1Count => action1Count;
       public static (Action<Variable,Variable> Action,MethodData Data)[] Delegate2s=new (Action<Variable,Variable>,MethodData)[16];
       static int action2Count;
       public static int Action2Count => action1Count;
       public static (Action<Variable,Variable,Variable> Action,MethodData Data)[] Delegate3s=new (Action<Variable,Variable,Variable>,MethodData)[16];
       static int action3Count;
       public static (Action<Variable,Variable,Variable,Variable> Action,MethodData Data)[] Delegate4s=new (Action<Variable,Variable,Variable,Variable>,MethodData)[16];
       static int action4Count;
       public static (Action<Variable,Variable,Variable,Variable,Variable> Action,MethodData Data)[] Delegate5s=new (Action<Variable,Variable,Variable,Variable,Variable>,MethodData)[16];
       static int action5Count;
       public static (Action<Variable,Variable,Variable,Variable,Variable,Variable> Action,MethodData Data)[] Delegate6s=new (Action<Variable,Variable,Variable,Variable,Variable,Variable>,MethodData)[16];
       static int action6Count;
        
        
       public static MethodID Add(MethodData function,MethodInfo methodInfo) {
           CollectionsUtility.Add(ref MethodInfos,ref methodInfoCount, (methodInfo,function));
           return new MethodID ( (ushort) (methodInfoCount - 1),MethodInfoInvoker.Id);
        }
        public static ushort Add(MethodData function,Delegate action,int paramLength) {
            switch (paramLength) {
                case 0:CollectionsUtility.Add(ref Delegates,ref action0Count, ((Action)action,function));
                     return (ushort) (action0Count - 1);
                case 1:CollectionsUtility.Add(ref Delegate1s,ref action1Count, ((Action<Variable>)action,function));
                     return (ushort) (action1Count - 1);
                case 2:CollectionsUtility.Add(ref Delegate2s,ref action2Count, ((Action<Variable,Variable>)action,function));
                     return (ushort) (action2Count - 1);
                case 3:CollectionsUtility.Add(ref Delegate3s,ref action3Count, ((Action<Variable,Variable,Variable>)action,function));
                     return (ushort) (action3Count - 1);
                case 4:CollectionsUtility.Add(ref Delegate4s,ref action4Count, ((Action<Variable,Variable,Variable,Variable>)action,function));
                     return (ushort) (action4Count - 1);
                case 5:CollectionsUtility.Add(ref Delegate5s,ref action5Count, ((Action<Variable,Variable,Variable,Variable,Variable>)action,function));
                     return (ushort) (action5Count - 1);
                case 6:CollectionsUtility.Add(ref Delegate6s,ref action6Count, ((Action<Variable,Variable,Variable,Variable,Variable,Variable>)action,function));
                     return (ushort) (action6Count - 1);
                
            }
            
            
            return default;
        }
       
        
        
    }
  
    
}