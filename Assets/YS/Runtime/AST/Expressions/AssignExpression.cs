
using System;
using System.Text;
using YS.Lexer;
using YS.Modules;
using YS.Instructions;
using YS.VM;
using YS.Text;
namespace YS.AST.Expressions {
    public class AssignExpression :IExpression{
        public TokenType Type { get; set; }
        
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }
        public void ToCode(StringBuilder builder) {
            builder.Append('(');
            Left.ToCode(builder);
            builder.Append(TokenInfo.ToString(Type));
            Right?.ToCode(builder);
           builder.Append(')');
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            switch (Left) {
                case IdentifierLiteral identifier: {
                    var left= context.Get(identifier.Name) ;
                    if (context.IsConst(left.id)) throw new Exception("const is not allowed to be \"re\"-assigned.");
                    if (Type == TokenType.EqualsToken) {
                        Right.Compile(left,context);
                    }
                    else {
                        var op = Type.ToBinaryOperator();
                        if (op == TokenType.Invalid) throw new Exception($"Instruction {Type} is invalid.");
                        var right= Right.Compile(context).Cast();
                        var binaryOpType = op.ToBinaryOpType();
                        if (!context.TryEmitIntrinsic(binaryOpType, left, left, right)) {
                            var function= (MethodData)(ModuleLibrary.FindBinaryOperator(binaryOpType, left.variable,
                                right.variable).Item1);
                            if (function == null) {
                                throw new Exception(op.ToString()+left.variable.type.ToString()+ (right.variable).type);
                            }
                            context.EmitData(function.Index);
                            context.EmitData(left.id,left.id,right.id);
                            context.Emit(function.InstructionId);
                        }
                      
                    }
                    if (target.id != 0) {
                        context.EmitCopy(target.id ,left.id);
                    }
                    break;
                }
                case IndexerExpression indexerExpression: {
                    var right= Right.Compile(context);
                    if (Type == TokenType.EqualsToken) {
                        indexerExpression.CompileSetter(right.Cast(), context);
                    }
                    if (target.id != 0) {
                        indexerExpression.Compile(target, context);
                    }
                    break;
                }
                case MemberExpression memberExpression: {
                    var right= Right.Compile(context);
                    if (Type == TokenType.EqualsToken) {
                        memberExpression.CompileSetter(right.Cast(), context);
                    }
                    else {
                        var op = Type.ToBinaryOperator();
                        if (op == TokenType.Invalid) throw new Exception($"Instruction {Type} is invalid.");
                        memberExpression.CompileGetAndSet(right.Cast(), context,op);
                        
                    }
                    if (target.id != 0) {
                        memberExpression.Compile(target, context);
                    }
                    break;
                }
            }
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            switch (Left) {
                case IdentifierLiteral identifier: {
                    var left= context.Get(identifier.Name) ;
                    if (context.IsConst(left)) throw new Exception("const is not allowed to be \"re\"-assigned.");
                    if (Type == TokenType.EqualsToken) {
                        Right.Compile(left,context);
                    }
                    else {
                        var op = Type.ToBinaryOperator();
                        if (op == TokenType.Invalid) throw new Exception($"Instruction {Type} is invalid.");
                        var right= Right.Compile(context).Cast();
                        var binaryOpType = op.ToBinaryOpType();
                        if (context.TryEmitIntrinsic(binaryOpType,left, left, right)) return Right.Compile(context);
                        var function = ModuleLibrary.FindBinaryOperator(op, left.variable,
                            right.variable);
                        if (function == null) {
                            throw new Exception(op.ToString() + left.variable.type.ToString() +
                                                right.variable.type);
                        }

                        context.EmitData(function.Index);
                        context.EmitData(left.id, left.id, right.id);
                        context.Emit(function.InstructionId);

                    }
                    return Right.Compile(context);
                    
                }
                case IndexerExpression indexerExpression: {
                    var right= Right.Compile(context).Cast();
                    if (Type == TokenType.EqualsToken) {
                        indexerExpression.CompileSetter(right, context);
                    }
                    return indexerExpression.Compile(context);
                }
                case MemberExpression memberExpression: {
                    var right= Right.Compile(context).Cast();
                    if (Type == TokenType.EqualsToken) {
                        memberExpression.CompileSetter(right, context);
                    }
                    else {
                        var op = Type.ToBinaryOperator();
                        if (op == TokenType.Invalid) throw new Exception($"Instruction {Type} is invalid.");
                        memberExpression.CompileGetAndSet(right, context,op);
                    }
                    return memberExpression.Compile(context);
                }
            }

            throw new Exception(Left.ToString());
        }
    }
}