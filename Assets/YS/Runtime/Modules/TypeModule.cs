#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using YS.Collections;
using YS.Modules;
using YS.VM;
namespace YS {
    [UnityEngine.Scripting.Preserve]
    
    public sealed unsafe class TypeModule {
        public StringDictionary<object> Members { get; set; } = new StringDictionary<object>(32);
        public NameSpace NameSpace;
        public  Type DeclaringType { get; }

        public bool IsStatic;
        public TypeModule BaseModule { get; set; }=VariableModule.Instance;

        static readonly StringDictionary<object> _methodsDictionary=new StringDictionary<object>(128);
        static readonly Stack<List<(MethodInfo,ParameterInfo[])>> _listPool=new (128);

        static List<(MethodInfo,ParameterInfo[])> PopFromPool() {
            if (_listPool.TryPop(out var list)) {
                return list;
            }
            
            return new List<(MethodInfo,ParameterInfo[])>();
        }
        static void Push(List<(MethodInfo,ParameterInfo[])> list) {
            _listPool.Push(list);
        }
        public string ModuleName { get; set; }

        public TypeModule() {
            NameSpace = ModuleLibrary.Root;
            IsStatic = true;
        }
        
        public TypeModule(Type type,bool dontGetMethods=false) {
            DeclaringType = type;
            IsStatic = type.IsSealed && type.IsAbstract;
            if (IsStatic) {
                if (type == typeof(GlobalModule)) {
                    NameSpace = ModuleLibrary.Root;
                }
                else {
                    var nameSpaceName = type.Namespace;
                    if(string.IsNullOrEmpty(nameSpaceName))  NameSpace = ModuleLibrary.Root;
                    else if (ModuleLibrary.Root.TryGetNameSpace(type.Namespace, out NameSpace)) {
                    }
                }
              
            }
            
            if (!dontGetMethods) {
                var flag = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static ;
                if(!IsStatic) {
                    flag |= BindingFlags.Instance;
                }
                foreach (var info in type.GetMethods(flag)) {
                    Register(info);
                }
            }

        }
       
        public void ClearTempMembers() {
            foreach (var entry in _methodsDictionary.ReadEntries()) {
                if (entry.value is List<(MethodInfo, ParameterInfo[])> list) {
                    list.Clear();
                    Push(list);
                }
            }
            _methodsDictionary.Clear();
        }
        public static void Register(MethodInfo info) {
            if (info.IsGenericMethodDefinition) return;
            var  name = info.Name;
            if (_methodsDictionary.TryGetValue(name,out var member)) {
                switch (member) {
                    case MethodInfo last:
                        var pooled = PopFromPool();
                        pooled.Add((last,last.GetParameters()));
                        pooled.Add((info,info.GetParameters()));
                        _methodsDictionary[name] = pooled;
                        break;
                    case List<(MethodInfo,ParameterInfo[])> list:
                        list.Add((info,info.GetParameters()));
                        break;
                    default:
                        throw new Exception(member.ToString());
                }
            }
            else {
                
                _methodsDictionary[name] = info;
            }
        }
        public void Register(MethodData function) {
            if (IsStatic && function.MethodType == MethodType.Extension) {
                NameSpace.RegisterExtensionMethod(function);
            }
            var  name = function.MethodName;
            if (Members.TryGetValue(name,out var member)) {
                switch (member) {
                    case MethodData last:
                        Members[name] = new List<MethodData>(2) {last, function};
                        break;
                    case List<MethodData> list:
                        list.Add(function);
                        break;
                    default:
                        throw new Exception(member.ToString());
                }
            }
            else Members[name] = function;
        }
#if !AOT
        static IntPtr GetFunctionPtr(Delegate @delegate) => @delegate.Method.MethodHandle.GetFunctionPointer();
#else
        static IntPtr ToPtr(delegate*<Variable,void> action) =>(IntPtr)action;
        static IntPtr ToPtr(delegate*<Variable,Variable,void> action) =>(IntPtr)action;
        static IntPtr ToPtr(delegate*<Variable,Variable,Variable,void>action) =>(IntPtr)action;
        static IntPtr ToPtr(delegate*<Variable,Variable,Variable,Variable,void> action) =>(IntPtr)action;
        static IntPtr ToPtr(delegate*<Variable,Variable,Variable,Variable,Variable,void> action) =>(IntPtr)action;
        static IntPtr ToPtr(delegate*<Variable,Variable,Variable,Variable,Variable,Variable,void> action) =>(IntPtr)action;
#endif 
        public void Register(string name,MethodData function) {
            if(function==null)return;
            if (Members.TryGetValue(name,out var member)) {
                switch (member) {
                    case MethodData last:
                        Members[name] = new List<MethodData>(2) {last, function};
                        break;
                    case List<MethodData> list:
                        list.Add(function);
                        break;
                    default:
                        throw new Exception(member.ToString());
                }
            }
            else Members[name] = function;
        }

