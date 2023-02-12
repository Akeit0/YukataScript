using System.Text;
using YS.Modules;
using YS.VM;

namespace YS.Instructions {
    public interface IInstruction {
        static IInstruction() {
            Instructions[0]= new DelegateOp();
            Instructions[1]= new Delegate1Op();
            Instructions[2]= new Delegate2Op();
            Instructions[3]= new Delegate3Op();
            Instructions[4]= new Delegate4Op();
            Instructions[5]= new Delegate5Op();
            Instructions[6]= new Delegate6Op();
        }
        public static IInstruction[] Instructions=new IInstruction[255] ;
        public static byte Count=7;
        public static void AppendVariable(StringBuilder builder, VirtualMachine vm) {
            var num = vm.ReadUshort();
            var variable = vm.Variables[num];
            builder.Append("var_");
            builder.Append(num);
            builder.Append(" : ");
            builder.Append(variable.type != null ? variable.type.Build() : "null");
        }
        public static void AppendVariable(ushort number,StringBuilder builder, VirtualMachine vm) {
            var variable = vm.Variables[number];
            builder.Append("var_");
            builder.Append(number);
            builder.Append(" : ");
            builder.Append(variable.type != null ? variable.type.Build() : "null");
        }
        
        public static void AppendVariableAndComma(StringBuilder builder, VirtualMachine vm) {
            AppendVariable(builder, vm);
            builder.Append(", ");
        }
        public void Execute(VirtualMachine vm);
        public void ToCode(VirtualMachine vm,StringBuilder builder,ref  int indentLevel);
        public static void AppendIndent(StringBuilder builder, int indentLevel) {
            var length = builder.Length;
            if (length > 0) {
                builder.Append(indents[indentLevel]);
            }
        }        
        public static readonly  string[] indents={"","\t","\t\t","\t\t\t","\t\t\t\t","\t\t\t\t"};
    }


}