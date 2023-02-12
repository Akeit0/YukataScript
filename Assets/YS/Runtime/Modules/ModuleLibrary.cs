using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;
using YS.Collections;
using YS.Instructions;
using YS.Lexer;
using YS.VM;
using YS.Text;
using static YS.Collections.CollectionsUtility;
namespace YS.Modules {
    public static class ModuleLibrary {
#if UNITY_EDITOR
        public static StringDictionary<Variable> NonGenericVariables=new StringDictionary<Variable>(16);
        public static void RegisterNonGeneric(Variable variable,bool checkNonGeneric=false) {
            if (checkNonGeneric && variable.GetType().IsConstructedGenericType) return;
            NonGenericVariables[ variable.type.Build()] = variable;
        }
#endif
        public static void ToJson(Variable variable, SimpleList<char> builder) {
            builder.AddRange("{\"type\":\"");
            var type = variable.type;
            builder.AddRange(type.Build());
            builder.AddRange("\",");
            var json = JsonUtility.ToJson(variable);
            builder.AddRange(json.AsSpan(1));
        }
        public static string ToJson(Variable variable,out StringBuilder builder) {
             builder = new StringBuilder(16);
            builder.Append("{\"type\":\"");
            var type = variable.type;
            builder.Append(type.Build());
            builder.Append("\",");
            var json = JsonUtility.ToJson(variable);
            builder.Append(json.AsSpan(1));
            var result = builder.ToString();
            builder.Clear();
            return result;
        }
        public static void ToJson(Variable variable, StringBuilder builder) {
            builder.Append("{\"type\":\"");
            var type = variable.type;
            builder.Append(type.Build());
            builder.Append("\",");
            var json = JsonUtility.ToJson(variable);
            builder.Append(json.AsSpan(1));
        }
       
        public static Variable FromJson(ReadOnlySpan<char> json,out int readCount) {
            var start = 0;
            var quoteCount = 0;
            while (quoteCount < 3)
                if (json[start++] == '"')
                    ++quoteCount;
            var end = start;
            while (json[++end] != '"') { }
            var type = json[start.. end];
            start = end + 6;
            while (json[++start] != ':') {
            }
            ++start;
            while (json[start] == ' ') {
                ++start;
            }
            var parenCount =json[start]=='{'? 2:1;
            end = start+1;
            bool isNull=true;
            while (0 < parenCount) {
                var c = json[end++];
                if (c == '}') --parenCount;
                else if (c == '{') ++parenCount;
                else if (isNull && c != ' ') isNull = false;
            }
            readCount = end;
#if UNITY_EDITOR
            var variable=NonGenericVariables[type].Clone();
#else
            var variable=Variable.New(_types[type]);
#endif
            var span = json[start.. end];
            if(isNull)return variable;
            string jsonValue = "{\"value\":".Concat(span);
            JsonUtility.FromJsonOverwrite(jsonValue, variable);
            return variable;
        }

        
        static ModuleLibrary() {
            GlobalModule.Activate();
        }
        public static object Lock = new object();
        static readonly HashSet<Assembly> _assemblies = new HashSet<Assembly>();

        public static object[] TypeList;

        public static readonly TypeKeyAddOnlyDictionary< Lazy<TypeModule>> ModuleDictionary = new (16);
        public static readonly NameSpaceTreeRoot Root=new NameSpaceTreeRoot(); 
        
        static StringDictionary< Type> _types = new StringDictionary< Type>(32) {
            {"char", typeof(char)},
            {"uint", typeof(uint)},
            {"long", typeof(long)},
            {"ulong", typeof(ulong)},
            {"object", typeof(object)},
            {"bool", typeof(bool)},
        };
       
        
       
      
        public static void Register<T>(Func<TypeModule>  module) {
             var type = typeof(T);
             
             var name = type.BuildFullName();
             if (type.IsConstructedGenericType) {
                 name = name.Replace('<', '_').Replace('>', '_').Replace(',', '_');
             }
             if(ModuleDictionary.TryAdd(type,new Lazy<TypeModule>(module))) {
                _types[name]= type;
                Root.Add(name,type);
             }
        }
        public static void Register(Type type,Func<TypeModule>  module) {
            var assembly = type.Assembly;
            _assemblies.Add(assembly);
            var name = type.BuildFullName();
             if (type.IsConstructedGenericType) {
                 name = name.Replace('<', '_').Replace('>', '_').Replace(',', '_');
             }
             if(ModuleDictionary.TryAdd(type,new Lazy<TypeModule>(module))) {
                _types[name]= type;
                Root.Add(name,type);
             }
        }
        
