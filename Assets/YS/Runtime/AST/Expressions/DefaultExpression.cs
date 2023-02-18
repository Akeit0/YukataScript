using System;
using System.Text;
using YS.Lexer;
using YS.VM;

namespace YS.AST.Expressions {
    public class DefaultExpression:IExpression {
        public static DefaultExpression Instance=new ();
        public void ToCode(StringBuilder builder) {
            builder.Append("default");
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            context.EmitCopy(target.id,1);
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            return (1, context.GetVariable(1));
        }
    }
}