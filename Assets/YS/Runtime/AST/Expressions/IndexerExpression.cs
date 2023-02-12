using System;
using System.Collections.Generic;
using System.Text;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.Instructions;
using YS.VM;
using YS.Text;
namespace YS.AST.Expressions {
    public class IndexerExpression: IExpression
    {
        public IExpression Expression { get; set; }
        public IExpression Arguments { get; set; }
        
        public void ToCode(StringBuilder builder) {
            Expression.ToCode(builder);
            builder.Append('[');
            Arguments?.ToCode(builder);
            builder.Append(']');
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            var name = ReadOnlySpan<char>.Empty;
            if (Expression is IdentifierLiteral identifierLiteral) {
                name = identifierLiteral.Name;
            }else if (Expression is KeywordIdentifier keywordIdentifier) {
                name = TokenInfo.ToString(keywordIdentifier.TokenType);
            }
            if (context.TryGet(name, out var id)) {
                var parentVariable = context.GetVariable(id);
                var functions= ModuleLibrary.FindFunctions(parentVariable,"get_Item");
                var list = new AddableSpan<ArgumentData>(stackalloc ArgumentData[6]);
              
                list.Add(new ArgumentData(id,ArgumentType.Instance));
                if (Arguments is ListedExpression listedExpression)
                    foreach (var e in listedExpression)
                        list.Add(e.Compile(context));
                else
                    list.Add(Arguments.Compile(context).id);
               
                var  function = functions as MethodData;
                if (function == null) {
                    function= Validator.FindAndValidateFunction((List<MethodData>) functions, list.AsSpan(), context);
                }
                else {
                    if (!Validator.ValidateFunction(function, list.AsSpan(), context)) {
                        throw new Exception("not match");
                    }
                }

                if (function.ReturnType != target.variable.type) {
                    throw new Exception("not match");
                }
                context.EmitData(function.Index);
                context.EmitData(target.id);
                foreach (var arg in list.AsSpan()) {
                    context.EmitData(arg);
                }
                context.Emit(function.InstructionId);
                return;
            }

            throw new Exception();
        }
        public void CompileSetter((ushort id,Variable variable) target, CompilingContext context) {
            var name = ReadOnlySpan<char>.Empty;
            if (Expression is IdentifierLiteral identifierLiteral) {
                name = identifierLiteral.Name;
            }else if (Expression is KeywordIdentifier keywordIdentifier) {
                name = TokenInfo.ToString(keywordIdentifier.TokenType);
            }
            if (context.TryGet(name, out var id)) {
                var parentVariable = context.GetVariable(id);
                var functions= ModuleLibrary.FindFunctions(parentVariable,"set_Item");
                var list = new AddableSpan<ArgumentData>(stackalloc ArgumentData[6]);
                list.Add(new ArgumentData(id,ArgumentType.Instance));
                if (Arguments is ListedExpression listedExpression)
                    foreach (var e in listedExpression)
                        list.Add(e.Compile(context));
                else
                    list.Add(Arguments.Compile(context).id);
                list.Add(target.id);
                var  function = functions as MethodData;
                if (function == null) {
                    function= Validator.FindAndValidateFunction((List<MethodData>) functions, list.AsSpan(), context);
                }
                else {
                    Validator.ValidateFunction(function, list.AsSpan(), context);
                }
                context.EmitData(function.Index);
                foreach (var arg in list.AsSpan()) {
                    context.EmitData(arg);
                }
                context.Emit(function.InstructionId);
                return;
            }

            throw new Exception();
        }
        public (ushort id,object obj) Compile( CompilingContext context) {
            var name = ReadOnlySpan<char>.Empty;
            if (Expression is IdentifierLiteral identifierLiteral) {
                name = identifierLiteral.Name;
            }else if (Expression is KeywordIdentifier keywordIdentifier) {
                name = TokenInfo.ToString(keywordIdentifier.TokenType);
            }
            if (context.TryGet(name, out var id)) {
                var parentVariable = context.GetVariable(id);
                var functions= ModuleLibrary.FindFunctions(parentVariable,"get_Item");
                var list = new AddableSpan<ArgumentData>(stackalloc ArgumentData[6]);
                list.Add(id);
                if (Arguments is ListedExpression listedExpression)
                    foreach (var e in listedExpression)
                        list.Add(e.Compile(context));
                else
                    list.Add(Arguments.Compile(context).id);
               
                var  function = functions as MethodData;
                if (function == null) {
                    function= Validator.FindAndValidateFunction((List<MethodData>) functions, list.AsSpan(), context);
                }
                else {
                    if (!Validator.ValidateFunction(function, list.AsSpan(), context)) {
                        throw new Exception("not match");
                    }
                }

                var resultAddress = context.AddVariable(function.ReturnType);
                
                context.EmitData(function.Index);
                context.EmitData(resultAddress.id);
                foreach (var arg in list.AsSpan()) {
                    context.EmitData(arg);
                }
                context.Emit(function.InstructionId);
                return resultAddress;
            }

            throw new Exception();
        }
    }
}