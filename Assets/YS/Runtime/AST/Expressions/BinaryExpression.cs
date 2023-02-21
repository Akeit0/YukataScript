using System;
using System.Collections.Generic;
using System.Text;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.Instructions;
using YS.VM;

namespace YS.AST.Expressions {
    public class BinaryExpression : IExpression
    {
        public TokenType Type { get; set; }
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }

       
        public void ToCode(StringBuilder builder) {
            builder.Append('(');
            Left.ToCode(builder);
            builder.Append(' ');
            builder.Append(TokenInfo.ToString(Type));
            builder.Append(' ');
            Right.ToCode(builder);
            builder.Append(')');
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context){
            if (Type == TokenType.AmpersandAmpersandToken) {
                CompileConditional(true,target, context);
                return;
            }
            if (Type == TokenType.BarBarToken) {
                CompileConditional(false,target, context);
                return;
            }
            
            var left= Left.Compile(context).Cast();
            var right= Right.Compile(context).Cast();
            var isLeftLiteral = Left is NumericLiteral;
            var isRightLiteral = Right is NumericLiteral;

            var binaryOpType = Type.ToBinaryOpType();
            if (context.TryEmitIntrinsic(binaryOpType, target, (left.id,left.variable,isLeftLiteral), (right.id,right.variable,isRightLiteral))) return;
            var (leftResult,rightResult) = ModuleLibrary.FindBinaryOperator(binaryOpType,left.variable,right.variable);
            var arguments = CollectionsUtility.FixedArray(
                new ArgumentData(left.id, isLeftLiteral ? ArgumentType.Literal : 0),
                new ArgumentData(right.id, isRightLiteral ? ArgumentType.Literal : 0)).AsSpan();
            if (leftResult != null) {
                if (leftResult is MethodData ) {
                    var function = (MethodData)leftResult;
                    if (Validator.ValidateFunction(function, arguments, context)) {
                        Validator.ValidateReturnAssignment(target, function, arguments, context);
                        return;
                    }
                }
                if (leftResult is List<MethodData> functions) {
                    var function = Validator.FindAndValidateFunction(functions, arguments, context);
                    if(function!=null) {
                        Validator.ValidateReturnAssignment(target, function, arguments, context);
                        return;
                    }
                }
            }
            if (rightResult != null) {
                if (rightResult is MethodData) {
                    var function = (MethodData)rightResult;
                    if (Validator.ValidateFunction(function, arguments, context)) {
                        Validator.ValidateReturnAssignment(target, function, arguments, context);
                        return;
                    }
                }
                if (leftResult is List<MethodData> functions) {
                    var function = Validator.FindAndValidateFunction(functions, arguments, context);
                    if(function!=null) {
                        Validator.ValidateReturnAssignment(target, function, arguments, context);
                        return;
                    }
                }
            }
            throw new Exception();
        }

        
        
        public (ushort id,object obj) Compile( CompilingContext context) {
            if (Type == TokenType.AmpersandAmpersandToken) {
                return CompileConditional(true, context);
            } if (Type == TokenType.BarBarToken) {
                return CompileConditional(false, context);
            }
            var left= Left.Compile(context).Cast();
            var right= Right.Compile(context).Cast();
            if (context.TryEmitIntrinsic(Type,  left, right,out var result)) return result;
            var function= ModuleLibrary.FindBinaryOperator(Type, left.variable,
                right.variable);
            var resultVariable = Variable.New(function.ReturnType);
           var resultAddress= context.AddVariable(resultVariable);
            context.EmitData(function.Index);
            context.EmitData(resultAddress,left.id,right.id);
            context.Emit(function.InstructionId);
            return (resultAddress,resultVariable);
        }
        
         void CompileConditional(bool isAnd, (ushort,Variable) data, CompilingContext context) {
             Left.Compile(data,context);
            context.EmitData(isAnd?(ushort)1: (ushort)0);
            context.EmitData(data.Item1);
            context.Emit(VM.Instructions.If);
            var d = context.EnterToScope();
            Right.Compile(data, context);
            context.ExitFromScope(d);
        }
         (ushort id,object obj) CompileConditional(bool isAnd,CompilingContext context) {
            var data = context.AddVariable<bool>();
            Left.Compile(data,context);
            context.EmitData(isAnd?(ushort)1: (ushort)0);
            context.EmitData(data.id);
            context.Emit(VM.Instructions.If);
            var d = context.EnterToScope();
            Right.Compile(data, context);
            context.ExitFromScope(d);
            return data;
        }
        
    }
}