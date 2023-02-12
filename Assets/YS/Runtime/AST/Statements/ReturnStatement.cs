using System.Text;

using YS.Lexer;
using YS.Instructions;
using YS.VM;

namespace YS.AST.Statements {
    public class ReturnStatement: IStatement {
        public IExpression ReturnValue { get; set; }
        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            builder.Append("return");
            builder.Append(" ");
            ReturnValue?.ToCode(builder);
            builder.Append(';');
        }

        public void Compile(CompilingContext context) {
            context.MoveToNextStatement();
           context.Emit(Return.Id);
        }
    }
}