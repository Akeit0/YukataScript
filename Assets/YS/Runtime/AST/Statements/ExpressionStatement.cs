using System.Text;
using UnityEngine;
using YS.Lexer;
using YS.VM;

namespace YS.AST.Statements {
    public class ExpressionStatement : IStatement
    {
        public IExpression Expression { get; set; }

        public void ToCode(StringBuilder builder, int indentLevel) {
            if(Expression!=null) {
                IStatement.AppendIndent(builder, indentLevel);
                Expression.ToCode(builder);
                builder.Append(';');
            }
        }

        public void Compile(CompilingContext context) {
            context.MoveToNextStatement();
            Expression.Compile(default,context);
        }
    }
}