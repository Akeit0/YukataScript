using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.Instructions;
using YS.VM;

namespace YS.AST.Expressions {
    public class NewExpression : IExpression {
        public IType Type { get; set; }
        public IExpression Arguments { get; set; }

        public NewExpression(IType type, IExpression arguments) {
            Type = type;
            Arguments = arguments;
        }

        public void ToCode(StringBuilder builder) {

            builder.Append("new ");
            builder.Append(Type.ToString());
            builder.Append('(');
            Arguments?.ToCode(builder);
            builder.Append(')');
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            var targetId = target.id;
            var type = Type.GetType(context);
            if (target.variable.type != type) {
                throw new Exception("type does not match");
            }

            var o = ModuleLibrary.FindFunctions(type, ".ctor");
            var list = new AddableSpan<ArgumentData>(stackalloc ArgumentData[6]);

            if (Arguments is not null) {
                if (Arguments is ListedExpression listedExpression)
                    foreach (var e in listedExpression)
                        list.Add(e.Compile(context));
                else
                    list.Add(Arguments.Compile(context).id);
            }

            if (o is List<MethodData> functions) {
                var validFunction = Validator.FindAndValidateFunction(functions, list.AsSpan(), context);
                if (validFunction != null) {
                    context.EmitData(validFunction.Index, target.id);
                    foreach (var arg in list.AsSpan()) {
                        context.EmitData(arg);
                    }

                    foreach (var Ref in validFunction.GetDefaultValuesToSet(list.Count)) {
                        context.EmitData(context.AddVariable(Ref));
                    }

                    context.Emit(validFunction.InstructionId);
                }
            }
            else if (o is MethodData function) {
                if (!Validator.ValidateFunction(function, list.AsSpan(), context)) {
                    throw new Exception();
                }

                if (targetId != 0) {
                    var RefType = target.variable.type;
                    var resultType = function.ReturnType;
                    if (resultType is null) {
                        throw new Exception("return type is void");
                    }

                    if (resultType != RefType) {
                        if (resultType.IsValueType)
                            throw new Exception("return type doesn't match");
                        if (!RefType.IsAssignableFrom(resultType)) {
                            throw new Exception("return type doesn't match");
                        }
                    }

                    context.EmitData(function.Index, targetId);
                    foreach (var arg in list.AsSpan()) {
                        context.EmitData(arg);
                    }

                    foreach (var Ref in function.GetDefaultValuesToSet(list.Count)) {
                        context.EmitData(context.AddVariable(Ref));
                    }

                    context.Emit(function.InstructionId);


                }
                else {

                    context.EmitData(function.Index);
                    if (function.HasReturnValue) {
                        context.EmitData(0);
                    }

                    foreach (var arg in list.AsSpan()) {
                        context.EmitData(arg);
                    }

                    foreach (var Ref in function.GetDefaultValuesToSet(list.Count)) {
                        context.EmitData(context.AddVariable(Ref));
                    }

                    context.Emit(function.InstructionId);


                }

                return;
            }
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            var type = Type.GetType(context);
            var list = new AddableSpan<ArgumentData>(stackalloc ArgumentData[6]);
            var o = ModuleLibrary.FindFunctions(type, ".ctor");
            if (Arguments is not null) {
                if (Arguments is ListedExpression listedExpression)
                    foreach (var e in listedExpression)
                        list.Add(e.Compile(context));
                else
                    list.Add(Arguments.Compile(context).id);
            }

            if (o is List<MethodData> functions) {
                var validFunction = Validator.FindAndValidateFunction(functions, list.AsSpan(), context);
                if (validFunction != null) {
                    var resultVariable = Variable.New(validFunction.ReturnType);
                    var variableAddress = context.AddVariable(resultVariable);
                    context.EmitData(validFunction.Index, variableAddress);
                    foreach (var arg in list.AsSpan()) {
                        context.EmitData(arg);
                    }

                    foreach (var Ref in validFunction.GetDefaultValuesToSet(list.Count)) {
                        context.EmitData(context.AddVariable(Ref));
                    }

                    context.Emit(validFunction.InstructionId);
                    return (variableAddress,resultVariable);
                }
            }
            else if (o is MethodData function) {
                var resultVariable = Variable.New(function.ReturnType);
                var variableAddress = context.AddVariable(resultVariable);
                if (Validator.ValidateFunction(function, list.AsSpan(), context)) {
                    context.EmitData(function.Index, variableAddress);
                    foreach (var arg in list.AsSpan()) {
                        context.EmitData(arg);
                    }

                    foreach (var Ref in function.GetDefaultValuesToSet(list.Count)) {
                        context.EmitData(context.AddVariable(Ref));
                    }
                    context.Emit(function.InstructionId);
                }
                else {
                    throw new Exception();
                }

                return (variableAddress,resultVariable);
            }
            throw new System.NotImplementedException();
        }
    }
}