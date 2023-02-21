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
using static System.Runtime.CompilerServices.Unsafe;
using __V = YS.Variable;
namespace YS.Generated {
    public static partial  class Wrapper {
";
        const string ModuleStart= @"
        static TypeModule Create_";
        const string AOTModuleStart= @"#else
        static unsafe TypeModule Create_";

         const string ClassStart = @"Module(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(";

        

         const string FileEnd = 
             
            @"            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}";

         const string FunctionStart = "       static void ";
        //This is a template static void UnityEngine_Vector3_op_Addition(__V res,__V  i1, __V i2) => As<_Vector3>(res).value=As<_Vector3>(i1).value+As<_Vector3>(i2).value;
         
         const string ConstTemplate = "            module.RegisterConst(\"";
         const string ConstTemplateVariable = "\",new Variable<";
         const string MethodTemplate = "            module.RegisterMethod";
         const string ConstructorTemplate = "            module.RegisterConstructor(";
         const string FieldGetterTemplate = "            module.RegisterFieldGetter(\"";
         const string FieldSetterTemplate = "            module.RegisterFieldSetter(\"";
         const string PropertyGetterTemplate = "            module.RegisterPropertyGetter(\"";
         const string PropertySetterTemplate = "            module.RegisterPropertySetter(\"";
         
         
         static string[] NewActionTemplate = {
             "() => ",
             "(__V  i1) => ",
             "(__V  i1, __V  i2) => ",
             "(__V  i1, __V  i2, __V  i3)  =>  ",
             "(__V  i1, __V  i2, __V  i3, __V  i4)  => ",
             "(__V  i1, __V  i2, __V  i3, __V  i4, __V  i5)  => ",
             "(__V  i1, __V  i2, __V  i3, __V  i4, __V  i5, __V  i6)  => ",
             "(__V  i1, __V  i2, __V  i3, __V  i4, __V  i5, __V  i6, __V  i7)  => "
         };
        public static string[] NewArgumentValueTemplate = {
            ">(i1).value", ">(i2).value", ">(i3).value", ">(i4).value",
            ">(i5).value", ">(i6).value", ">(i7).value", ">(i8).value",
        };
        
        static string[] NewFuncTemplate = {
            "(__V res) =>  As<",
            "(__V res, __V  i1) => As<",
            "(__V res,__V  i1, __V  i2) => As<",
            "(__V res,__V  i1, __V  i2, __V  i3)  =>  As<",
            "(__V res,__V  i1, __V  i2, __V  i3, __V  i4)  => As<",
            "(__V res,__V  i1, __V  i2, __V  i3, __V  i4, __V  i5)  => As<",
            "(__V res,__V  i1, __V  i2, __V  i3, __V  i4, __V  i5, __V  i6)  => As<",
            "(__V res,__V  i1, __V  i2, __V  i3, __V  i4, __V  i5, __V  i6, __V  i7)  => As<"
        };
        static readonly StringBuilder InspectorBuilder=new (100);
        static readonly StringBuilder RegisterBuilder=new (100);
        static readonly StringBuilder AliasBuilder=new (100);
        static readonly StringBuilder FunctionNameBuilder=new (30);
        static readonly StringBuilder FunctionBuilder=new (2000);
        static readonly StringBuilder FullFunctionBuilder=new (2000);
        static readonly StringBuilder ClassBuilder=new (2000);
        static readonly StringBuilder AOTClassBuilder=new (2000);
        static readonly StringBuilder FileBuilder=new (2000);
        static readonly StringBuilder MemberBuilder=new (100);
        static readonly StringBuilder AOTMemberBuilder=new (100);
        static readonly StringDictionary<string> AliasDict=new (20);
        static readonly Dictionary<Type, string> TypeNameDict = new (10);
        static readonly StringDictionary< SimpleList<MethodInfo>> MethodInfoDict= new (10);
        public static readonly StringHashSet BannedNames = new () {};

