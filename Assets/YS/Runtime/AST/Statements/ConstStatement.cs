

using System.Text;
using YS.AST.Expressions;
using YS.Collections;
using YS.Lexer;
using YS.VM;
using YS.Text;
namespace YS.AST.Statements {
    public class ConstStatement : IStatement {
        public IExpression Type;
        public StringSegment Variable { get; set; }
        public IExpression Value { get; set; }
        
        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            if (Type is not null) {
                Type.ToCode(builder);
            }
            else {
                builder.Append("var");
            }
           
            builder.Append(' ');
            builder.Append(Variable.AsSpan());
            if(Value!=null) {
                builder.Append(" = ");
                Value.ToCode(builder);
            }
            builder.Append(';');
        }

        public bool TryGetBox(out Variable variable) {
            if (Type is KeywordIdentifier keyword) {
                
               return keyword.TokenType.TryGetVariable(out variable);
            }
            variable = null;
            return false;
        }
        public string TypeName {
            get {
                if (Type != null) {
                    var builder = new StringBuilder();
                    Type.ToCode(builder);
                    return builder.ToString();
                }
                return null;
                
            }
        }

        public void Compile(CompilingContext context) {
            context.MoveToNextStatement();
            if (Value is IdentifierLiteral) {
                var id=  Value.Compile(context).Cast();
                context.DefineAsConst(Variable,Validator.GetCopy(id, context).id);
            }
            else if (Value is NumericLiteral or StringLiteral) {
                var (id,_)=  Value.Compile(context);
                context.DefineAsConst(Variable,id);
            }
            else {
                var (id,_)=  Value.Compile(context);
                context.DefineAsConst(Variable,id);
            }
           
        }
    }
}