        public void RegisterConst(string name,Variable variable ) {
            Members[name] = variable;
        }
       ConstructorInfo GetConstructorInfo(Type[] types) => DeclaringType.GetConstructor(types);
#if !AOT
       public void RegisterConstructor(Action<Variable> action) {
           Register(new MethodData(GetConstructorInfo(Array.Empty<Type>()),GetFunctionPtr(action)));
       }
       public void RegisterConstructor(Type type,Action<Variable,Variable>  action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(type)),GetFunctionPtr(action)));
       }
       public void RegisterConstructor(Type arg1,Type arg2,Action<Variable,Variable,Variable> action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(arg1,arg2)),GetFunctionPtr(action)));
       }
       public void RegisterConstructor(Type arg1,Type arg2,Type arg3,Action<Variable,Variable,Variable,Variable> action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(arg1,arg2,arg3)),GetFunctionPtr(action)));
       }
       public void RegisterConstructor(Type arg1,Type arg2,Type arg3,Type arg4,Action<Variable,Variable,Variable,Variable,Variable> action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(arg1,arg2,arg3,arg4)),GetFunctionPtr(action)));
       }
       
       public void RegisterFieldSetter(string name,Action<Variable,Variable>  action) {
           Register(name, MethodData.New(DeclaringType.GetField(name),FieldType.InstancedSetter,GetFunctionPtr(action)));
       }
       public void RegisterFieldGetter(string name,Action<Variable,Variable>  action) {
           Register(name, MethodData.New(DeclaringType.GetField(name),FieldType.InstancedGetter,GetFunctionPtr(action)));
       }
       public void RegisterFieldSetter(string name,Action<Variable>  action) {
           Register(name,  MethodData.New(DeclaringType.GetField(name),FieldType.StaticSetter,GetFunctionPtr(action)));
       }
       public void RegisterFieldGetter(string name,Action<Variable>  action) {
           Register(name, MethodData.New(DeclaringType.GetField(name),FieldType.StaticGetter,GetFunctionPtr(action)));
       }
       public void RegisterPropertySetter(string name,Action<Variable,Variable>  action) {
           Register(name, new MethodData(GetMethodInfo("set_"+name),GetFunctionPtr(action)));
       }
       
       public void RegisterPropertyGetter(string name,Action<Variable,Variable>  action) {
           Register(name,new MethodData(GetMethodInfo("get_"+name),GetFunctionPtr(action)));
       }
       public void RegisterPropertySetter(string name,Action<Variable>  action) {
           Register(name,new MethodData(GetMethodInfo("set_"+name),GetFunctionPtr(action)));
       }

       public void RegisterPropertyGetter(string name, Action<Variable> action) {
           Register(name, new MethodData(GetMethodInfo("get_" + name), GetFunctionPtr(action)));
       }
