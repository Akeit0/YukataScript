using System;
using System.Text;
using UnityEngine;
using YS.AST.Expressions;
using YS.Async;
using YS.Lexer;
using YS.Instructions;
using YS.Modules;
using YS.VM;

namespace YS.AST.Expressions {
    public class AwaitExpression : IExpression {
        public IExpression Right { get; set; }

        public void ToCode(StringBuilder builder) {
            builder.Append("await");
            builder.Append(' ');
            Right.ToCode(builder);
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            var (t,variable) = Right.Compile(context).Cast();
            var variableType = variable.type;
            if (DelegateLibrary.AwaiterIdDictionary.TryGetValue(variableType, out var id)) {
                context.EmitData(id, target.id, t);
                context.Emit(VM.Instructions.CustomAwaiter);
                return;
            }

            if (ModuleLibrary.TryGetMember(variable,"GetAwaiter", out var o)) {
                var getAwaiter = (MethodData) o;
                var awaiter = context.AddVariable(getAwaiter.ReturnType);
                if (ModuleLibrary.TryGetModule(awaiter.variable, out var module)) {
                    var isCompletedFunction = (MethodData) module.GetMember("IsCompleted");
                    var onCompletedFunction = (MethodData) module.GetMember("OnCompleted");
                    var getResultFunction = (MethodData) module.GetMember("GetResult");
                    var isCompleted = context.AddVariable<bool>();
                    context.Emit(VM.Instructions.AwaitableVoidPattern);
                    context.EmitData(getAwaiter.Index);
                    context.EmitData(awaiter.id,t);
                    context.EmitData(isCompletedFunction.Index);
                    context.EmitData(isCompleted.id);
                    context.EmitData(getResultFunction.Index);
                    context.EmitData(onCompletedFunction.Index);
                    return;
                }
            }
            

            throw new Exception();
        }

        public (ushort id,object obj) Compile( CompilingContext context){
            var (t,variable) = Right.Compile(context).Cast();
            var variableType = variable.type;
            if (DelegateLibrary.AwaiterIdDictionary.TryGetValue(variableType, out var id)) {
                var awaiter = DelegateLibrary.Awaiters[id];
                var result = context.AddVariable(awaiter.ResultType);
                context.EmitData(id, result.id, t);
                context.Emit(VM.Instructions.CustomAwaiter);
                return result;
            }
            throw new Exception();
        }
    }
}