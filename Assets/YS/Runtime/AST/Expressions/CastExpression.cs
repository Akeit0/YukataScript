using System.Text;
using YS.Lexer;
using YS.VM;

namespace YS.AST.Expressions {
    public class CastExpression : IExpression
    {
        public object Type { get; set; }
        public IExpression Value { get; set; }

        public CastExpression(object type, IExpression arg) {
            Type = type;
            Value = arg;
        }

       
        public void ToCode(StringBuilder builder) {
            builder.Append('(');
            if (Type is string str) {
                builder.Append(str);
                
            }else if (Type is IExpression expression) {
                expression.ToCode(builder);
            }
            builder.Append(')');
            Value.ToCode(builder);
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            throw new System.NotImplementedException();
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            throw new System.NotImplementedException();
        }
    }
}