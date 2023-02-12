using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using YS.Collections;
using YS.Modules;
using YS.VM;
namespace YS {
    
    public sealed class TypeModule {
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
       public void RegisterConstructor(Action<Variable> action) {
           Register(new MethodData(GetConstructorInfo(Array.Empty<Type>()),action));
       }
       public void RegisterConstructor(Type type,Action<Variable,Variable>  action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(type)),action));
       }
       public void RegisterConstructor(Type arg1,Type arg2,Action<Variable,Variable,Variable> action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(arg1,arg2)),action));
       }
       public void RegisterConstructor(Type arg1,Type arg2,Type arg3,Action<Variable,Variable,Variable,Variable> action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(arg1,arg2,arg3)),action));
       }
       public void RegisterConstructor(Type arg1,Type arg2,Type arg3,Type arg4,Action<Variable,Variable,Variable,Variable,Variable> action) {
           Register(new MethodData(GetConstructorInfo(Util.Types(arg1,arg2,arg3,arg4)),action));
       }
       
       public void RegisterFieldSetter(string name,Action<Variable,Variable>  action) {
           Register(name, MethodData.New(DeclaringType.GetField(name),FieldType.InstancedSetter,action));
       }
       public void RegisterFieldGetter(string name,Action<Variable,Variable>  action) {
           Register(name, MethodData.New(DeclaringType.GetField(name),FieldType.InstancedGetter,action));
       }
       public void RegisterFieldSetter(string name,Action<Variable>  action) {
           Register(name,  MethodData.New(DeclaringType.GetField(name),FieldType.StaticSetter,action));
       }
       public void RegisterFieldGetter(string name,Action<Variable>  action) {
           Register(name, MethodData.New(DeclaringType.GetField(name),FieldType.StaticGetter,action));
       }
       public void RegisterPropertySetter(string name,Action<Variable,Variable>  action) {
           Register(name, new MethodData(GetMethodInfo("set_"+name),action));
       }
       
       public void RegisterPropertyGetter(string name,Action<Variable,Variable>  action) {
           Register(name,new MethodData(GetMethodInfo("get_"+name),action));
       }
       public void RegisterPropertySetter(string name,Action<Variable>  action) {
           Register(name,new MethodData(GetMethodInfo("set_"+name),action));
       }
       public void RegisterPropertyGetter(string name,Action<Variable>  action) {
           Register(name,new MethodData(GetMethodInfo("get_"+name),action));
       }
       
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
       
        public void RegisterMethod(string name,Action action) {
            Register(new MethodData(DeclaringType,MethodType.Static,name,null,null,action));
        }
        public void RegisterMethod(string name,Action<Variable> action) {
            Register(new MethodData(GetMethodInfo(name),action));
        }
        public void RegisterMethod(string name,Action<Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name),action));
        }
        public void RegisterMethod(string name,Action<Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name),action));
        }
        public void RegisterMethod(string name,Action<Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name),action));
        }
        public void RegisterMethod(string name,Action<Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name),action));
        }
        public void RegisterMethod(string name,Action<Variable,Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name),action));
        }
        
        public void RegisterMethod(string name,Type[] types,Action action) {
            Register(new MethodData(GetMethodInfo(name,types),action));
        }
        public void RegisterMethod(string name,Type[] types,Action<Variable> action) {
            Register(new MethodData(GetMethodInfo(name,types),action));
        }
        public void RegisterMethod(string name,Type[] type,Action<Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name,type),action));
        }
        
        public void RegisterMethod(string name,Type[] type,Action<Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name,type),action));
        }
        public void RegisterMethod(string name,Type[] type,Action<Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name,type),action));
        }
        public void RegisterMethod(string name,Type[] type,Action<Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name,type),action));
        }
        public void RegisterMethod(string name,Type[] type,Action<Variable,Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(GetMethodInfo(name,type),action));
        }
        
        public void RegisterMethod(MethodInfo methodInfo) {
            Register(new MethodData(methodInfo));
        }
        public void RegisterMethod(MethodInfo methodInfo,Action action) {
            Register(new MethodData(methodInfo,action));
        }
        public void RegisterMethod(MethodInfo methodInfo,Action<Variable> action) {
            Register(new MethodData(methodInfo,action));
        }
        public void RegisterMethod(MethodInfo methodInfo,Action<Variable,Variable> action) {
            Register(new MethodData(methodInfo,action));
        }
        public void RegisterMethod(MethodInfo methodInfo,Action<Variable,Variable,Variable> action) {
            Register(new MethodData(methodInfo,action));
        }
        public void RegisterMethod(MethodInfo methodInfo,Action<Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(methodInfo,action));
        }
        public void RegisterMethod(MethodInfo methodInfo,Action<Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(methodInfo,action));
        }
        public void RegisterMethod(MethodInfo methodInfo,Action<Variable,Variable,Variable,Variable,Variable,Variable> action) {
            Register(new MethodData(methodInfo,action));
        }
        // public void RegisterArithmeticMethod(string name,Type returnType,Action<Variable,Variable,Variable> action) {
        //     Register(new MethodData(new MethodData(DeclaringType,MethodType.Static,name,returnType,ArithmeticBinaryParametersCache),action));
        // }
        // public void RegisterComparisonMethod(string name,Action<Variable,Variable,Variable> action) {
        //     Register(new MethodData(new MethodData(DeclaringType,MethodType.Static,name,typeof(bool),ArithmeticBinaryParametersCache),action));
        // }
        // public void RegisterArithmeticMethod(string name,Type returnType,Action<Variable,Variable> action) {
        //     Register(new MethodData(new MethodData(DeclaringType,MethodType.Static,name,returnType,ArithmeticUnaryParametersCache),action));
        // }
        // public void RegisterArithmeticMethod(string name,Action<Variable,Variable,Variable> action) {
        //     Register(new MethodData(new MethodData(DeclaringType,MethodType.Static,name,DeclaringType,ArithmeticBinaryParametersCache),action));
        // }
        // public void RegisterArithmeticMethod(string name,Action<Variable,Variable> action) {
        //     Register(new MethodData(new MethodData(DeclaringType,MethodType.Static,name,DeclaringType,ArithmeticUnaryParametersCache),action));
        // }

        private ParamDescription[] ArithmeticBinaryParametersCache=>_arithmeticBinaryParametersCache??=new ParamDescription[]{new ("a",DeclaringType),new ("b",DeclaringType)};
        private ParamDescription[] _arithmeticBinaryParametersCache;
         private ParamDescription[] ArithmeticUnaryParametersCache=>_arithmeticUnaryParametersCache??=new ParamDescription[]{new ("a",DeclaringType)};
        private ParamDescription[] _arithmeticUnaryParametersCache;
        
        
        
        public void RegisterMethod(MethodData description) {
            Register(description);
        }
        
        
        
        public void RegisterIncrement(string paramName,Action<Variable,Variable> prefix,Action<Variable,Variable> postFix) {
            var (prefixData, postFixData) =
                MethodData.NewRefUnary(DeclaringType, paramName,true, prefix, postFix);
            Register( prefixData);
            Register( postFixData);
        }
        public void RegisterDecrement(string paramName,Action<Variable,Variable> prefix,Action<Variable,Variable> postFix) {
            var (prefixData, postFixData) =
                MethodData.NewRefUnary(DeclaringType, paramName,false, prefix, postFix);
            Register( prefixData);
            Register( postFixData);
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