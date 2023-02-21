using System;
using System.Text;
using YS.VM;

namespace YS.Instructions {
    public class AwaitableVoidPattern : IInstruction{
       

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            throw new NotImplementedException();
        }
    }
    public class CustomAwaiter : IInstruction {
      
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
           vm.ReadUshort();
           var num = vm.ReadUshort();
           var variable = vm.Variables[num];
           if(variable .type!=null) {
               builder.Append("var_");
               builder.Append(num);
               builder.Append(" : ");
               builder.Append( variable.type.FullName );
               builder.Append(" = ");
           }
            builder.Append(" await   ");
            IInstruction.AppendVariable(builder, vm);
            
        }
    
    }
}