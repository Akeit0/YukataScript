using System.Text;
using YS.AST.Statements;
using YS.Lexer;
using YS.VM;

namespace YS.AST.Statements {
    public class AsyncStatement: IStatement  {
        public BlockStatement Consequence { get; set; }

       

        public void ToCode(StringBuilder builder ,int indentLevel)
        {
            IStatement.AppendIndent(builder,indentLevel);
            builder.Append("async ");
           Consequence.ToCode(builder,indentLevel);
         
        }
        public string TokenLiteral() => "async";
        public void Compile(CompilingContext context) {
            throw new System.NotImplementedException();
        }
    }
}