#else
        public void RegisterConstructor(delegate*<Variable,void> action) {
           Register(new MethodData(GetConstructorInfo(Array.Empty<Type>()),ToPtr(action)));
       }
       public void RegisterConstructor(Type type,delegate*<Variable,Variable,void>  action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(type)),ToPtr(action)));
       }
       public void RegisterConstructor(Type arg1,Type arg2,delegate*<Variable,Variable,Variable,void> action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(arg1,arg2)),ToPtr(action)));
       }
       public void RegisterConstructor(Type arg1,Type arg2,Type arg3,delegate*<Variable,Variable,Variable,Variable,void> action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(arg1,arg2,arg3)),ToPtr(action)));
       }
       public void RegisterConstructor(Type arg1,Type arg2,Type arg3,Type arg4,delegate*<Variable,Variable,Variable,Variable,Variable,void> action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(arg1,arg2,arg3,arg4)),ToPtr(action)));
       }
       
       public void RegisterFieldSetter(string name,delegate*<Variable,Variable,void>  action) {
           Register(name, MethodData.New(DeclaringType.GetField(name),FieldType.InstancedSetter,ToPtr(action)));
       }
       public void RegisterFieldGetter(string name,delegate*<Variable,Variable,void>  action) {
           Register(name, MethodData.New(DeclaringType.GetField(name),FieldType.InstancedGetter,ToPtr(action)));
       }
       public void RegisterFieldSetter(string name,delegate*<Variable,void>  action) {
           Register(name,  MethodData.New(DeclaringType.GetField(name),FieldType.StaticSetter,ToPtr(action)));
       }
       public void RegisterFieldGetter(string name,delegate*<Variable,void>  action) {
           Register(name, MethodData.New(DeclaringType.GetField(name),FieldType.StaticGetter,ToPtr(action)));
       }
       public void RegisterPropertySetter(string name,delegate*<Variable,Variable,void>  action) {
           Register(name, new MethodData(GetMethodInfo("set_"+name),ToPtr(action)));
       }
       
       public void RegisterPropertyGetter(string name,delegate*<Variable,Variable,void>  action) {
           Register(name,new MethodData(GetMethodInfo("get_"+name),ToPtr(action)));
       }
       public void RegisterPropertySetter(string name,delegate*<Variable,void>  action) {
           Register(name,new MethodData(GetMethodInfo("set_"+name),ToPtr(action)));
       }

       public void RegisterPropertyGetter(string name, delegate*<Variable,void> action) {
           Register(name, new MethodData(GetMethodInfo("get_" + name), ToPtr(action)));
       }

#endif
       
       MethodInfo GetMethodInfo(string name,Type[] types=null) {
           if(_methodsDictionary.TryGetValue(name,out var o))
           {
               if (o is MethodInfo methodInfo) {
                   return methodInfo;
               }
               if (types == null) return null;
               var length = types.Length;
               var list = (List<(MethodInfo, ParameterInfo[] parameterInfos)>) o;
               for (var index = 0; index < list.Count; index++) {
                   var pair = list[index];
                   var parameterInfos = pair.parameterInfos;
                   if (parameterInfos.Length != length) continue;
                   for (int i = 0; i < length; i++) {
                       if (types[i] != parameterInfos[i].ParameterType) goto next;
                   }
                   list.RemoveAt(index);
                   return pair.Item1;
                   next: ;
               }

               Debug.Log("Cannot Find Valid"+name+types.ToString());
           }
           else Debug.Log("Cannot Find "+name);
           
           return DeclaringType.GetMethod(name,types??Array.Empty<Type>());
       }
