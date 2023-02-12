using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using YS.AST.Expressions;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.Instructions;
using YS.VM;
using YS.Text;
namespace YS.AST.Expressions {
    public class MemberExpression : IIdentifier
    {
        public IExpression Parent { get; set; }
        public StringSegment Member { get; set; }

       
        public void ToCode(StringBuilder builder) {
            //  builder.Append('(');
            Parent.ToCode(builder);
            builder.Append('.');
            builder.Append(Member.AsSpan());
            //  builder.Append(')');
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            var (id, o) = Parent.Compile(context);
            if (id!=0&&o is Variable variable) {
                if (ModuleLibrary.TryGetGetter(variable, Member, out var getter)) {
                    
                    Validator.ValidateReturnAssignment(target,getter,new ArgumentData(id,ArgumentType.Instance),context);
                }
                return;
            }
            if (o is Type type) {
                if (ModuleLibrary.TryGetMember(type, Member.AsSpan(), out var f)) {
                    switch (f) {
                        case Variable constant when target.variable.type == constant.type:
                            context.EmitCopy(target.id,context.AddVariable(constant));
                            return;
                        case MethodData functionData:
                            Validator.ValidateReturnAssignment(target,functionData,Span<ArgumentData>.Empty, context);
                            return;
                        case List<MethodData> list:
                            foreach (var function in list) {
                                if (function.HasReturnValue) {
                                    Validator.ValidateReturnAssignment(target,function,Span<ArgumentData>.Empty, context);
                                    return;
                                }
                            }
                            return;
                    }
                }
                var getter= (MethodData)ModuleLibrary.FindFunctions(type,"get_"+Member);
                Validator.ValidateReturnAssignment(target,getter,Span<ArgumentData>.Empty, context);
                return;
            }
            
            throw new Exception(Parent.ToString()+" is not found");
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            var (id, o) = Parent.Compile(context);
            if (id!=0&&o is Variable variable) {
                if (ModuleLibrary.TryGetMember(variable.type, Member.AsSpan(), out var f)) {
                    switch (f) {
                        case Variable :
                            return (id, f);
                        case MethodData functionData: {
                            var newVariable = context.AddVariable(functionData.ReturnType);
                            context.EmitData(functionData.Index);
                            context.EmitData(newVariable.id, id);
                            context.Emit(functionData.InstructionId);
                            return newVariable;
                        }
                        case List<MethodData> list:
                            foreach (var function in list) {
                                if (function.HasReturnValue) {
                                    var newVariable = context.AddVariable(function.ReturnType);
                                    context.EmitData(function.Index);
                                    context.EmitData(newVariable.id, id);
                                    context.Emit(function.InstructionId);
                                    return newVariable;
                                }
                            } 
                            throw new KeyNotFoundException(variable.type+Member.ToString()+"setter does not exist");
                    }
                }

                throw new KeyNotFoundException(variable.type+Member.ToString());
            }
            if (o is Type type) {
                if (ModuleLibrary.TryGetMember(type, Member.AsSpan(), out var f)) {
                     switch (f) {
                        case Variable constant:
                            return (context.AddVariable(constant), constant);
                        case MethodData functionData: {
                            var newVariable = context.AddVariable(functionData.ReturnType);
                            context.EmitData(functionData.Index);
                            context.EmitData(newVariable.id);
                            context.Emit(functionData.InstructionId);
                            return newVariable;
                        }
                        case List<MethodData> list:
                            foreach (var function in list) {
                                if (function.HasReturnValue) {
                                    var newVariable = context.AddVariable(function.ReturnType);
                                    context.EmitData(function.Index);
                                    context.EmitData(newVariable.id);
                                    context.Emit(function.InstructionId);
                                    return newVariable;
                                }
                            } 
                            throw new KeyNotFoundException(type+Member.ToString()+"setter does not exist");
                        default: return (0, f);
                    }
                }
                throw new KeyNotFoundException(Member.ToString());
            }

            if (o is NameSpace node) {
                var name = Member.AsSpan();
                var hash = name.GetExHashCode();
                if (node.TryGetTypeOrNameSpace(hash, name, out type, out var node2)) {
                    if (type is not null) {
                        return (0, type);
                    } 
                    if (node2 is not null) {
                        return (0, node2);
                    } 
                }
            }
            throw new Exception();
        }
        public (ushort id,object obj) CompileForFunctionCall( CompilingContext context) {
            var (id, o) = Parent.Compile(context);
            if (id!=0&&o is Variable variable) {
                if (ModuleLibrary.TryGetMember(variable.type, Member.AsSpan(), out var f)) {
                    return (id, f);
                }
                var list = new List<MethodData>();
                if (context.TryGetExFunction(context.GetVariable(id).type, Member.AsSpan(), list)) {
                    return (id, list);
                }
                throw new KeyNotFoundException(variable.type+Member.ToString());
            }
            if (o is Type type) {
                if (ModuleLibrary.TryGetMember(type, Member.AsSpan(), out var f)) {
                    return (0, f);
                }
                throw new KeyNotFoundException(Member.ToString());
            }
            
            throw new Exception();
        }
        