        static WrapperGenerator() {
            BannedNames.Add("runInEditMode");
        }
        static void Initialize() {
            AliasBuilder.Clear();
            FileBuilder.Clear();
            ClassBuilder.Clear();
            AOTClassBuilder.Clear();
            FunctionNameBuilder.Clear();
            FunctionBuilder.Clear();
            FullFunctionBuilder.Clear();
            MemberBuilder.Clear();
            AOTMemberBuilder.Clear();
            AliasDict.Clear();
            TypeNameDict.Clear();
            MethodInfoDict.Clear();
        }
        static void Append(this StringBuilder builder,string arg1,string arg2) {
            builder.Append(arg1);
            builder.Append(arg2);
        }
        static void Append(this StringBuilder builder,StringBuilder arg1,string arg2) {
            builder.Append(arg1);
            builder.Append(arg2);
        }
        static void Append(this StringBuilder builder,string arg1,string arg2,string arg3) {
            builder.Append(arg1);
            builder.Append(arg2);
            builder.Append(arg3);
        }static void Append(this StringBuilder builder,string arg1,string arg2,string arg3,string arg4) {
            builder.Append(arg1);
            builder.Append(arg2);
            builder.Append(arg3);
            builder.Append(arg4);
        }
        
        static void AppendStaticArg(this StringBuilder builder,int argNumber, ParameterInfo[] parameterInfos) {
            builder.Append("As<");
            builder.Append(parameterInfos[argNumber].ParameterType.GetVariableName());
            builder.Append(NewArgumentValueTemplate[argNumber]);
        }
        static void AppendInstanceArg(this StringBuilder builder,int argNumber, ParameterInfo[] parameterInfos) {
            builder.Append("As<");
            builder.Append(parameterInfos[argNumber].ParameterType.GetVariableName());
            builder.Append(NewArgumentValueTemplate[argNumber+1]);
        }
        static void AppendNameBuilderAndClear(this StringBuilder builder) {
            builder.Append(FunctionStart);
            builder.Append(FunctionNameBuilder);
            FunctionNameBuilder.Clear();
        }

        static void CopyMemberToAOT() {
            AOTMemberBuilder.Append(MemberBuilder);
            AOTMemberBuilder.Append("&");
            AOTMemberBuilder.Append(FunctionNameBuilder);
            AOTMemberBuilder.AppendLine(");");
        }
        static string GetVariableName(this Type type) {
            if (type.IsArray) {
                 return "Variable<"+GetName(type.GetElementType())+"[]>";
            }

            return "_" + GetName(type);
        }

        static string GetName(this Type type) {
            if (TypeNameDict.TryGetValue(type, out var name)){
                return name;
            }
            if (type.IsArray) {
                 name = type.GetElementType().GetName()+"[]";
                 TypeNameDict[type] = name;
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
                    AliasDict[nameData.Name] = nameData.Name;
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

        static void AddTemp() {
            ClassBuilder.Append(MemberBuilder);
            MemberBuilder.Clear();
            AOTClassBuilder.Append(AOTMemberBuilder);
            AOTMemberBuilder.Clear();
            if(FunctionBuilder.Length!=0) {
                FileBuilder.Append(FunctionBuilder);
                FunctionBuilder.Clear();
            }
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

                var fullName = typeName;
                FileBuilder.Append(FileStart);
                ClassBuilder.AppendLine("#if !AOT");
                ClassBuilder.Append(ModuleStart,typeName);
                AOTClassBuilder.Append(AOTModuleStart,typeName);
                
                
                typeName = GetName(type);
                ClassBuilder.Append(ClassStart,typeName);
                ClassBuilder.AppendLine("));");
                AOTClassBuilder.Append(ClassStart,typeName);
                AOTClassBuilder.AppendLine("));");
                
                var count = 0;
                foreach (var constructorInfo in type.GetConstructors()) {
                    if (!Generate(constructorInfo,fullName,count)) continue;
                    count++;
                    AddTemp() ;
                }
                foreach (var fieldInfo in type.GetFields(BindingFlags.DeclaredOnly|BindingFlags.Public | BindingFlags.Static)) {
                    if (!GenerateStaticField(fieldInfo,fullName)) continue;
                    AddTemp() ;
                }
                foreach (var fieldInfo in type.GetFields(BindingFlags.DeclaredOnly|BindingFlags.Public |BindingFlags.Instance)) {
                    if (!GenerateInstanceField(fieldInfo,fullName)) continue;
                    AddTemp() ;
                }
                foreach (var propertyInfo in type.GetProperties(BindingFlags.DeclaredOnly|BindingFlags.Public | BindingFlags.Static)) {
                    if (!GenerateStaticProperty(propertyInfo,fullName)) continue;
                    AddTemp() ;
                }
                
                foreach (var propertyInfo in type.GetProperties(BindingFlags.DeclaredOnly|BindingFlags.Public |BindingFlags.Instance)) {
                    if (!GenerateInstanceProperty(propertyInfo,fullName)) continue;
                    AddTemp() ;
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
                        if (!Generate(list[0],fullName)) continue;
                        AddTemp() ;
                    }
                    else {
                        count = 0;
                        foreach (var methodInfo in list.AsSpan()) {
                            if (!GenerateWithType(methodInfo,fullName,count)) continue;
                            count++;
                            AddTemp() ;
                        }
                    }
                }
                FileBuilder.Append(ClassBuilder);
                FileBuilder.Append(AOTClassBuilder);
                FileBuilder.AppendLine("#endif");
                FileBuilder.Append(FileEnd);
                AliasBuilder.AppendLine(@"#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif");
                foreach (var pair in AliasDict) {
                    if(pair.Value.Contains('.')&&!pair.Value.Contains('[')) {
                       AliasBuilder.Append("using ");
                       AliasBuilder.Append(pair.Key);
                       AliasBuilder.Append(" = ");
                       AliasBuilder.Append(pair.Value);
                       AliasBuilder.AppendLine(";");
                    }
                    AliasBuilder.Append("using _");
                    AliasBuilder.Append(pair.Key);
                    AliasBuilder.Append(" = YS.Variable<");
                    AliasBuilder.Append(pair.Value);
                    AliasBuilder.AppendLine(">;");
                }
                return  AliasBuilder.ToString()+FileBuilder.ToString();
            }
            finally {
                Initialize();
            }
        }


