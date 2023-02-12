using System;
using System.Text;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.Instructions;
using YS.VM;
using YS.Text;
namespace YS.AST.Expressions {
    public class PreIncrementDecrementExpression:IExpression {
        public bool IsIncrement;
        public StringSegment Identifier;

        public PreIncrementDecrementExpression(bool isIncrement,StringSegment identifier) {
            IsIncrement = isIncrement;
            Identifier = identifier;
        }
        public void ToCode(StringBuilder builder) {
            builder.Append('(');
            builder.Append(IsIncrement ? "++" : "--");
            builder.Append(Identifier.AsSpan());
            builder.Append(')');
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            var (rightId,rightVariable) = context.Get(Identifier);
            var (targetId, targetVariable) = target;
            if ( context.IsConst(rightId)) throw new Exception();
            MethodData function= (MethodData) ModuleLibrary.FindFunctions(rightVariable, IsIncrement?"op_Increment":"op_Decrement");
            if(targetId==0) {
                context.EmitData(function.Index,targetId,rightId);
                context.Emit(function.InstructionId);
                return;
            }
            Span<ArgumentData> span = stackalloc ArgumentData[1];
            span[0] = new ArgumentData(rightId,ArgumentType.Ref);
            Validator.ValidateReturnAssignment(target, function,span,context);
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            var (rightId,rightVariable)  = context.Get(Identifier);
            if ( context.IsConst(rightId)) throw new Exception();
            MethodData function= (MethodData) ModuleLibrary.FindFunctions(rightVariable, IsIncrement?"op_Increment":"op_Decrement");
            Span<ArgumentData> span = stackalloc ArgumentData[1];
            span[0] = new ArgumentData(rightId,ArgumentType.Ref);
           return Validator.GetReturnId(function,span,context);
        }
    }
}