        public void CompileSetter((ushort id,Variable variable) target,  CompilingContext context) {
            var (id, o) = Parent.Compile(context);
            if (id!=0&&o is Variable variable) {
                var parentVariable = variable;
                if(ModuleLibrary.TryGetSetter(parentVariable,Member,out var setter)) {
                    Span<ArgumentData> span = stackalloc ArgumentData[2]
                        {new ArgumentData(id, ArgumentType.Instance), target.id};
                    if (Validator.ValidateFunction(setter, span, context)) {
                        Validator.ValidateReturnAssignment(default, setter, span, context);
                        return;
                    }
                }
                throw new Exception(context.GetVariable(id).type.Build()+" "+Member+" "+target.variable.type.Build());
            }
            if (o is Type type) {
                if (ModuleLibrary.TryGetSetter(type, Member, out var setter)) {
                    Span<ArgumentData> span = stackalloc ArgumentData[1];
                    span[0] = new ArgumentData(target.id);
                    if (Validator.ValidateFunction(setter, span, context)) {
                        Validator.ValidateReturnAssignment(default, setter, span, context);
                        return;
                    }
                }

                throw new Exception();
            }
            throw new Exception();
        }
        public void CompileGetAndSet((ushort id,Variable variable) right,  CompilingContext context,TokenType tokenType) {
            var (id, o) = Parent.Compile(context);
            if (id!=0&&o is Variable variable) {
                var parentVariable = variable;
                if (!ModuleLibrary.TryGetGetterAndSetter(parentVariable, Member, out var getter,out var setter)) {
                    throw new KeyNotFoundException(Member.ToString());
                }
                var getVariable = right.variable.type==getter.ReturnType?right.variable.New():Variable.New(getter.ReturnType);
                var getAddress = context.AddVariable(getVariable);
                var resultVariable = getVariable.New();
                var resultAddress=context.AddVariable(resultVariable);
                context.EmitData(getter.Index);
                context.EmitData(getAddress,id);
                context.Emit(getter.InstructionId);
                var binaryOpType = tokenType.ToBinaryOpType();
                if(!context.TryEmitIntrinsic(binaryOpType,(resultAddress,resultVariable),(getAddress,getVariable),right)) {
                    var binaryOp = ModuleLibrary.FindBinaryOperator(tokenType, getVariable, right.variable);
                    context.EmitData(binaryOp.Index);
                    context.EmitData(resultAddress, getAddress, right.id);
                    context.Emit(binaryOp.InstructionId);
                }
                context.EmitData(setter.Index);
                context.EmitData(id,resultAddress);
                context.Emit(setter.InstructionId);
               return;
            } if (o is Type type) {
                if (!ModuleLibrary.TryGetGetterAndSetter(type, Member, out var getter, out var setter)) {
                    throw new KeyNotFoundException(Member.ToString());
                }

                var getVariable=context.AddVariable(getter.ReturnType);
                context.EmitData(getter.Index);
                context.EmitData(getVariable.id);
                context.Emit(getter.InstructionId);
               
                var resultVariable = getVariable.variable.New();
                var resultAddress=context.AddVariable(resultVariable);
                var binaryOpType = tokenType.ToBinaryOpType();
                if (!context.TryEmitIntrinsic(binaryOpType, (resultAddress, resultVariable), getVariable, right)) {
                    var binaryOp = ModuleLibrary.FindBinaryOperator(tokenType, getVariable.variable, right.variable);
                    Validator.ValidateReturnAssignment((resultAddress, resultVariable), binaryOp, new ArgumentData(getVariable.id), context);
                }
                context.EmitData(setter.Index);
                context.EmitData(resultAddress);
                context.Emit(setter.InstructionId);
                throw new Exception();
            }

            throw new Exception();
        }

      

        public (ushort,Variable) AddTypeVariable(CompilingContext context) {
            return context.AddVariable(context.GetType(Member));
        }

        public override string ToString() {
            return Parent.ToString() + "." + Member.ToString();
           
        }
    }
}