
using System;
using System.Text;
using YS.Lexer;
using YS.VM;

namespace YS.AST.Expressions {
    public class BooleanLiteral : IExpression {
        public bool Value { get; set; }
        public static readonly string T = "true";
        public static readonly string F = "false";

        public static BooleanLiteral True = new BooleanLiteral() {Value = true};
        public static BooleanLiteral False = new BooleanLiteral() {Value = false};
        public void ToCode(StringBuilder builder) {
            builder.Append(Value?T:F);
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context){
            if (target.variable is Variable<bool>) {
               var v= context.AddVariable(Value?Constants.True:Constants.False);
               context.EmitCopy(target.id,v);
               return;
            }
            
            throw new Exception("not bool");
        }
        public (ushort id,object obj) Compile( CompilingContext context) {
            var variable = Value ? Constants.True : Constants.False;
            return (context.AddVariable(variable),variable);
        }
    }
}