using System.Text;
using System.Text.RegularExpressions;
using YS.Lexer;
using YS.VM;

namespace YS.AST.Expressions {
    public class StringLiteral: IExpression {
        public string Value { get; set; }

        public StringLiteral(string value) {
            Value = value;
        }
        public void ToCode(StringBuilder builder) {
           builder.Append('"');
           builder.Append(Value);
           builder.Append('"');
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context){
            var (targetId, targetVariable) = target;
            if (targetVariable is Variable<string>) {
                var src=context.AddVariable(new Variable<string>(Value));
                context.EmitCopyObject(targetId,src);
            }else 
                throw new System.NotImplementedException();
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            var variable = Value.Contains('\\')
                ? new Variable<string>(Regex.Unescape(Value))
                : new Variable<string>(Value);
            return (context.AddVariable(variable),variable);
        }
    }
}