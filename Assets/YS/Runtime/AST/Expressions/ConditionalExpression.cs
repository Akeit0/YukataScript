using System.Text;
using YS.Instructions;
using YS.VM;

namespace YS.AST.Expressions {
    public class ConditionalExpression:IExpression {
        public IExpression Condition;
        public IExpression Consequent;
        public IExpression Alternative;
        public void ToCode(StringBuilder builder) {
            Condition.ToCode(builder);
            builder.Append('?');
            Consequent.ToCode(builder);
            builder.Append(':');
            Alternative.ToCode(builder);
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context){
            var (b,_) = Condition.Compile(context);
            context.EmitData((ushort) 1);
            context.EmitData(b);

            context.Emit(IfElseInstruction.Id);

            var d = context.EnterToScope();
            Consequent.Compile(target,context);
            context.ExitFromScope(d);

            context.Emit(ElseInstruction.Id);
            d = context.EnterToScope();
            Alternative.Compile(target,context);
            context.ExitFromScope(d);
            
        }

        public (ushort id,object obj) Compile( CompilingContext context){
            var (b,_) =Condition.Compile(context);
            context.EmitData((ushort)1);
            context.EmitData(b);
            
            context.Emit(IfElseInstruction.Id);
            
            var d = context.EnterToScope();
            var first=Consequent.Compile(context);
            context.ExitFromScope(d);
           
            context.Emit(ElseInstruction.Id);
            d = context.EnterToScope();
            Alternative.Compile(context);
            context.ExitFromScope(d);
            return first;
        }
    }
}