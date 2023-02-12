using System.Text;
using YS.VM;

namespace YS.AST {
    public interface IStatement {
        public void ToCode(StringBuilder builder, int indentLevel);
        public static void AppendIndent(StringBuilder builder, int indentLevel) {
            var length = builder.Length;
            if (length > 0) {
                builder.Append(indents[indentLevel]);
            }
        }        
        public void Compile (CompilingContext context);
        public static readonly  string[] indents={"\n","\n\t","\n\t\t","\n\t\t\t","\n\t\t\t\t","\n\t\t\t\t\t","\n\t\t\t\t\t\t","\n\t\t\t\t\t\t\t","\n\t\t\t\t\t\t\t\t"};
    }
}