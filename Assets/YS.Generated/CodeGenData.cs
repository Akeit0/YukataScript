using System;
using System.Collections.Generic;
using static YS.CodeGenerationType;
namespace YS.Generated {
    public static class CodeGenData {
        [Attributes.CodeGenAssembly]
        public static List<string> AssemblyNameList=new List<string>() {
            "mscorlib",
            "UnityEngine.CoreModule"
        };
        [Attributes.CodeGenTypes]
        public static List<(CodeGenerationType,Type)> TypeList=new List<(CodeGenerationType, Type)>() {
            (InspectorOnly,typeof(int)),
            (InspectorOnly,typeof(float)),
        };
    }
}