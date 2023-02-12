using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.Instructions;
using YS.VM;
using static YS.AST.Validator;
namespace YS.AST.Expressions {
    
    
    public class CallExpression : IExpression
    {
        
        public IExpression Function { get; set; }
        public IExpression Arguments { get; set; }
        public List<MethodData> TempMethodData;
        public void ToCode(StringBuilder builder) {
           
            Function.ToCode(builder);
            builder.Append('(');
            Arguments?.ToCode(builder);
            builder.Append(')');
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            var (id, func) = Function is MemberExpression memberExpression?memberExpression.CompileForFunctionCall(context): Function.Compile(context);
            if (func == null) {
                throw new Exception(" function not found");
            }
            var list = new AddableSpan<ArgumentData>(stackalloc ArgumentData[6]);
        
          
            if (id != 0) {
                list.Add(new ArgumentData(id,ArgumentType.Instance));
            }
            if (Arguments is not null) {
                if (Arguments is ListedExpression listedExpression)
                    foreach (var e in listedExpression)
                        list.Add(e.Compile(context));
                else
                    list.Add(Arguments.Compile(context).id);
            }
            
            if (func is List<MethodData> functions) {
                var validFunction = FindAndValidateFunction(functions, list.AsSpan(), context);
                if (validFunction != null) {
                    ValidateReturnAssignment(target, validFunction, list.AsSpan(), context);
                    return;
                }
                throw new NullReferenceException(functions[0].MethodName);
            }
            if (func is MethodData function) {
                if (!ValidateFunction(function, list.AsSpan(), context)) {
                    throw new NullReferenceException(function.MethodName);
                }
                ValidateReturnAssignment(target, function, list.AsSpan(), context);
              
                return;
            }

            
        
            
           
        }

        public (ushort id,object obj) Compile( CompilingContext context){
            var (id, func) =  Function is MemberExpression memberExpression?memberExpression.CompileForFunctionCall(context): Function.Compile(context);

            if (func == null) {
                throw new Exception(" function not found");
            }
            var list = new AddableSpan<ArgumentData>(stackalloc ArgumentData[6]);
            if (id != 0) {
                list.Add(new ArgumentData(id,ArgumentType.Instance));
            }
            if (Arguments is not null) {
                if (Arguments is ListedExpression listedExpression)
                    foreach (var e in listedExpression)
                        list.Add(e.Compile(context));
                else
                    list.Add(new ArgumentData(Arguments.Compile(context).id));
            }

            if (func is List<MethodData> functions) {
                var validFunction = FindAndValidateFunction(functions, list.AsSpan(), context);
                if (validFunction != null) {
                    return GetReturnId(validFunction, list.AsSpan(), context);
                }
                throw new Exception(func.GetType()+"not found");
                
            }
            if (func is MethodData function) {
                if (ValidateFunction(function, list.AsSpan(), context)) {
                    return GetReturnId(function, list.AsSpan(), context);
                }
                throw new Exception(func.GetType()+"not found");
                
            }
            
            throw new Exception(func.GetType()+"not found");
        }
    }
}