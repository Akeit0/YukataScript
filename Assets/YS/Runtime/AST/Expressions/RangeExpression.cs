using System;
using System.Text;
using YS.Instructions;
using YS.VM;

namespace YS.AST.Expressions {
    public class IntRangeExpression:IExpression {
        public IExpression Start { get; }
        public IExpression End { get; }

        public IntRangeExpression(IExpression end) {
            End = end;
        }
        public IntRangeExpression(IExpression start,IExpression end) {
            Start = start;
            End = end;
        }
        
        
        public void ToCode(StringBuilder builder) {
            if(Start!=null)
                Start.ToCode(builder);
            builder.Append("..");
            End.ToCode(builder);
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            if (target.variable is not Variable<IntRange>) {
                throw new Exception("start must be IntRange ");
            }
            if (Start != null) {
                var (startId,startVariable) = Start.Compile(context);
                if (startVariable is not Variable<int>) {
                    throw new Exception("start must be int ");
                }

                var (endId,endVariable) = End.Compile(context);
                if (endVariable is not Variable<int>) {
                    throw new Exception("start must be int ");
                }

                context.EmitData(target.id, startId, endId);
                context.Emit(VM.Instructions.IntRange);
            }
            else {
                var (endId,endVariable) = End.Compile(context);
                if (endVariable is not Variable<int>) {
                    throw new Exception("start must be int ");
                }
                context.EmitData(target.id, 1,endId);
                context.Emit(VM.Instructions.IntRange);
            }
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            var (rangeAddress,rangeVariable) = context.AddVariable<IntRange>();
            if (Start != null) {
                var (startId,startVariable) = Start.Compile(context);
                if (startVariable is not Variable<int>) {
                    throw new Exception("start must be int ");
                }

                var (endId,endVariable)  = End.Compile(context);
                if (endVariable is not Variable<int>) {
                    throw new Exception("start must be int ");
                }

                context.EmitData(rangeAddress, startId, endId);
                context.Emit(VM.Instructions.IntRange);
                return (rangeAddress,rangeVariable) ;
            }
            else {
                var (endId,endVariable)  = End.Compile(context);
                if (endVariable is not Variable<int>) {
                    throw new Exception("start must be int ");
                }
                context.EmitData(rangeAddress, 1,endId);
                context.Emit(VM.Instructions.IntRange);
                return (rangeAddress,rangeVariable);
            }
        }
    }
}