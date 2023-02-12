using System;
using System.Text;
using YS.Lexer;
using YS.Modules;
using YS.Instructions;
using YS.VM;


namespace YS.AST.Expressions {
    public class UnaryExpression : IExpression
    {
        public TokenType Type { get; set; }
        public IExpression Right { get; set; }

        public void ToCode(StringBuilder builder) {
            builder.Append('(');
            builder.Append(TokenInfo.ToString(Type));
            Right.ToCode(builder);
            builder.Append(')');
        }
        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
           
            var (rightId,rightVariable)= Right.Compile(context).Cast();
            
            var operatorName = Type.ToUnaryOperatorName();
            var function = (MethodData) ModuleLibrary.FindFunctions(rightVariable, operatorName);
           Span<ArgumentData> span = stackalloc ArgumentData[1];
           span[0] = rightId;
           Validator.ValidateReturnAssignment(target, function,span,context);
           
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            Variable variable;
            if (Type==TokenType.ExclamationToken&&Right is BooleanLiteral booleanLiteral) {
                variable = booleanLiteral.Value ? Constants.False : Constants.True;
                return (context.AddVariable(variable),variable );
            }
            if (Right is NumericLiteral numericLiteral) {
                if(Type==TokenType.PlusToken)
                    return Validator.GetCopy(numericLiteral.Compile(context).Cast(),context);
            }
            var (rightId,rightVariable)= Right.Compile(context).Cast();
            var operatorName = Type.ToUnaryOperatorName();
            var function= (MethodData) ModuleLibrary.FindFunctions(rightVariable, operatorName);
          
            Span<ArgumentData> span = stackalloc ArgumentData[1];
            span[0] = rightId;
           return Validator.GetReturnId(function,span,context);
        }
    }
}

