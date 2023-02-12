using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using UnityEngine;
using YS.Collections;
using YS.Modules;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace YS.Editor.CodeGeneration {
  
    public static class WrapperGenerator {

        
        const string InspectorVariableFileStart =
            @"
namespace YS.Generated {";
        const string InspectorVariableTemplate =
            "    public class ";

        const string InspectorVariableFileEnd =
            @"
}";
        
        const string RegisterFileStart =
            @"using static YS.Modules.ModuleLibrary;
namespace YS.Generated {
    public static partial class Wrapper {
        static void RegisterModules() {";

        const string RegisterFileEnd =
            @"        }
    }
}";
        const string RegisterTemplate = "             Register(typeof("; 
        const string RegisterTemplateForInstantiable = "             Register(new Variable<"; 
        
        const string FileStart =
            @"
namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_";

         const string ClassStart = @"Module(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(";

    

         const string FileEnd = 
             
            @"            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}";

         const string ConstTemplate = "            module.RegisterConst(\"";
         const string ConstTemplateVariable = "\",new Variable<";
         const string MethodTemplate = "            module.RegisterMethod(\"";
         const string ConstructorTemplate = "            module.RegisterConstructor(";
         const string FieldGetterTemplate = "            module.RegisterFieldGetter(\"";
         const string FieldSetterTemplate = "            module.RegisterFieldSetter(\"";
         const string PropertyGetterTemplate = "            module.RegisterPropertyGetter(\"";
         const string PropertySetterTemplate = "            module.RegisterPropertySetter(\"";
         
         
         
         static string[] ActionTemplate = {
            "() => ",
            "input1 => ",
            "(input1, input2) => ",
            "(input1, input2, input3)  =>  ",
            "(input1, input2, input3, input4)  => ",
            "(input1, input2, input3, input4, input5)  => ",
            "(input1, input2, input3, input4, input5, input6)  => ",
            "(input1, input2, input3, input4, input5, input6, input7)  => "
        };
        public static string[] ArgumentValueTemplate = {
            "input1.As<", "input2.As<", "input3.As<", "input4.As<",
            "input5.As<", "input6.As<", "input7.As<", "input8.As<",
        };

        public static string[] FuncTemplate = {
            "result => result.SetValue(",
            "(result, input1) => result.SetValue(",
            "(result, input1, input2) => result.SetValue(",
            "(result, input1, input2, input3) => result.SetValue(",
            "(result, input1, input2, input3, input4) => result.SetValue(",
            "(result, input1, input2, input3, input4, input5) => result.SetValue(",
            "(result, input1, input2, input3, input4, input5, input6) => result.SetValue(",
        };
        static readonly StringBuilder InspectorBuilder=new (100);
        static readonly StringBuilder RegisterBuilder=new (100);
        static readonly StringBuilder AliasBuilder=new (100);
        static readonly StringBuilder ClassBuilder=new (2000);
        static readonly StringBuilder MemberBuilder=new (100);
        static readonly StringBuilder MemberBuilder2=new (100);
        static readonly StringDictionary<string> AliasDict=new (20);
        static readonly Dictionary<Type, string> TypeNameDict = new (10);
        static readonly StringDictionary< SimpleList<MethodInfo>> MethodInfoDict= new (10);
        public static readonly StringHashSet BannedNames = new () {};

        static WrapperGenerator() {
            BannedNames.Add("runInEditMode");
            BannedNames.Add("Chars");
        }
        static void Initialize() {
            AliasBuilder.Clear();
            ClassBuilder.Clear();
            MemberBuilder.Clear();
            MemberBuilder2.Clear();
            AliasDict.Clear();
            TypeNameDict.Clear();
            MethodInfoDict.Clear();
        }

        static string GetName(this Type type) {
            if (TypeNameDict.TryGetValue(type, out var name)){
                return name;
            }
          
            if (type.IsArray) {
                 name = type.GetElementType().GetName()+"[]";
                TypeNameDict.Add(type,name);
                return name;
            }
            if (type.IsConstructedGenericType) {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                    name = Nullable.GetUnderlyingType(type).GetName()+"?";
                    TypeNameDict.Add(type,name);
                    return name;
                }
                var nameSpace = type.Namespace;
                var fullName = type.BuildFullName();
                string alias;
                if (string.IsNullOrEmpty(nameSpace)) {
                    alias = fullName.Split('<')[0];
                }
                else {
                    alias= fullName.Remove(0, Math.Min(fullName.Length,nameSpace.Length + 1)).Split('<')[0];
                }

                if (type.IsNested) {
                    alias = alias.Replace('+', '_');
                    fullName = fullName.Replace('+', '.');
                }
                while (AliasDict.ContainsKey(alias)) {
                    alias += "_";
                }
                AliasDict[alias] = fullName;
                TypeNameDict.Add(type,alias);
                return alias;
            }
            else {
                
                var nameData = type.GetNameFull();
            
                if (nameData.Namespace is null) {
                    TypeNameDict.Add(type,nameData.Name);
                    return nameData.Name;
                }

                var alias = nameData.Name;
                if (type.IsNested) {
                    alias = alias.Replace('+', '_');
                    nameData.FullName = nameData.FullName.Replace('+', '.');
                }
                while (AliasDict.ContainsKey(alias)) {
                    alias += "_";
                }
            
                if (nameData.Namespace == "(global::)") {
                    AliasDict[alias] = nameData.Name;
                }
                else {
                    AliasDict[alias] = nameData.FullName;
                }
                TypeNameDict.Add(type,alias);
                return alias;
            }
            
        }
        
        public static string AssetsToAbsolutePath(this string self)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            return self.Replace("Assets", Application.dataPath).Replace("/", "\\");
#else
        return self.Replace("Assets", Application.dataPath);
#endif
        }
        public static HashSet<Type>BasicAllowedTypes=new (16){typeof(object),typeof(int),typeof(float),typeof(double),typeof(string),typeof(Type)};
        public static HashSet<Type>AllowedTypes=new (16);
        public static HashSet<Type>BannedTypes=new (16);
        public static bool UseOnlyAllowed=false;
        public static void GenerateScript(string assetFolderPath,Type type,bool replace=false) {
            var filePath =assetFolderPath+ "/Wrapper." + type.BuildFullName().Replace('<','_').Replace('>','_').Replace(',','_') + ".cs";
           
            if (File.Exists(filePath.AssetsToAbsolutePath())&&!replace) {
                return;
            }
            CreateFolder(assetFolderPath);
          
            var code = GenerateWrapper(type);
            if(code!=null)
                File.WriteAllText(filePath, code);
        }
        
        static readonly char[] splitChars = new char[]{ Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
        /// <summary>
        /// 指定されたパスのフォルダを生成する
        /// </summary>
        /// <param name="path">フォルダパス（例: Assets/Sample/FolderName）</param>
        static void CreateFolder(string path)
        {
#if UNITY_EDITOR
            var target = "";
           
            foreach (var dir in path.Split(splitChars)) {
                var parent = target;
                target = Path.Combine(target, dir);
                if (!AssetDatabase.IsValidFolder(target)) {
                    AssetDatabase.CreateFolder(parent, dir);
                }
            }
#endif
        }
        
        static HashSet<Type> AllReadyCreated=new HashSet<Type>();
        
        static List<(string name ,CodeGenerationType codeGenerationType,Type type)>typeListCopy=new();
        public static void GenerateScript(string assetFolderPath,List<(string name ,CodeGenerationType codeGenerationType,Type type)> typeList,bool replace=false) {
            RegisterBuilder.AppendLine(RegisterFileStart);
            InspectorBuilder.AppendLine(InspectorVariableFileStart);
            foreach (var pair in typeList) {
                var codeGenerationType = pair.codeGenerationType;
                var type = pair.type;
                if (type is null||type.IsByRefLike|type.IsGenericTypeDefinition|type==typeof(Array)) continue;
                var nextType = type;
                while (true) {
                    if (nextType is null||AllReadyCreated.Contains(nextType)) break;
                    AllReadyCreated.Add(nextType);
                    var fullName = nextType.BuildFullName(true);
                    var filePath =assetFolderPath+ "/Modules/Wrapper." + fullName.Replace('<','_').Replace('>','_').Replace(',','_').Replace("[]","_Array") + ".cs";
                    if (codeGenerationType!=CodeGenerationType.InspectorOnly&& !(File.Exists(filePath.AssetsToAbsolutePath()) && !replace)) {
                        CreateFolder(assetFolderPath);
                        var code = GenerateWrapper(nextType);
                        if(code!=null)
                            File.WriteAllText(filePath, code);
                    }
                    if(nextType.IsAbstract)
                        typeListCopy.Add((fullName,CodeGenerationType.WrapperOnly,nextType));
                    else  typeListCopy.Add((fullName,codeGenerationType,nextType));
                     if(codeGenerationType!=CodeGenerationType.InspectorOnly) {
                         if(nextType.IsAbstract&&nextType.IsSealed) {
                             RegisterBuilder.Append(RegisterTemplate);
                             RegisterBuilder.Append(fullName);
                             RegisterBuilder.Append("),()=>Create_");
                         }
                         else {
                             RegisterBuilder.Append(RegisterTemplateForInstantiable);
                             RegisterBuilder.Append(fullName);
                             RegisterBuilder.Append(">(),()=>Create_");
                         }
                     }
                    var typeName = fullName.Replace('.','_');
                    
                    if (nextType.IsArray) {
                        typeName = typeName.Replace("[]", "_Array");
                    }
                    if (type.IsConstructedGenericType) {
                        typeName = typeName.Replace('<', '_').Replace('>', '_').Replace(',', '_');
                    }
                    if (type.IsNested) {
                        typeName = typeName.Replace('+', '_');
                    }
                    if (codeGenerationType != CodeGenerationType.InspectorOnly) {
                        RegisterBuilder.Append(typeName);
                    }
                    if (codeGenerationType != CodeGenerationType.WrapperOnly&&nextType!=typeof(Enum)) {
                        InspectorBuilder.Append(InspectorVariableTemplate);
                            
                            InspectorBuilder.Append(typeName);
                            InspectorBuilder.Append("Variable");
                            InspectorBuilder.Append(":Variable<");
                            InspectorBuilder.Append(fullName);
                            InspectorBuilder.AppendLine(">{}"); 
                    }

                    if (codeGenerationType == CodeGenerationType.InspectorOnly) {
                        break;
                    }
                    nextType = nextType.BaseType;
                    if ((nextType != typeof(object) && nextType != typeof(ValueType))) {
                        RegisterBuilder.Append("Module(typeof(");
                        RegisterBuilder.Append(nextType.BuildFullName());
                        RegisterBuilder.AppendLine(")));");
                    }
                    else {
                        RegisterBuilder.AppendLine("Module());");
                        break;
                    }
                }
            }
            RegisterBuilder.AppendLine(RegisterFileEnd);
            File.WriteAllText(assetFolderPath+"/Wrapper.RegisterModules.cs", RegisterBuilder.ToString());
            RegisterBuilder.Clear();
            InspectorBuilder.AppendLine(InspectorVariableFileEnd);
            File.WriteAllText(assetFolderPath+"/InspectorVariables.cs", InspectorBuilder.ToString());
            InspectorBuilder.Clear();
            AllReadyCreated.Clear();
            typeList.Clear();
            typeList.AddRange(typeListCopy);
            typeListCopy.Clear();
        }

        public static void GenerateNonGenericVariable(string assetFolderPath, List<(string name, Type type)> typeList) {
            ClassBuilder.Append(InspectorVariableFileStart);

        }
        public static string GenerateWrapper(Type type) {
            if (type is null) return null;
            if (type.IsEnum) {
                try {
                    return GenerateEnum(type);
                }
                finally {
                    Initialize();
                }
               
            }
            if (type.IsGenericTypeDefinition) return "";
            try {
                var typeName = type.BuildFullName().Replace('.','_');
                if (type.IsConstructedGenericType) {
                    typeName = typeName.Replace('<', '_').Replace('>', '_').Replace(',', '_');
                }

                if (type.IsNested) {
                    typeName = typeName.Replace('+', '_');
                }
                ClassBuilder.Append(FileStart);
                ClassBuilder.Append(typeName);
                
                
                
                typeName = GetName(type);
                ClassBuilder.Append(ClassStart);
                ClassBuilder.Append(typeName);
                ClassBuilder.AppendLine("));");
                foreach (var constructorInfo in type.GetConstructors()) {
                    if (!Generate(constructorInfo)) continue;
                    ClassBuilder.Append(MemberBuilder);
                    ClassBuilder.Append(MemberBuilder2);
                    MemberBuilder.Clear();
                    MemberBuilder2.Clear();
                }
                foreach (var fieldInfo in type.GetFields(BindingFlags.DeclaredOnly|BindingFlags.Public | BindingFlags.Static)) {
                    if (!GenerateStaticField(fieldInfo)) continue;
                    ClassBuilder.Append(MemberBuilder);
                    MemberBuilder.Clear();
                }
                foreach (var fieldInfo in type.GetFields(BindingFlags.DeclaredOnly|BindingFlags.Public |BindingFlags.Instance)) {
                    if (!GenerateInstanceField(fieldInfo)) continue;
                    ClassBuilder.Append(MemberBuilder);
                    MemberBuilder.Clear();
                }
                foreach (var propertyInfo in type.GetProperties(BindingFlags.DeclaredOnly|BindingFlags.Public | BindingFlags.Static)) {
                    if (!GenerateStaticProperty(propertyInfo)) continue;
                    ClassBuilder.Append(MemberBuilder);
                    MemberBuilder.Clear();
                }
                
                foreach (var propertyInfo in type.GetProperties(BindingFlags.DeclaredOnly|BindingFlags.Public |BindingFlags.Instance)) {
                    if (!GenerateInstanceProperty(propertyInfo)) continue;
                    ClassBuilder.Append(MemberBuilder);
                    MemberBuilder.Clear();
                }
                
                foreach (var methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly  )) {
                    if (methodInfo.IsGenericMethodDefinition) continue;
                    if (BannedNames.Contains(methodInfo.Name)) continue;
                    if (MethodInfoDict.TryGetValue(methodInfo.Name, out var list)) {
                        list.Add(methodInfo);
                    }
                    else {
                        MethodInfoDict[methodInfo.Name] = new SimpleList<MethodInfo>{methodInfo};
                    }
                }

                foreach (var pair in MethodInfoDict) {
                    var list = pair.Value;
                    if (list.Count == 1) {
                        if (!Generate(list[0])) continue;
                        ClassBuilder.Append(MemberBuilder);
                        MemberBuilder.Clear();
                    }
                    else {
                        foreach (var methodInfo in list.AsSpan()) {
                            if (!GenerateWithType(methodInfo)) continue;
                            ClassBuilder.Append(MemberBuilder);
                            ClassBuilder.Append(MemberBuilder2);
                            MemberBuilder.Clear();
                            MemberBuilder2.Clear();
                        }
                    }
                }
                ClassBuilder.Append(FileEnd);
                foreach (var pair in AliasDict) {
                   
                    AliasBuilder.Append("using ");
                    AliasBuilder.Append(pair.Key);
                    AliasBuilder.Append(" = ");
                    AliasBuilder.Append(pair.Value);
                    AliasBuilder.AppendLine(";");
                }
                return  AliasBuilder.ToString()+ClassBuilder.ToString();
            }
            finally {
                Initialize();
            }
        }


        static string GenerateEnum(Type type) {
           
            var typeName = type.BuildFullName();
            ClassBuilder.Append(FileStart);
            ClassBuilder.Append(typeName.Replace('.','_'));
            typeName = GetName(type);
            ClassBuilder.Append(ClassStart);
            ClassBuilder.Append(typeName);
            ClassBuilder.AppendLine("));");
            MemberInfo[] memberInfos = type.GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var info in memberInfos) {
                var name = info.Name;
                ClassBuilder.Append(ConstTemplate);
                ClassBuilder.Append(name);
                ClassBuilder.Append(ConstTemplateVariable);
                ClassBuilder.Append(typeName);
                ClassBuilder.Append( ">(");
                ClassBuilder.Append(info.DeclaringType.GetName());
                ClassBuilder.Append('.');
                ClassBuilder.Append(name);
                ClassBuilder.AppendLine( "));");
            }
            ClassBuilder.Append(FileEnd);
            foreach (var pair in AliasDict) {
                   
                AliasBuilder.Append("using ");
                AliasBuilder.Append(pair.Key);
                AliasBuilder.Append(" = ");
                AliasBuilder.Append(pair.Value);
                AliasBuilder.AppendLine(";");
            }
            return  AliasBuilder.ToString()+ClassBuilder.ToString();
        }
        static bool Generate(ConstructorInfo info) {
            if (info.IsDefined(typeof(ObsoleteAttribute))) {
                return false;
            }
            var parameterInfos = info.GetParameters();
            var length = parameterInfos.Length;
            if(4<length) return false;
            var builder=MemberBuilder;
            var builder2=MemberBuilder2;
            builder.Append(ConstructorTemplate);
           if(length!=0) builder2.Append(',');
            builder2.Append(FuncTemplate[length] );
            builder2.Append(" new ");
            builder2.Append(info.DeclaringType.GetName());
            builder2.Append('(');
            for (int i = 0; i < length; i++) {
                if (!BuildParamWithTypes(i, parameterInfos[i])) return false;
                if(i != length - 1) builder.Append(',' );
                builder2.Append(i != length - 1 ? ">()," : ">()");
            }
            builder2.AppendLine( ")));");
            return true;
        }
       
        static bool Generate(MethodInfo info) {
            if (info.IsDefined(typeof(ObsoleteAttribute))||info.IsGenericMethodDefinition) {
                return false;
            }
            var returnType = info.ReturnType;
            if (returnType.IsByRefLike) return false;
            if (info.IsSpecialName) {
                return GenerateSpecialName(MemberBuilder,info);
            }
            var parameterInfos = info.GetParameters();
            var length = parameterInfos.Length;
            
            if(5<length) return false;
            
            var builder=MemberBuilder;
            builder.Append(MethodTemplate);
            builder.Append(info.Name);
            builder.Append("\", ");
            if (info.IsStatic) {
                builder.Append(returnType != typeof(void) ? FuncTemplate[length] : ActionTemplate[length]);
                builder.Append(info.DeclaringType.GetName());
                builder.Append('.');
                builder.Append(info.Name);
                builder.Append('(');
                for (int i = 0; i < length; i++) {
                    if (!BuildParam(i, parameterInfos[i])) return false;
                    builder.Append(i != length - 1 ? ">()," : ">()");
                }
                builder.AppendLine(info.ReturnType != typeof(void) ? ")));" : "));");
            }
            else {
                builder.Append(info.ReturnType != typeof(void) ? FuncTemplate[length+1] : ActionTemplate[length+1]);
                builder.Append(ArgumentValueTemplate[0]);
                builder.Append(info.DeclaringType.GetName());
                builder.Append(">().");
                builder.Append(info.Name);
                builder.Append('(');
                for (int i = 0; i < length; i++) {
                    if (!BuildParam(i+1,parameterInfos[i])) return false;
                    
                    builder.Append(i != length - 1 ? ">()," : ">()");
                }
                builder.AppendLine(info.ReturnType != typeof(void) ? ")));" : "));");
            }
            return true;
        }
        static bool GenerateWithType(MethodInfo info) {
            if (info.IsDefined(typeof(ObsoleteAttribute))||info.IsGenericMethodDefinition) {
                return false;
            }
            if (info.IsSpecialName) {
                return GenerateSpecialNameWithTypes(info);
            }
            var parameterInfos = info.GetParameters();
            var length = parameterInfos.Length;
            
            if(5<length) return false;
            var builder=MemberBuilder;
            var builder2=MemberBuilder2;
            builder.Append(MethodTemplate);
            builder.Append(info.Name);
            builder.Append("\", Types(");
            builder2.Append(',');
            if (info.IsStatic) {
                builder2.Append(info.ReturnType != typeof(void) ? FuncTemplate[length] : ActionTemplate[length]);
                builder2.Append(info.DeclaringType.GetName());
                builder2.Append('.');
                builder2.Append(info.Name);
                builder2.Append('(');
                for (int i = 0; i < length; i++) {
                    if (!BuildParamWithTypes(i, parameterInfos[i])) return false;
                    if(i != length - 1) builder.Append(',' );
                    builder2.Append(i != length - 1 ? ">()," : ">()");
                }

                builder.Append(')');
                builder2.AppendLine(info.ReturnType != typeof(void) ? ")));" : "));");
            }
            else {
                builder2.Append(info.ReturnType != typeof(void) ? FuncTemplate[length+1] : ActionTemplate[length+1]);
                builder2.Append(ArgumentValueTemplate[0]);
                builder2.Append(info.DeclaringType.GetName());
                builder2.Append(">().");
                builder2.Append(info.Name);
                builder2.Append('(');
                for (int i = 0; i < length; i++) {
                    if (!BuildParamWithTypes(i+1,parameterInfos[i])) return false;
                    if(i != length - 1) builder.Append(',' );
                    builder2.Append(i != length - 1 ? ">()," : ">()");
                }
                builder.Append(')');
                builder2.AppendLine(info.ReturnType != typeof(void) ? ")));" : "));");
            }
            return true;
        }
        
        
        static bool BuildParam(int inputIndex,ParameterInfo info) {
            var builder=MemberBuilder;
            var type = info.ParameterType;
            if (type.IsByRefLike||type.IsPointer) {
                MemberBuilder.Clear();
                return false;
            }
            if (!type.IsByRef) {
                builder.Append(ArgumentValueTemplate[inputIndex]);
                builder.Append(type.GetName());
                return true;
            }
            type = info.ParameterType.GetElementType();
            if (info.IsIn) {
                builder.Append(ArgumentValueTemplate[inputIndex]);
                builder.Append(type.GetName());
            }
            else if (info.IsOut) {
                builder.Append("out ");
                builder.Append(ArgumentValueTemplate[inputIndex]);
                builder.Append(type.GetName());
            }
            else {
                builder.Append("ref ");
                builder.Append(ArgumentValueTemplate[inputIndex]);
                builder.Append(type.GetName());
            }
            return true;
        }
        static bool BuildParamWithTypes(int inputIndex,ParameterInfo info) {
            var builder=MemberBuilder;
            var builder2=MemberBuilder2;
            var type = info.ParameterType;
            if (type.IsByRefLike||type.IsPointer) {
                builder.Clear();
                builder2.Clear();
                return false;
            }

            string name;
            if (!type.IsByRef) {
                 name = type.GetName();
                builder.Append("typeof(");
                builder.Append(name);
                builder.Append(')');
                builder2.Append(ArgumentValueTemplate[inputIndex]);
                builder2.Append(name);
                return true;
            }
            type = info.ParameterType.GetElementType();
            name = type.GetName();
            builder.Append("typeof(");
            builder.Append(name);
            builder.Append(").MakeByRefType()");
            if (info.IsIn) {
                builder2.Append(ArgumentValueTemplate[inputIndex]);
                builder2.Append(name);
            }
            else if (info.IsOut) {
                builder2.Append("out ");
                builder2.Append(ArgumentValueTemplate[inputIndex]);
                builder2.Append(name);
            }
            else {
                builder2.Append("ref ");
                builder2.Append(ArgumentValueTemplate[inputIndex]);
                builder2.Append(name);
            }
            return true;
        }
        


         static bool GenerateSpecialName(StringBuilder builder, MethodInfo info) {
            var name = info.Name;
            if(TryGetSpecialData(name,out var data)) {
                builder.Append(MethodTemplate);
                builder.Append(name);
                MemberBuilder.Append("\", ");
                switch (data.Item1) {
                    case SpecialInstructionType.Unary:GenerateUnaryInstruction( data, builder, info);
                        break;
                    case SpecialInstructionType.Binary:
                        GenerateBinaryInstruction(data, builder, info);
                        return true;
                    case SpecialInstructionType.IndexGetter:
                      return  GenerateGetIndexer( info);
                     case SpecialInstructionType.IndexSetter:
                      return  GenerateSetIndexer( info);
                  
                }
            }
            return false;
         }
         static bool GenerateSpecialNameWithTypes(MethodInfo info) {
            var name = info.Name;
            var data = GetSpecialData(name);
            if (data.Item1 is not SpecialInstructionType.Binary) {
                MemberBuilder.Clear();
                return false;
            }
            MemberBuilder.Append(MethodTemplate);
            MemberBuilder.Append(name);
            MemberBuilder.Append("\", Types(");
            switch (data.Item1) {
                case SpecialInstructionType.Binary:GenerateBinaryInstructionWithTypes(data, info);
                    break;
                
                
            }

            return true;
        }
        enum SpecialInstructionType {
            Invalid,
            Unary,
            Binary,
            IndexSetter,
            IndexGetter,
        }
        
        static Dictionary<string, (SpecialInstructionType,string)> SpecialNameToInstructionDictionary = new () {
            {"op_Addition", (SpecialInstructionType.Binary,"+")},
            {"op_Subtraction",(SpecialInstructionType.Binary,"-")},
            {"op_Division",(SpecialInstructionType.Binary,"/")},
            {"op_Multiply",(SpecialInstructionType.Binary,"*")},
            {"op_Equality",(SpecialInstructionType.Binary,"==")},
            {"op_Inequality",(SpecialInstructionType.Binary,"!=")},
            {"op_LessThan",(SpecialInstructionType.Binary,"<")},
            {"op_GreaterThan",(SpecialInstructionType.Binary,">")},
            {"LessThanOrEqual",(SpecialInstructionType.Binary,"<=")},
            {"GreaterThanOrEqual",(SpecialInstructionType.Binary,">=")},
            {"op_BitwiseAnd",(SpecialInstructionType.Binary,"&")},
            {"op_BitwiseOr",(SpecialInstructionType.Binary,"|")},
            {"op_LogicalNot",(SpecialInstructionType.Unary,"!")},
            {"op_UnaryNegation",(SpecialInstructionType.Unary,"-")},
            {"op_UnaryPlus",(SpecialInstructionType.Unary,"+")},
            //{"op_Increment",(SpecialInstructionType.Unary,"++")},
            //{"op_Decrement",(SpecialInstructionType.Unary,"--")},
            {"set_Item",(SpecialInstructionType.IndexSetter,null)},
            {"get_Item",(SpecialInstructionType.IndexGetter,null)},
        };

        static (SpecialInstructionType, string) GetSpecialData(string name) {
            if (SpecialNameToInstructionDictionary.TryGetValue(name, out var data)) {
                return data;
            }
            
            return default;
        }static bool TryGetSpecialData(string name,out (SpecialInstructionType, string) data) {
            return (SpecialNameToInstructionDictionary.TryGetValue(name, out data));

        }




        static void GenerateUnaryInstruction((SpecialInstructionType,string)data,StringBuilder builder, MethodInfo info) {
            builder.Append(FuncTemplate[1]);
            builder.Append(data.Item2);
            builder.Append(ArgumentValueTemplate[0]);
            var parameterInfos = info.GetParameters();
            builder.Append(parameterInfos[0].ParameterType.GetName());
            builder.AppendLine( ">()));");
        }
        static void GenerateBinaryInstruction((SpecialInstructionType,string)data,StringBuilder builder, MethodInfo info) {
            builder.Append(FuncTemplate[2]);
            var parameterInfos = info.GetParameters();
            builder.Append(ArgumentValueTemplate[0]);
            builder.Append(parameterInfos[0].ParameterType.GetName());
            builder.Append( ">()");
            builder.Append(data.Item2);
            builder.Append(ArgumentValueTemplate[1]);
            builder.Append(parameterInfos[1].ParameterType.GetName());
            builder.AppendLine( ">()));");
        }
        static void GenerateBinaryInstructionWithTypes((SpecialInstructionType,string)data, MethodInfo info) {
            var builder = MemberBuilder2;
            builder.Append(FuncTemplate[2]);
            var parameterInfos = info.GetParameters();
            BuildParamWithTypes(0, parameterInfos[0]);
            MemberBuilder.Append(',');
            builder.Append( ">()");
            builder.Append(data.Item2);
            BuildParamWithTypes(1, parameterInfos[1]);
            MemberBuilder.Append("),");
            builder.AppendLine( ">()));");
        }
        
         static bool GenerateGetIndexer(MethodInfo info) {
             var parameterInfos = info.GetParameters();
             var length = parameterInfos.Length;
             if(5<length) return false;
             var builder=MemberBuilder;
             builder.Append( FuncTemplate[length+1]);
             builder.Append(ArgumentValueTemplate[0]);
             builder.Append(info.DeclaringType.GetName());
             builder.Append(">()[");
             for (int i = 0; i < length; i++) {
                 if (!BuildParam(i+1,parameterInfos[i])) return false;
                 builder.Append(i != length - 1 ? ">()," : ">()");
             }
             builder.AppendLine( "]));");
             return true;
        }
        static bool GenerateSetIndexer(MethodInfo info) {
             var parameterInfos = info.GetParameters();
             var length = parameterInfos.Length;
             if(5<length) return false;
             var builder=MemberBuilder;
             builder.Append( ActionTemplate[length+1]);
             builder.Append(ArgumentValueTemplate[0]);
             builder.Append(info.DeclaringType.GetName());
             builder.Append(">()[");
             for (int i = 0; i < length-1; i++) {
                 if (!BuildParam(i+1,parameterInfos[i])) return false;
                 builder.Append(i != length - 2 ? ">()," : ">()]=");
             }
             if (!BuildParam(length,parameterInfos[length-1])) return false;
             builder.AppendLine(">());");
             return true;
        }
        
        
        
         static bool GenerateInstanceField(FieldInfo info) {
             if (info.IsDefined(typeof(ObsoleteAttribute))) {
                return false;
             }
             var name = info.Name;
             if( BannedNames.Contains(name) )return false;
            var builder=MemberBuilder;
             builder.Append(FieldGetterTemplate);
             builder.Append(name);
             builder.Append("\", ");
             builder.Append(FuncTemplate[1]);
             builder.Append(ArgumentValueTemplate[0]);
             builder.Append(info.DeclaringType.GetName());
             builder.Append( ">().");
             builder.Append(name);
             builder.AppendLine( "));");
             if (info.IsInitOnly || info.IsLiteral) return true;
            
             builder.Append(FieldSetterTemplate);
             builder.Append(name);
             builder.Append("\", ");
             builder.Append(ActionTemplate[2]);
             builder.Append(ArgumentValueTemplate[0]);
             builder.Append(info.DeclaringType.GetName());
             builder.Append(">().");
             builder.Append(name);
             builder.Append("=");
             builder.Append(ArgumentValueTemplate[1]);
             builder.Append(info.FieldType.GetName());
             builder.AppendLine( ">());");
             return true;
        }
        static bool GenerateStaticField(FieldInfo info) {
            if (info.IsDefined(typeof(ObsoleteAttribute))) {
                return false;
            }
            var builder=MemberBuilder;
            var name = info.Name;
            if( BannedNames.Contains(name) )return false;
            if (info.IsInitOnly || info.IsLiteral) {
                builder.Append(ConstTemplate);
                builder.Append(name);
                builder.Append(ConstTemplateVariable);
                builder.Append(info.FieldType.GetName());
                builder.Append( ">(");
                builder.Append(info.DeclaringType.GetName());
                builder.Append('.');
                builder.Append(name);
                builder.AppendLine( "));");
                return true;
            }
            builder.Append(FieldGetterTemplate);
            builder.Append(name);
            builder.Append("\", ");
            builder.Append(FuncTemplate[0]);
            builder.Append(info.DeclaringType.GetName());
            builder.Append('.');
            builder.Append(name);
            builder.AppendLine( "));");
            
            builder.Append(FieldSetterTemplate);
            builder.Append(name);
            builder.Append("\", ");
            builder.Append(ActionTemplate[1]);
            builder.Append(info.DeclaringType.GetName());
            builder.Append('.');
            builder.Append(name);
            builder.Append("=");
            builder.Append(ArgumentValueTemplate[1]);
            builder.Append(info.FieldType.GetName());
            builder.AppendLine( ">());");
            return true;
        }
        static bool GenerateInstanceProperty(PropertyInfo info) {
            if (info.IsDefined(typeof(ObsoleteAttribute))) {
                return false;
            }
            var builder=MemberBuilder;
            var name = info.Name;
            if( BannedNames.Contains(name) )return false;
            if(name=="Item") return false;
            if (info.CanRead) {
                builder.Append(PropertyGetterTemplate);
                builder.Append(name);
                builder.Append("\", ");
                builder.Append(FuncTemplate[1]);
                builder.Append(ArgumentValueTemplate[0]);
                builder.Append(info.DeclaringType.GetName());
                builder.Append( ">().");
                builder.Append(name);
                builder.AppendLine( "));");
            }
            if (info.CanWrite&&info.GetSetMethod(true).IsPublic) {
                builder.Append(PropertySetterTemplate);
                builder.Append(name);
                builder.Append("\", ");
                builder.Append(ActionTemplate[2]);
                builder.Append(ArgumentValueTemplate[0]);
                builder.Append(info.DeclaringType.GetName());
                builder.Append(">().");
                builder.Append(name);
                builder.Append("=");
                builder.Append(ArgumentValueTemplate[1]);
                builder.Append(info.PropertyType.GetName());
                builder.AppendLine( ">());");
            }
            return true;
        }
        static bool GenerateStaticProperty(PropertyInfo info) {
            if (info.IsDefined(typeof(ObsoleteAttribute))) {
                return false;
            }
            var builder=MemberBuilder;
            var name = info.Name;
            if( BannedNames.Contains(name) )return false;
            if (info.CanRead) {
                builder.Append(PropertyGetterTemplate);
                builder.Append(name);
                builder.Append("\", ");
                builder.Append(FuncTemplate[0]);
                builder.Append(info.DeclaringType.GetName());
                builder.Append('.');
                builder.Append(name);
                builder.AppendLine( "));");
            }
            if (info.CanWrite) {
                builder.Append(PropertySetterTemplate);
                builder.Append(name);
                builder.Append("\", ");
                builder.Append(ActionTemplate[1]);
                builder.Append(info.DeclaringType.GetName());
                builder.Append('.');
                builder.Append(name);
                builder.Append("=");
                builder.Append(ArgumentValueTemplate[0]);
                builder.Append(info.PropertyType.GetName());
                builder.AppendLine( ">());");
            }
            return true;
        }

    }
}