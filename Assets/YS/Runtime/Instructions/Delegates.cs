using System.Text;
using YS.Modules;
using YS.VM;

namespace YS.Instructions {
    
    
    public sealed class DelegateOp : IInstruction {
       
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            var description = DelegateLibrary.DelegateDataArray[vm.ReadUshort()];
            builder.Append(description.DeclaringType.Build());  
            builder.Append('.');
            builder.Append(description.MethodName);  
            builder.Append("()");
        }
    }
    public sealed class Delegate1Op : IInstruction {
        
        public void ToCode(VirtualMachine vm, StringBuilder builder,ref  int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            var description = DelegateLibrary.Delegate1s[vm.ReadUshort()].Data;
            if (description.ReturnType!=null) {
                IInstruction.AppendVariable(builder, vm);
                builder.Append(" = ");
                builder.Append(description.DeclaringType.Build());  
                builder.Append('.');
                builder.Append(description.MethodName);  
                builder.Append("()");
            }
            else {
                builder.Append(description.DeclaringType.Build());  
                builder.Append('.');
                builder.Append(description.MethodName);  
                builder.Append('(');
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
        }
    }
    public sealed class Delegate2Op : IInstruction {
      

        public void ToCode(VirtualMachine vm, StringBuilder builder,ref  int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            var description = DelegateLibrary.Delegate2s[vm.ReadUshort()].Data;
            if (description.ReturnType!=null) {
                IInstruction.AppendVariable(builder, vm);
                builder.Append(" = ");
                builder.Append(description.DeclaringType.Build());  
                builder.Append('.');
                builder.Append(description.MethodName);  
                builder.Append('(');
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
            else {
                builder.Append(description.DeclaringType.Build());  
                builder.Append('.');
                builder.Append(description.MethodName);  
                builder.Append('(');
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
        }
    }
    public sealed class Delegate3Op : IInstruction {

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            var description = DelegateLibrary.Delegate3s[vm.ReadUshort()].Data;
            if (description.ReturnType!=null) {
                IInstruction.AppendVariable(builder, vm);
                builder.Append(" = ");
                builder.Append(description.DeclaringType.Build());  
                builder.Append('.');
                builder.Append(description.MethodName);  
                builder.Append('(');
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
            else {
                builder.Append(description.DeclaringType.Build());  
                builder.Append(description.MethodName);  
                builder.Append('(');
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
        }

    }

    public sealed class Delegate4Op : IInstruction {
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            var description = DelegateLibrary.Delegate4s[vm.ReadUshort()].Data;
            if (description.ReturnType != null) {
                IInstruction.AppendVariable(builder, vm);
                builder.Append(" = ");
                builder.Append(description.DeclaringType.Build());
                builder.Append('.');
                builder.Append(description.MethodName);
                builder.Append('(');
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
            else {
                builder.Append(description.DeclaringType.Build());
                builder.Append('.');
                builder.Append(description.MethodName);
                builder.Append('(');
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
        }
    }

    public sealed class Delegate5Op : IInstruction {
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            var description = DelegateLibrary.Delegate5s[vm.ReadUshort()].Data;
            if (description.ReturnType != null) {
                IInstruction.AppendVariable(builder, vm);
                builder.Append(" = ");
                builder.Append(description.DeclaringType.Build());
                builder.Append('.');
                builder.Append(description.MethodName);
                builder.Append('(');
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
            else {
                builder.Append(description.DeclaringType.Build());
                builder.Append('.');
                builder.Append(description.MethodName);
                builder.Append('(');
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
        }
    }
    public sealed class Delegate6Op : IInstruction {
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            var description = DelegateLibrary.Delegate6s[vm.ReadUshort()].Data;
            if (description.ReturnType != null) {
                IInstruction.AppendVariable(builder, vm);
                builder.Append(" = ");
                builder.Append(description.DeclaringType.Build());
                builder.Append('.');
                builder.Append(description.MethodName);
                builder.Append('(');
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
            else {
                builder.Append(description.DeclaringType.Build());
                builder.Append('.');
                builder.Append(description.MethodName);
                builder.Append('(');
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariableAndComma(builder, vm);
                IInstruction.AppendVariable(builder, vm);
                builder.Append(')');
            }
        }
    }
}