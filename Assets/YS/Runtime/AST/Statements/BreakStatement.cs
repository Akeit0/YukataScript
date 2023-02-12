using System.Text;
using YS.Instructions;
using YS.VM;

namespace YS.AST.Statements {
    public class BreakStatement: IStatement {
        
        public static BreakStatement Instance { get; } = new BreakStatement();
        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            builder.Append("break;");
        }

        public void Compile(CompilingContext context) {
            context.MoveToNextStatement();
            context.Emit(Break.Id);
        }
    }
}