#if !AOT
        public void RegisterMethod0(string name,Action action) {
            Register(new MethodData(DeclaringType,MethodType.Static,name,null,null,GetFunctionPtr(action)));
        }
        public void RegisterMethod1(string name,Action<Variable> action) {
            Register(new MethodData(GetMethodInfo(name),GetFunctionPtr(action)));
        }
        public void RegisterMethod2(string name,Action<Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name),GetFunctionPtr(action)));
        }
        public void RegisterMethod3(string name,Action<Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name),GetFunctionPtr(action)));
        }
        public void RegisterMethod4(string name,Action<Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name),GetFunctionPtr(action)));
        }
        public void RegisterMethod5(string name,Action<Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name),GetFunctionPtr(action)));
        }
       
        
        public void RegisterMethod0(string name,Type[] types,Action action) {
            Register(new MethodData(GetMethodInfo(name,types),GetFunctionPtr(action)));
        }
        public void RegisterMethod1(string name,Type[] types,Action<Variable> action) {
            Register(new MethodData(GetMethodInfo(name,types),GetFunctionPtr(action)));
        }
        public void RegisterMethod2(string name,Type[] type,Action<Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name,type),GetFunctionPtr(action)));
        }
        
        public void RegisterMethod3(string name,Type[] type,Action<Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name,type),GetFunctionPtr(action)));
        }
        public void RegisterMethod4(string name,Type[] type,Action<Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name,type),GetFunctionPtr(action)));
        }
        public void RegisterMethod5(string name,Type[] type,Action<Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name,type),GetFunctionPtr(action)));
        }
        public void RegisterMethod6(string name,Type[] type,Action<Variable,Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name,type),GetFunctionPtr(action)));
        }
        
        public void RegisterMethod(MethodInfo methodInfo) {
            Register(new MethodData(methodInfo));
        }
        public void RegisterMethod0(MethodInfo methodInfo,Action action) {
            Register(new MethodData(methodInfo,GetFunctionPtr(action)));
        }
        public void RegisterMethod1(MethodInfo methodInfo,Action<Variable> action) {
            Register(new MethodData(methodInfo,GetFunctionPtr(action)));
        }
        public void RegisterMethod2(MethodInfo methodInfo,Action<Variable,Variable> action) {
            Register(new MethodData(methodInfo,GetFunctionPtr(action)));
        }
        public void RegisterMethod3(MethodInfo methodInfo,Action<Variable,Variable,Variable> action) {
            Register(new MethodData(methodInfo,GetFunctionPtr(action)));
        }
        public void RegisterMethod4(MethodInfo methodInfo,Action<Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(methodInfo,GetFunctionPtr(action)));
        }
        public void RegisterMethod5(MethodInfo methodInfo,Action<Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(methodInfo,GetFunctionPtr(action)));
        }
        public void RegisterMethod6(MethodInfo methodInfo,Action<Variable,Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(methodInfo,GetFunctionPtr(action)));
        }
        public void RegisterIncrement(string paramName,Action<Variable,Variable> prefix,Action<Variable,Variable> postFix) {
            var (prefixData, postFixData) =
                MethodData.NewRefUnary(DeclaringType, paramName,true,GetFunctionPtr(prefix) , GetFunctionPtr(postFix));
            Register( prefixData);
            Register( postFixData);
        }
        public void RegisterDecrement(string paramName,Action<Variable,Variable> prefix,Action<Variable,Variable> postFix) {
            var (prefixData, postFixData) =
                MethodData.NewRefUnary(DeclaringType, paramName,false, GetFunctionPtr(prefix), GetFunctionPtr(postFix));
            Register( prefixData);
            Register( postFixData);
        }
#else
    public void RegisterMethod0(string name,delegate*<void> action) {
            Register(new MethodData(DeclaringType,MethodType.Static,name,null,null,action));
        }
        public void RegisterMethod1(string name,delegate*<Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name),ToPtr(action)));
        }
     
        public void RegisterMethod2(string name,delegate*<Variable,Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name),ToPtr(action)));
        }
       
        public void RegisterMethod3(string name,delegate*<Variable,Variable,Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name),ToPtr(action)));
        }
       
        public void RegisterMethod4(string name,delegate*<Variable,Variable,Variable,Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name),ToPtr(action)));
        }
        public void RegisterMethod5(string name,delegate*<Variable,Variable,Variable,Variable,Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name),ToPtr(action)));
        }
       
        
        public void RegisterMethod0(string name,Type[] types,delegate*<void>  action) {
            Register(new MethodData(GetMethodInfo(name,types),action));
        }
        public void RegisterMethod1(string name,Type[] types,delegate*<Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name,types),ToPtr(action)));
        }
        public void RegisterMethod2(string name,Type[] type,delegate*<Variable,Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name,type),ToPtr(action)));
        }
        
        public void RegisterMethod3(string name,Type[] type,delegate*<Variable,Variable,Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name,type),ToPtr(action)));
        }
        public void RegisterMethod4(string name,Type[] type,delegate*<Variable,Variable,Variable,Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name,type),ToPtr(action)));
        }
        public void RegisterMethod5(string name,Type[] type,delegate*<Variable,Variable,Variable,Variable,Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name,type),ToPtr(action)));
        }
        public void RegisterMethod6(string name,Type[] type,delegate*<Variable,Variable,Variable,Variable,Variable,Variable,void> action) {
            Register(new MethodData(GetMethodInfo(name,type),ToPtr(action)));
        }
        
        public void RegisterMethod(MethodInfo methodInfo) {
            Register(new MethodData(methodInfo));
        }
        // public void RegisterIncrement(string paramName,delegate*<Variable,Variable,void> prefix,delegate*<Variable,Variable,void> postFix) {
        //     var (prefixData, postFixData) =
        //         MethodData.NewRefUnary(DeclaringType, paramName,true,GetFunctionPtr(prefix) , GetFunctionPtr(postFix));
        //     Register( prefixData);
        //     Register( postFixData);
        // }
        // public void RegisterDecrement(string paramName,Action<Variable,Variable> prefix,Action<Variable,Variable> postFix) {
        //     var (prefixData, postFixData) =
        //         MethodData.NewRefUnary(DeclaringType, paramName,false, GetFunctionPtr(prefix), GetFunctionPtr(postFix));
        //     Register( prefixData);
        //     Register( postFixData);
        // }
