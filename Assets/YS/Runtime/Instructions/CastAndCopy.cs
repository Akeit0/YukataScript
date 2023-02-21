using System.Text;
using YS.VM;

namespace YS.Instructions {
   
    public sealed class Copy : IInstruction {
       
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
        }

    }
    public sealed class Read16 : IInstruction {
      

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" =  ");
            builder.Append(vm.ReadUshort());
        }

    }
    public sealed class Read32 : IInstruction {
       

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" =  ");
            builder.Append(vm.Read<int>());
        }

    }
   
    
    public sealed class Read64 : IInstruction {
       
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            builder.Append("copy 64bit integer to");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" ");
            builder.Append(vm.Read<long>());
        }
    }
   
    public sealed class Is_null : IInstruction {
       
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" is null ");
        }
    }
    
    public sealed class CopyObject : IInstruction {
        
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = (object)");
            IInstruction.AppendVariable(builder, vm);
        }
    } 
    public sealed class Boxing : IInstruction {
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue(vm.ReadVariable().ToObject());
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = (object) ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
    
    public sealed class NumericCast : IInstruction {
        public void Execute(VirtualMachine vm) {
            throw new System.NotImplementedException();
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            throw new System.NotImplementedException();
        }
    }
    


    public sealed class IntToDouble : IInstruction {
    
        
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            builder.Append("cast int to double : to ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" from ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
    public sealed class IntToFloat : IInstruction {
      
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            builder.Append("cast int to float : to ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" from ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
    public sealed class FloatToDouble : IInstruction {
      
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            builder.Append("cast float to double : to");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" from ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
}