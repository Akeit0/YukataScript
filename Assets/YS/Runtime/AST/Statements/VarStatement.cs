


using System.Text;
using YS.AST.Expressions;
using YS.Collections;
using YS.Lexer;
using YS.VM;
using YS.Text;
namespace YS.AST.Statements {
    public class VarStatement : IStatement {
        public IType Type;
        public StringSegment Variable { get; set; }
        public IExpression Value { get; set; }
        
        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            if (Type is not null) {
                builder.Append(Type.ToString());
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


        public void Compile(CompilingContext context) {
            context.MoveToNextStatement();
            if (Type != null) {
                var target = Type.AddVariable(context);
                if(Value!=null)
                    Value.Compile(target,context);
                context.Define(Variable,target.id);
                return;
            }
            if (Value is IdentifierLiteral) {
                var id=  Value.Compile(context).Cast();
                context.Define(Variable,Validator.GetCopy(id, context).id);
            }
            if (Value is NumericLiteral numericLiteral) {
                context.Define(Variable,numericLiteral.AddVariable(context));
            }else if (Value is StringLiteral stringLiteral) {
                var (id,_) = stringLiteral.Compile(context);
                var target = context.AddVariable(new Variable<string>());
                context.EmitCopyObject(target,id);
                context.Define(Variable,target);
            }
            else {
                var (id,_)=  Value.Compile(context);
                context.Define(Variable,id);
            }
         
        }
    } 
}