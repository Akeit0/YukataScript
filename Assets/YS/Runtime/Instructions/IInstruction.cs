using System.Text;
using YS.Modules;
using YS.VM;

namespace YS.Instructions {
    public interface IInstruction {
        static IInstruction()
        {
            Instructions = new IInstruction[]
            {
                new DelegateOp(),
                new Delegate1Op(),
                new Delegate2Op(),
                new Delegate3Op(),
                new Delegate4Op(),
                new Delegate5Op(),
                new Delegate6Op(),
                new MethodInfoInvoker(),
                new Copy(),
                new Read8(),
                new Read16(),
                new Read32(),
                new Read64(),
                new CopyObject(),
                new IsNull(),
                new Return(),
                new Break(),
                new BreakIf(),
                new Continue(),
                new InfiniteLoop(),
                new ForEach(),
                new IfInstruction(),
                new IfElseInstruction(),
                new ElseInstruction(),
                new AwaitableVoidPattern(),
                new CustomAwaiter(),
                new IntRangeOp(),
                new IntArithmeticOp(),
                new FloatArithmeticOp(),
                new DoubleArithmeticOp(),
                new Boxing(),
                new IntToFloat(),
                new IntToDouble(),
                new FloatToDouble(),


            };

        }
        public static readonly IInstruction[] Instructions;
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