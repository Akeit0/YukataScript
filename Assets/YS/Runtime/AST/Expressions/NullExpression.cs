using System;
using System.Text;
using YS.Lexer;
using YS.Modules;
using YS.VM;

namespace YS.AST.Expressions {
    public class NullExpression:IExpression {
        public static NullExpression Instance=new ();
        public void ToCode(StringBuilder builder) {
            builder.Append("null");
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            if (!target.variable.type.IsValueType) {
                context.EmitCopyObject(target.id,1);
                
            }
            else {
                throw new Exception(target.variable.type.Build() + " cannot be null");
            }
        }
        public (ushort id,object obj) Compile( CompilingContext context) {
            throw new Exception("null");
          
        }
    }
}