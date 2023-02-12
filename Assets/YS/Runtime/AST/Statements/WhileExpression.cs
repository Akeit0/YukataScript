using System;
using System.Text;
using YS.Lexer;
using YS.Instructions;
using YS.VM;
namespace YS.AST.Statements {
    public class WhileExpression : IStatement {
        public bool IsNot { get; set; }
        public IExpression Condition { get; set; }
        public BlockStatement Block { get; set; }

        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            builder.Append(IsNot ? "while not " : "while ");
            builder.Append('(');
            Condition.ToCode(builder);
            builder.Append(')');
            Block.ToCode(builder,indentLevel);
            
        }

        public void Compile(CompilingContext context) {
            throw new NotSupportedException();

        }
    }
}