        static string GenerateEnum(Type type) {
           
            var typeName = type.BuildFullName();
            ClassBuilder.Append(FileStart,ModuleStart);
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
        static bool Generate(ConstructorInfo info,string fullName,int count) {
            if (info.IsDefined(typeof(ObsoleteAttribute))) {
                return false;
            }
            var parameterInfos = info.GetParameters();
            var length = parameterInfos.Length;
            if(4<length) return false;
            var builder=MemberBuilder;
            var functionBuilder=FunctionBuilder;
            functionBuilder.Append("        static void ");
            FunctionNameBuilder.Append(fullName);
            FunctionNameBuilder.Append("_ctor");
            for (int i = 0; i < count; i++) {
                FunctionNameBuilder.Append('_');
            }
            functionBuilder.Append(FunctionNameBuilder);
            functionBuilder.Append(NewFuncTemplate[length]);
            
            functionBuilder.Append(info.DeclaringType.GetVariableName());
            functionBuilder.Append(">(res).value = new ");
            functionBuilder.Append(info.DeclaringType.GetName());
            functionBuilder.Append('(');
            builder.Append(ConstructorTemplate);
            for (int i = 0; i < length; i++) {
                if (!BuildParamWithTypes(i, parameterInfos[i])) return false;
                builder.Append(',');
                if(i != length - 1) {
                    functionBuilder.Append(',');
                }
            }

            CopyMemberToAOT();
            builder.Append(FunctionNameBuilder);
            builder.AppendLine(");");
            FunctionNameBuilder.Clear();
            functionBuilder.AppendLine( ");");
            FileBuilder.Append(functionBuilder);
            functionBuilder.Clear();
            return true;
        }
        static bool Generate(MethodInfo info,string fullName) {
            if (info.IsDefined(typeof(ObsoleteAttribute))||info.IsGenericMethodDefinition) {
                return false;
            }
            if (info.IsSpecialName) {
                return GenerateSpecialName(info,fullName);
            }
            var parameterInfos = info.GetParameters();
            var length = parameterInfos.Length;
            
            if(5<length) return false;
            var builder=MemberBuilder;
            var functionBuilder=FunctionBuilder;
            builder.Append(MethodTemplate);
            builder.Append(length+ (info.IsStatic ? 0 : 1)+(info.ReturnType != typeof(void)?1:0));
            builder.Append("(\"");
            builder.Append(info.Name);
            builder.Append("\",");
            functionBuilder.Append(FunctionStart);
            FunctionNameBuilder.Append(fullName);
            FunctionNameBuilder.Append('_');
            FunctionNameBuilder.Append(info.Name);
            functionBuilder.Append(FunctionNameBuilder);
            if (info.IsStatic) {
                if (info.ReturnType != typeof(void)) {
                    functionBuilder.Append( NewFuncTemplate[length] ,info.ReturnType.GetVariableName(),">(res).value=");
                }
                else {
                    functionBuilder.Append(NewActionTemplate[length]);
                }
                functionBuilder.Append(info.DeclaringType.GetName());
                functionBuilder.Append('.');
                functionBuilder.Append(info.Name);
                functionBuilder.Append('(');
                for (int i = 0; i < length; i++) {
                    if (!BuildParam(i, parameterInfos[i])) return false;
                    if(i != length - 1) {
                        functionBuilder.Append(',');
                    }
                }
                functionBuilder.AppendLine(");");
            }
            else {
                if (info.ReturnType != typeof(void)) {
                    functionBuilder.Append( NewFuncTemplate[length+1] ,info.ReturnType.GetVariableName(),">(res).value=");
                }
                else {
                    functionBuilder.Append(NewActionTemplate[length+1]);
                }

                functionBuilder.Append("As<",info.DeclaringType.GetVariableName());
                functionBuilder.Append(">(i1).value.",info.Name);
                functionBuilder.Append('(');
                for (int i = 0; i < length; i++) {
                    if (!BuildParam(i+1,parameterInfos[i])) return false;
                    if(i != length - 1) functionBuilder.Append(',');
                    
                }
                functionBuilder.AppendLine(");");
            }
            CopyMemberToAOT();
            builder.Append(FunctionNameBuilder);
            FunctionNameBuilder.Clear();
            builder.AppendLine(");");
            return true;
        }
        static bool GenerateWithType(MethodInfo info,string fullName,int count) {
            if (info.IsDefined(typeof(ObsoleteAttribute))||info.IsGenericMethodDefinition) {
                return false;
            }
            if (info.IsSpecialName) {
                return GenerateSpecialNameWithTypes(info,fullName,count);
            }
            var parameterInfos = info.GetParameters();
            var length = parameterInfos.Length;
            
            if(5<length) return false;
            var builder=MemberBuilder;
            var functionBuilder=FunctionBuilder;
            builder.Append(MethodTemplate);
            builder.Append(length+ (info.IsStatic ? 0 : 1)+(info.ReturnType != typeof(void)?1:0));
            builder.Append("(\"");
            builder.Append(info.Name);
            builder.Append("\", Types(");
            functionBuilder.Append(FunctionStart);
            FunctionNameBuilder.Append(fullName);
            FunctionNameBuilder.Append('_');
            FunctionNameBuilder.Append(info.Name);
            for (int i = 0; i < count; i++) {
                FunctionNameBuilder.Append('_');
            }
            functionBuilder.Append(FunctionNameBuilder);
            if (info.IsStatic) {
                if (info.ReturnType != typeof(void)) {
                    functionBuilder.Append( NewFuncTemplate[length] ,info.ReturnType.GetVariableName(),">(res).value=");
                }
                else {
                    functionBuilder.Append(NewActionTemplate[length]);
                }
                functionBuilder.Append(info.DeclaringType.GetName());
                functionBuilder.Append('.');
                functionBuilder.Append(info.Name);
                functionBuilder.Append('(');
                for (int i = 0; i < length; i++) {
                    if (!BuildParamWithTypes(i, parameterInfos[i])) return false;
                    if(i != length - 1) {
                        builder.Append(',');
                        functionBuilder.Append(',');
                    }
                }

                functionBuilder.AppendLine(");");
            }
            else {
                if (info.ReturnType != typeof(void)) {
                    functionBuilder.Append( NewFuncTemplate[length+1] ,info.ReturnType.GetVariableName(),">(res).value=");
                }
                else {
                    functionBuilder.Append(NewActionTemplate[length+1]);
                }
                functionBuilder.Append("As<",info.DeclaringType.GetVariableName());
                functionBuilder.Append(">(i1).value.",info.Name);
                functionBuilder.Append('(');
                for (int i = 0; i < length; i++) {
                    if (!BuildParamWithTypes(i+1,parameterInfos[i])) return false;
                    if(i != length - 1) {
                        builder.Append(',');
                        functionBuilder.Append(',');
                    }
                }
                
                functionBuilder.AppendLine(");");
            }
            builder.Append("),");
            CopyMemberToAOT();
            builder.Append(FunctionNameBuilder);
            FunctionNameBuilder.Clear();
            builder.AppendLine(");");
            return true;
        }
        
        
        static bool BuildParam(int inputIndex,ParameterInfo info) {
            var functionBuilder=FunctionBuilder;
            var type = info.ParameterType;
            if (type.IsByRefLike||type.IsPointer) {
                functionBuilder.Clear();
                MemberBuilder.Clear();
                AOTMemberBuilder.Clear();
                return false;
            }
            string name;
            if (!type.IsByRef) {
                name = type.GetVariableName();
                functionBuilder.Append("As<");
                functionBuilder.Append(name);
                functionBuilder.Append(NewArgumentValueTemplate[inputIndex]);
                
                return true;
            }
            type = info.ParameterType.GetElementType();
            name = type.GetVariableName();
            if (info.IsIn) {
                functionBuilder.Append("As<");
                functionBuilder.Append(name);
                functionBuilder.Append(NewArgumentValueTemplate[inputIndex]);
            }
            else if (info.IsOut) {
                functionBuilder.Append("out As<");
                functionBuilder.Append(name);
                functionBuilder.Append(NewArgumentValueTemplate[inputIndex]);
               
            }
            else {
                functionBuilder.Append("ref As<");
                functionBuilder.Append(name);
                functionBuilder.Append(NewArgumentValueTemplate[inputIndex]);
                
            }
            return true;
        }
        static bool BuildParamWithTypes(int inputIndex,ParameterInfo info) {
            var builder=MemberBuilder;
            var functionBuilder=FunctionBuilder;
            var type = info.ParameterType;
            if (type.IsByRefLike||type.IsPointer) {
                builder.Clear();
                AOTMemberBuilder.Clear();
                functionBuilder.Clear();
                return false;
            }

            string name;
            if (!type.IsByRef) {
                builder.Append("typeof(");
                builder.Append(type.GetName());
                builder.Append(')');
                functionBuilder.Append("As<");
                functionBuilder.Append(type.GetVariableName());
                functionBuilder.Append(NewArgumentValueTemplate[inputIndex]);
                
                return true;
            }
            type = info.ParameterType.GetElementType();
            
            builder.Append("typeof(");
            builder.Append(type.GetName());
            builder.Append(").MakeByRefType()");
            name = type.GetVariableName();
            if (info.IsIn) {
                functionBuilder.Append("As<");
                functionBuilder.Append(name);
                functionBuilder.Append(NewArgumentValueTemplate[inputIndex]);
            }
            else if (info.IsOut) {
                functionBuilder.Append("out As<");
                functionBuilder.Append(name);
                functionBuilder.Append(NewArgumentValueTemplate[inputIndex]);
               
            }
            else {
                functionBuilder.Append("ref As<");
                functionBuilder.Append(name);
                functionBuilder.Append(NewArgumentValueTemplate[inputIndex]);
                
            }
            return true;
        }
        


         static bool GenerateSpecialName(MethodInfo info,string fullTypeName) {
            var name = info.Name;
            if(TryGetSpecialData(name,out var data)) {
                MemberBuilder.Append(MethodTemplate);
                MemberBuilder.Append(info.GetParameters().Length+ (info.IsStatic ? 0 : 1)+(info.ReturnType != typeof(void)?1:0));
                MemberBuilder.Append("(\"");
                MemberBuilder.Append(name);
                MemberBuilder.Append("\", ");
                FunctionNameBuilder.Append(fullTypeName,"_" ,name);
                CopyMemberToAOT();
                MemberBuilder.Append(FunctionNameBuilder);
                MemberBuilder.AppendLine(");");
                FunctionBuilder.AppendNameBuilderAndClear();
                switch (data.Item1) {
                    case SpecialInstructionType.Unary:GenerateUnaryInstruction( data, info);
                        break;
                    case SpecialInstructionType.Binary:
                        GenerateBinaryInstruction(data, info);
                        return true;
                    case SpecialInstructionType.IndexGetter:
                      return  GenerateGetIndexer( info);
                     case SpecialInstructionType.IndexSetter:
                      return  GenerateSetIndexer( info);
                  
                }
            }
            return false;
         }
         static bool GenerateSpecialNameWithTypes(MethodInfo info,string fullTypeName,int count) {
            var name = info.Name;
            MemberBuilder.Clear();
            var data = GetSpecialData(name);
            if (data.Item1 is not SpecialInstructionType.Binary) {
                return false;
            }
            MemberBuilder.Append(MethodTemplate);
            MemberBuilder.Append(info.GetParameters().Length+ (info.IsStatic ? 0 : 1)+(info.ReturnType != typeof(void)?1:0));
            MemberBuilder.Append("(\"");
            MemberBuilder.Append(name);
            MemberBuilder.Append("\", Types(");
            FunctionNameBuilder.Append(fullTypeName,"_" ,name);
            for (int i = 0; i < count; i++) {
                FunctionNameBuilder.Append('_');
            }
            FunctionBuilder.Append(FunctionStart);
            FunctionBuilder.Append(FunctionNameBuilder);
            switch (data.Item1) {
                case SpecialInstructionType.Binary:GenerateBinaryInstructionWithTypes(data, info);
                    break;
            } 
            
            MemberBuilder.Append("),");
            CopyMemberToAOT();
            MemberBuilder.Append(FunctionNameBuilder);
            MemberBuilder.AppendLine(");");
            FunctionNameBuilder.Clear();

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




        static void GenerateUnaryInstruction((SpecialInstructionType,string)data,MethodInfo info) {
            var builder = FunctionBuilder;
            builder.Append(NewFuncTemplate[1],info.ReturnType.GetVariableName(),">(res).value=");
            builder.Append(data.Item2);
            builder.AppendStaticArg(0,info.GetParameters());
            builder.AppendLine(";");
        }
        static void GenerateBinaryInstruction((SpecialInstructionType,string)data,MethodInfo info) {
            var builder = FunctionBuilder;
            builder.Append(NewFuncTemplate[2], info.ReturnType.GetVariableName(), ">(res).value=");
            var parameterInfos = info.GetParameters();
            builder.AppendStaticArg(0,parameterInfos);
            builder.Append(data.Item2);
            builder.AppendStaticArg(1,parameterInfos);
            builder.AppendLine(";");
            
        }
        static void GenerateBinaryInstructionWithTypes((SpecialInstructionType,string)data, MethodInfo info) {
            var builder = FunctionBuilder;
            builder.Append(NewFuncTemplate[2], info.ReturnType.GetVariableName(), ">(res).value=");
            var parameterInfos = info.GetParameters();
            BuildParamWithTypes(0, parameterInfos[0]);
            MemberBuilder.Append(',');
            builder.Append(data.Item2);
            BuildParamWithTypes(1, parameterInfos[1]);
            builder.AppendLine(";");
        }
        
         static bool GenerateGetIndexer(MethodInfo info) {
             var parameterInfos = info.GetParameters();
             var length = parameterInfos.Length;
             if(5<length) return false;
             var functionBuilder=FunctionBuilder;
             functionBuilder.Append( NewFuncTemplate[length+1],info.ReturnType.GetVariableName(),">(res).value=As<");
             functionBuilder.Append(info.DeclaringType.GetVariableName(),">(i1).value[");
             for (int i = 0; i < length; i++) {
                 if (!BuildParam(i+1,parameterInfos[i])) return false;
                 if(i != length - 1) functionBuilder.Append(',');
             }
             functionBuilder.AppendLine( "];");
             return true;
        }
        static bool GenerateSetIndexer(MethodInfo info) {
             var parameterInfos = info.GetParameters();
             var length = parameterInfos.Length;
             if(5<length) return false;
             var functionBuilder=FunctionBuilder;
             functionBuilder.Append( NewActionTemplate[length+1],"As<",info.DeclaringType.GetVariableName(),">(i1).value[");
             for (int i = 0; i < length-1; i++) {
                 if (!BuildParam(i+1,parameterInfos[i])) return false;
                 if(i != length - 2) functionBuilder.Append(',');
             }
             functionBuilder.Append("]=");
             if (!BuildParam(length,parameterInfos[length-1])) return false;
             functionBuilder.AppendLine(";");
             return true;
        }
        
        
        
         static bool GenerateInstanceField(FieldInfo info,string fullName) {
             if (info.IsDefined(typeof(ObsoleteAttribute))) {
                return false;
             }
             var fieldType = info.FieldType;
             if (fieldType.IsByRefLike) return false;
             var name = info.Name;
             if( BannedNames.Contains(name) )return false;
            var builder=MemberBuilder;
            var functionBuilder=FunctionBuilder;
             builder.Append(FieldGetterTemplate);
             builder.Append(name);
             builder.Append("\", ");
             FunctionNameBuilder.Append(fullName,"_get_",name);
             CopyMemberToAOT();
             builder.Append(FunctionNameBuilder,");");
             builder.AppendLine();
             functionBuilder.AppendNameBuilderAndClear();
             functionBuilder.Append(NewFuncTemplate[1]);
             functionBuilder.Append(fieldType.GetVariableName());
             functionBuilder.Append(">(res).value=As<");
             functionBuilder.Append(info.DeclaringType.GetVariableName());
             functionBuilder.Append( ">(i1).value.");
             functionBuilder.Append(name);
             functionBuilder.AppendLine(";");
             if (info.IsInitOnly || info.IsLiteral) return true;
            
             builder.Append(FieldSetterTemplate,name,"\", ");
             FunctionNameBuilder.Append(fullName,"_set_",name);
             builder.Append(FunctionNameBuilder,");");
             builder.AppendLine();
             AOTMemberBuilder.Append(FieldSetterTemplate,name,"\",&");
             AOTMemberBuilder.Append(FunctionNameBuilder,");");
             AOTMemberBuilder.AppendLine();
             functionBuilder.AppendNameBuilderAndClear();
             functionBuilder.Append(NewActionTemplate[2],"As<",info.DeclaringType.GetVariableName(),NewArgumentValueTemplate[0]);
             functionBuilder.Append('.');
             functionBuilder.Append(name,"= As<",info.FieldType.GetVariableName(),NewArgumentValueTemplate[1]);
             functionBuilder.AppendLine(";");
             return true;
        }
        static bool GenerateStaticField(FieldInfo info,string fullName) {
            if (info.IsDefined(typeof(ObsoleteAttribute))) {
                return false;
            }
            var builder=MemberBuilder;
            var name = info.Name;
            if( BannedNames.Contains(name) )return false;
            if (info.IsInitOnly || info.IsLiteral) {
                builder.Append(ConstTemplate,name,ConstTemplateVariable,info.FieldType.GetName());
                builder.Append( ">(",info.DeclaringType.GetName());
                builder.Append('.');
                builder.Append(name);
                builder.AppendLine( "));");
                return true;
            }
            var fieldType = info.FieldType;
            var functionBuilder=FunctionBuilder;
            builder.Append(FieldGetterTemplate);
            builder.Append(name);
            builder.Append("\", ");
            FunctionNameBuilder.Append(fullName,"_get_",name);
            CopyMemberToAOT();
            builder.Append(FunctionNameBuilder,");");
            builder.AppendLine();
            functionBuilder.AppendNameBuilderAndClear();
            functionBuilder.Append(NewFuncTemplate[0],fieldType.GetVariableName(),">(res).value=",info.DeclaringType.GetName());
            functionBuilder.Append(".value.",name);
            functionBuilder.AppendLine(";");
            
            builder.Append(FieldSetterTemplate,name,"\", ");
            FunctionNameBuilder.Append(fullName,"_set_",name);
            
            builder.Append(FunctionNameBuilder,");");
            builder.AppendLine();
            AOTMemberBuilder.Append(FieldSetterTemplate,name,"\",&");
            AOTMemberBuilder.Append(FunctionNameBuilder,");");
            AOTMemberBuilder.AppendLine();
            functionBuilder.AppendNameBuilderAndClear();
            functionBuilder.Append(NewActionTemplate[1]);
            functionBuilder.Append(info.DeclaringType.GetName());
            functionBuilder.Append('.');
            functionBuilder.Append(name,"=As<",info.FieldType.GetVariableName(),NewArgumentValueTemplate[0]);
            functionBuilder.AppendLine(";");
            return true;
        }
        static bool GenerateInstanceProperty(PropertyInfo info,string fullTypeName) {
            if (info.IsDefined(typeof(ObsoleteAttribute))) {
                MemberBuilder.Clear();
                AOTMemberBuilder.Clear();
                return false;
            }
            var builder=MemberBuilder;
            var name = info.Name;
            if( BannedNames.Contains(name) ||name=="Item"){
                MemberBuilder.Clear();
                AOTMemberBuilder.Clear();
                return false;
            }
            var functionBuilder=FunctionBuilder;
            var fieldType=info.PropertyType;
            if (info.CanRead) {
                builder.Append(PropertyGetterTemplate);
                builder.Append(name);
                builder.Append("\", ");
                FunctionNameBuilder.Append(fullTypeName,"_get_",name);
                CopyMemberToAOT();
                builder.Append(FunctionNameBuilder,");");
                builder.AppendLine();
                functionBuilder.AppendNameBuilderAndClear();
                functionBuilder.Append(NewFuncTemplate[1], fieldType.GetVariableName(),">(res).value=As<");
                functionBuilder.Append(info.DeclaringType.GetVariableName(),">(i1).value.",name);
                functionBuilder.AppendLine( ";");
            }
            if (info.CanWrite&&info.GetSetMethod(true).IsPublic) {
                builder.Append(PropertySetterTemplate,name,"\", ");
                FunctionNameBuilder.Append(fullTypeName,"_set_",name);
                builder.Append(FunctionNameBuilder,");");
                builder.AppendLine();
                AOTMemberBuilder.Append(PropertySetterTemplate,name,"\",& ");
                AOTMemberBuilder.Append(FunctionNameBuilder,");");
                AOTMemberBuilder.AppendLine();
                
                functionBuilder.AppendNameBuilderAndClear();
                functionBuilder.Append(NewActionTemplate[2], "As<");
                functionBuilder.Append(info.DeclaringType.GetVariableName(),">(i1).value.",name);
                functionBuilder.Append(" = As<",fieldType.GetVariableName(),">(i2).value");
                functionBuilder.AppendLine(";");
            }
            return true;
        }
        static bool GenerateStaticProperty(PropertyInfo info,string fullTypeName) {
            if (info.IsDefined(typeof(ObsoleteAttribute))) {
                return false;
            }
            var builder=MemberBuilder;
            var name = info.Name;
            if( BannedNames.Contains(name) )return false;
            var functionBuilder=FunctionBuilder;
            var fieldType=info.PropertyType;
            if (info.CanRead) {
                builder.Append(PropertyGetterTemplate);
                builder.Append(name);
                builder.Append("\", ");
                FunctionNameBuilder.Append(fullTypeName,"_get_",name);
                CopyMemberToAOT();
                builder.Append(FunctionNameBuilder,");");
                builder.AppendLine();
                functionBuilder.AppendNameBuilderAndClear();
                functionBuilder.Append(NewFuncTemplate[0], fieldType.GetVariableName(),">(res).value=");
                functionBuilder.Append(info.DeclaringType.GetName(),".",name);
                functionBuilder.AppendLine( ";");
            }
            if (info.CanWrite) {
                builder.Append(PropertySetterTemplate,name,"\", ");
                FunctionNameBuilder.Append(fullTypeName,"_set_",name);
                builder.Append(FunctionNameBuilder,");");
                builder.AppendLine();
                AOTMemberBuilder.Append(PropertySetterTemplate,name,"\",& ");
                AOTMemberBuilder.Append(FunctionNameBuilder,");");
                AOTMemberBuilder.AppendLine();
                functionBuilder.AppendNameBuilderAndClear();
                functionBuilder.Append(NewActionTemplate[1], info.DeclaringType.GetName(),".",name);
                functionBuilder.Append(" = As<",fieldType.GetVariableName(),">(i1).value");
                functionBuilder.AppendLine(";");
            }
            return true;
        }

    }
}