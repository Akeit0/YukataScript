using System.Text;
using YS.VM;

namespace YS.AST.Statements {
    public class UsingStatement :IStatement {
        public string NameSpace;
        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            builder.Append("using ");
            builder.Append(NameSpace);
            builder.Append(';');
        }

        public void Compile(CompilingContext context) {
            context.MoveToNextStatement();
            context.AddNameSpace(NameSpace);
        }
    }
}