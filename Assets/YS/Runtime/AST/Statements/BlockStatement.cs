

using System;
using System.Collections.Generic;
using System.Text;
using YS.Lexer;
using YS.VM;

namespace YS.AST.Statements {
    public class BlockStatement : IStatement
    {
        public List<IStatement> Statements { get; set; }
        public void ToCode(StringBuilder builder, int indentLevel) {
            
            builder.Append('{');
            foreach (var statement in Statements) {
                statement.ToCode(builder, indentLevel+1);
            }
            IStatement.AppendIndent(builder,indentLevel);
            builder.Append('}');
        }

        public void Compile(CompilingContext context) {
            throw new NotImplementedException();
        }
    }
}