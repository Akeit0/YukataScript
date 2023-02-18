using System;
using System.Text;
using YS.Lexer;
using YS.Instructions;
using YS.VM;
namespace YS.AST.Statements {
    public class WhileStatement : IStatement {
        public bool IsNot { get; set; }
        public IExpression Condition { get; set; }
        public BlockStatement Block { get; set; }

        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            builder.Append(IsNot ? "while not " : "while ");
            builder.Append('(');
            Condition.ToCode(builder);
            builder.Append(')');
            Block.ToCode(builder,indentLevel);
            
        }

        public void Compile(CompilingContext context) {
            context.MoveToNextStatement();
            context.Emit(InfiniteLoop.Id);
            var d = context.EnterToScope();
            var (boolean,variable)=Condition.Compile(context).Cast();
            context.EmitData(IsNot ?(ushort)1:(ushort)0);
            context.EmitData(boolean);
            context.Emit(BreakIf.Id);
            foreach (var statement in Block.Statements) {
                
                statement.Compile(context);
            }
            context.ExitFromScope(d);

        }
    }
}