using System.Text;
using YS.Lexer;
using YS.Instructions;
using YS.VM;

namespace YS.AST.Statements {
    public class IfStatement : IStatement {
        public bool IsNot { get; set; }
        public IExpression Condition { get; set; }
        public BlockStatement IfBlock { get; set; }
        public BlockStatement ElseBlock { get; set; }

        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            builder.Append(IsNot ? "if not " : "if ");
            builder.Append('(');
            Condition.ToCode(builder);
            builder.Append(')');
            IfBlock.ToCode(builder,indentLevel);
            if (ElseBlock is not null)
            {
                IStatement.AppendIndent(builder,indentLevel);
                builder.Append("else ");
                ElseBlock.ToCode(builder,indentLevel);
            }
        }
        public void Compile(CompilingContext context) {
            context.MoveToNextStatement();
            
            var (b,_)=Condition.Compile(context);
            context.EmitData(IsNot ?(ushort)0:(ushort)1);
            context.EmitData(b);
            
            context.Emit(ElseBlock==null? VM.Instructions.If:VM.Instructions.IfElse);
            
            var d = context.EnterToScope();
            foreach (var statement in IfBlock.Statements) {
               statement.Compile(context);
            }
            context.ExitFromScope(d);
            if (ElseBlock != null)
            {
                context.Emit(VM.Instructions.Else);
                d = context.EnterToScope();
                foreach (var statement in ElseBlock.Statements) {
                    statement.Compile(context);
                }
                context.ExitFromScope(d);
            }

        }
    }
}