using System.Text;
using YS.Instructions;
using YS.VM;

namespace YS.AST.Statements {
    public class ContinueStatement: IStatement {
        public static ContinueStatement Instance { get; } = new ContinueStatement();
        
        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            builder.Append("continue;");
        }

        public void Compile(CompilingContext context) {
            context.MoveToNextStatement();
            context.Emit(Continue.Id);
        }
    }
}