#endif
       
        
        
        public void RegisterMethod(MethodData description) {
            Register(description);
        }
        
        
        
       
        
        
        
         public object FindFunctions(ReadOnlySpan<char> name) {
             if (Members.TryGetValue(name, out var o)) {
                return o;
             }
             return BaseModule?.FindFunctions(name);
             
         }
        public bool TryGetMember(ReadOnlySpan<char> name,out object member) {
             if (Members.TryGetValue(name, out  member)) {
                return true;
             }
             if (BaseModule != null) {
                 return BaseModule.TryGetMember(name,out member);
             }
             member = null;
             return false;
             
         }  
        public object GetMember(ReadOnlySpan<char> name) {
             if (Members.TryGetValue(name, out var  member)) {
                return member;
             }
             if (BaseModule != null) {
                 if (BaseModule.TryGetMember(name, out member)) {
                     return member;
                 }
             }
             return null;
             
         }
        public object GetMember(string name) {
             if (Members.TryGetValue(name, out var  member)) {
                return member;
             }
             return BaseModule?.GetMember(name);
        } 
        public bool TryGetSetter(ReadOnlySpan<char> name,out MethodData setter) {
             if (Members.TryGetValue(name, out var  member)) {
                 if (member is MethodData functionData) {
                     setter = functionData;return true;
                 }
                 if (member is List<MethodData> list) {
                     foreach (var function in list) {
                         if (!function.HasReturnValue) {
                             setter = function; return true;
                         }
                     }
                 }
                 setter = null;
                return false;
             }
             if (!IsStatic&&BaseModule!=null&&BaseModule != VariableModule.Instance) {
                 return BaseModule.TryGetSetter(name,out setter);
             }
             setter = null;
             return false;
        }
        public bool TryGetGetter(ReadOnlySpan<char> name,out MethodData getter) {
             if (Members.TryGetValue(name, out var  member)) {
                 if (member is MethodData functionData) {
                     getter = functionData;return true;
                 }
                 if (member is List<MethodData> list) {
                     foreach (var function in list) {
                         if (function.HasReturnValue) {
                             getter = function; return true;
                         }
                     }
                 }
                 getter = null;
                return false;
             }
             if (!IsStatic&&BaseModule!=null&&BaseModule != VariableModule.Instance) {
                 return BaseModule.TryGetSetter(name,out getter);
             }
             getter = null;
             return false;
        }public bool TryGetGetterAndSetter(ReadOnlySpan<char> name,out MethodData getter,out MethodData setter) {
            getter = null;
            setter = null;
             if (Members.TryGetValue(name, out var  member)) {
                 if (member is List<MethodData> list) {
                     foreach (var function in list) {
                         if (!function.HasReturnValue) {
                             setter = function; 
                             if ( getter != null) {
                                 return true;
                             }
                         }
                         else {
                             getter = function;
                             if ( setter != null) {
                                 return true;
                             }
                         }
                     }
                 }
                 return false;
             }
             if (!IsStatic&&BaseModule!=null&&BaseModule != VariableModule.Instance) {
                 return BaseModule.TryGetGetterAndSetter(name,out getter,out setter);
             }
             return false;
        }
      
        public bool TryGetMember(int hashCode,ReadOnlySpan<char> name,out object member) {
             if (Members.TryGetValue(hashCode,name, out  member)) {
                return true;
             }

             if (BaseModule != null) {
                 return BaseModule.TryGetMember(hashCode,name,out member);
             }

             member = null;
             return false;
             
         }
        public bool TryGetMember(int hashCode,string name,out object member) {
             if (Members.TryGetValue(hashCode,name, out  member)) {
                return true;
             }

             if (BaseModule != null) {
                 return BaseModule.TryGetMember(hashCode,name,out member);
             }

             member = null;
             return false;
             
         }


        public override string ToString() {
            return ModuleName;
        }
    }
}