        public static void Register(Variable variable,Func<TypeModule>  module) {
            var type = variable.type;
            var assembly = type.Assembly;
            _assemblies.Add(assembly);
            var lazyModule = new Lazy<TypeModule>(module);
            variable.Cache = lazyModule;
            var name = type.BuildFullName();
             if (type.IsConstructedGenericType) {
                 name = name.Replace('<', '_').Replace('>', '_').Replace(',', '_');
             }
             if(ModuleDictionary.TryAdd(type,lazyModule)) {
                _types[name]= type;
                Root.Add(name,type);
             }
        }
        

      
        public static bool TryGetModule(Type type, out TypeModule module) {
            if (ModuleDictionary.TryGetValue(type, out var lazyModule)) {
                module = lazyModule.Value;
                return true;
            }
            if (type != typeof(object)) {
                return TryGetModule(type.BaseType, out module);
            }
            module = null;
            return false;
        }
        public static bool TryGetModule(Variable variable, out TypeModule module) {
             module = (variable.Cache as Lazy<TypeModule>)?.Value;
             if (module is not null) {
                 return true;
             }
             if (variable.type != typeof(object)) {
                return TryGetModule(variable.type.BaseType, out module);
             }
             module = null;
             return false;
        }
        public static TypeModule GetModule(Type type) {
            if (ModuleDictionary.TryGetValue(type, out var lazyModule)) {
                return lazyModule.Value;
            }

            throw new Exception();
        }

        
        
    
        
       
        public static object FindFunctions(Type type,string methodName) {
            if (!TryGetModule(type,out var module)) {
                var baseType = type.BaseType;
                if (baseType is not null&baseType != typeof(object)) return FindFunctions(baseType, methodName);
                return VariableModule.Instance.FindFunctions(methodName);
            }
            return module.FindFunctions(methodName);
        }
        public static object FindFunctions(Variable variable,string methodName) {
            if (!TryGetModule(variable,out var module)) {
               
                return VariableModule.Instance.FindFunctions(methodName);
            }
            return module.FindFunctions(methodName);
        }
        public static bool TryGetSetter(Type type,ReadOnlySpan<char> methodName,out MethodData functionData) {
            if (!TryGetModule(type,out var module)) {
                functionData = null;
                return false;
            }
            
            return module.TryGetSetter(methodName,out functionData);
        }
        public static bool TryGetSetter(Variable variable,ReadOnlySpan<char> methodName,out MethodData functionData) {
            if (!TryGetModule(variable,out var module)) {
                functionData = null;
                return false;
            }
            
            return module.TryGetSetter(methodName,out functionData);
        }
        public static bool TryGetGetter(Type type,ReadOnlySpan<char> methodName,out MethodData functionData) {
            if (!TryGetModule(type,out var module)) {
                functionData = null;
                return false;
            }
            
            return module.TryGetGetter(methodName,out functionData);
        }
         public static bool TryGetGetter(Variable variable,ReadOnlySpan<char> methodName,out MethodData functionData) {
             if (!TryGetModule(variable,out var module)) {
                functionData = null;
                return false;
            }
            
            return module.TryGetGetter(methodName,out functionData);
        }
        
        public static bool TryGetGetterAndSetter(Type type,ReadOnlySpan<char> methodName,out MethodData getter,out MethodData setter) {
            if (!TryGetModule(type,out var module)) {
                getter = null;
                setter = null;
                return false;
            }
            return module.TryGetGetterAndSetter(methodName,out getter,out setter);
        }
        public static bool TryGetGetterAndSetter(Variable variable,ReadOnlySpan<char> methodName,out MethodData getter,out MethodData setter) {
         var module = (variable.Cache as Lazy<TypeModule>)?.Value;
         if (module is null) {
             getter = null;
             setter = null;
             return false;
         }
         return module.TryGetGetterAndSetter(methodName,out getter,out setter);
        }

        
        public static object FindFunctions(Type type,ReadOnlySpan<char> methodName) {
            if (!TryGetModule(type,out var module)) {
                
                return VariableModule.Instance.FindFunctions(methodName);
            }
            
            return module.FindFunctions(methodName);
        }
        public static bool TryGetMember(Type type,ReadOnlySpan<char> methodName,out object member) {
            if (!TryGetModule(type,out var module)) {
                
                return VariableModule.Instance.TryGetMember(methodName,out member);
            }
            
            return module.TryGetMember(methodName,out member);
        }
        public static bool TryGetMember(Variable variable,ReadOnlySpan<char> methodName,out object member) {
            var module = (variable.Cache as Lazy<TypeModule>)?.Value;
            if (module is not null) {
                return module.TryGetMember(methodName, out   member);
            }
            else {
                return VariableModule.Instance.TryGetMember(methodName,out member);
            }
            
          
        }
        
         public static bool TryGetMember(Type type,string methodName,out object member,bool searchFromVariableModule) {
            if (!TryGetModule(type,out var module)) {
                if (!searchFromVariableModule) {
                    member = null;
                    return false;
                }
                return VariableModule.Instance.TryGetMember(methodName,out member);
            }
            
            return module.TryGetMember(methodName,out member);
        }
        public static bool TryGetMember(Type type,int hash,string methodName,out object member,bool searchFromVariableModule) {
            if (!TryGetModule(type,out var module)) {
                if (!searchFromVariableModule) {
                    member = null;
                    return false;
                }
                return VariableModule.Instance.TryGetMember(methodName,out member);
            }
            
            return module.TryGetMember(hash,methodName,out member);
        }
        
        public static MethodData FindBinaryOperator(TokenType tokenType,Variable left,Variable right) {
            var op = tokenType.ToBinaryOpType();
            var (hash,name) = Util.BinaryOperatorHashAndName[(ushort)op];
            var module = (left.Cache as Lazy<TypeModule>)?.Value;
            if (module is not null) {
                module.TryGetMember(hash, name, out var  leftResult);
                return  (MethodData) leftResult;
            }
            if (left.type == right.type) {
                return null;
            }
            module = (right.Cache as Lazy<TypeModule>)?.Value;
            if (module is not null) {
                module.TryGetMember(hash, name, out var rightResult);
                return  (MethodData) rightResult;
            }
            return null;
        }
        
        public static (object,object) FindBinaryOperator(BinaryOpType op,Variable left,Variable right) {
            var (hash,name) = Util.BinaryOperatorHashAndName[(ushort)op];
            object leftResult = null;
            object rightResult = null;
            var module = (left.Cache as Lazy<TypeModule>)?.Value;
            if (module is not null) {
                module.TryGetMember(hash, name, out leftResult);
            }
           
            if (left.type == right.type) {
                return (leftResult, null);
            }
            module = (right.Cache as Lazy<TypeModule>)?.Value;
            if (module is not null) {
                module.TryGetMember(hash, name, out rightResult);
            }
            
            return (leftResult, rightResult);

        }
        
        
        
        
        
    }
}