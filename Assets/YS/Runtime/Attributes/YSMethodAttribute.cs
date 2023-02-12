using System;

namespace YS.Attributes {

    [AttributeUsage(AttributeTargets.Method)]
    public class ReflectionCallAttribute : Attribute {
        
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class CodeGenAssemblyAttribute :Attribute {
      
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class CodeGenTypesAttribute :Attribute {
      
    }
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Field)]
    public class CodeGenAttribute :Attribute {
        public CodeGenerationType CodeGenerationType;
        public CodeGenAttribute(CodeGenerationType codeGenerationType) {
            CodeGenerationType = codeGenerationType;
        }
        public CodeGenAttribute() {
            CodeGenerationType = CodeGenerationType.All;
        }
    }
}