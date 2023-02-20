using System.Text;
using YS.AST.Expressions;
using YS.AST.Statements;
using YS.Collections;
using YS.Lexer;
using YS.Instructions;
using YS.VM;
using YS.Text;
namespace YS.AST.Statements {
    public class ForEachStatement : IStatement
    {
        public StringSegment Current{ get; set; }
        public IExpression Enumerable{ get; set; }
        public BlockStatement Consequence { get; set; }

      
        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            builder.Append("for ");
            builder.Append(Current.AsSpan());
            builder.Append(" in ");
            Enumerable.ToCode(builder);
            builder.Append(' ');
            Consequence.ToCode(builder,indentLevel);
        }

        public void Compile(CompilingContext context) {
            context.MoveToNextStatement();
            var (id,variable)=Enumerable.Compile(context).Cast();
            var eId = DelegateLibrary.EnumeratorIdDictionary[variable.type];
            context.EmitData(eId);
            context.EmitData(id);
            var (current,_)= context.AddVariable(DelegateLibrary.Enumerators[eId].GetCurrentType());
           context.EmitData(current);
            context.Emit(VM.Instructions.ForEach);
            var d = context.EnterToScope();
            context.Define(Current,current);
            foreach (var statement in Consequence.Statements) {
                statement.Compile(context);
            }
            context.ExitFromScope(d);
        }
    }
}