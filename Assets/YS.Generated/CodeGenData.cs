using System;
using System.Collections.Generic;
using static YS.CodeGenerationType;
namespace YS.Generated {
    public static class CodeGenData {
        [CodeGenAssembly]
        public static List<string> AssemblyNameList=new List<string>() {
            "mscorlib",
            "UnityEngine.CoreModule"
        };
        [CodeGenTypes]
        public static List<(CodeGenerationType,Type)> TypeList=new List<(CodeGenerationType, Type)>() {
            (InspectorOnly,typeof(int)),
            (InspectorOnly,typeof(float)),
        };
    }
}