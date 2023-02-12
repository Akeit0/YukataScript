using System;
using System.Collections.Generic;
using UnityEngine;
using YS.AST.Expressions;
using YS.Collections;
using YS.Modules;
using YS.Instructions;
using YS.VM;

namespace YS.AST {
    public enum ArgumentType:byte {
        Value,
        Instance,
        Literal,
        Ref,
        In,
        Out,
        Null,
        Default,
    }

    public struct ArgumentData {
        public ushort Id;
        public ArgumentType ArgumentType;

        public static implicit operator ArgumentData(ushort id) => new ArgumentData(id);
        public ArgumentData(ushort id) {
            Id = id;
            ArgumentType = 0;
        }public ArgumentData(ushort id,ArgumentType argumentType) {
            Id = id;
            ArgumentType = argumentType;
        }
    }
    public enum AssignmentType:byte {
       Invalid,
       Boxing,
       IntToDouble,
       IntToDoubleLiteral,
       IntToFloat,
       IntToFloatLiteral,
       FloatToDouble,
       FloatToDoubleLiteral,
       

    }
    
    public static class Validator {

        public static (ushort id, Variable variable) Cast(this (ushort id, object o) src) =>
            new(src.id, (Variable) src.o);

        public static ArgumentData Compile(IExpression arg, CompilingContext context) {
            ushort id;
            switch (arg) {
                case OutVarNameExpression outVarNameExpression:
                    id = context.AddVariable(Variable.MustBeZero);
                    context.Define(outVarNameExpression.Name,id);
                    return new ArgumentData(id, ArgumentType.Out);
                case NullExpression:
                    return new ArgumentData(1, ArgumentType.Null);
                case DefaultExpression:
                    return new ArgumentData(1, ArgumentType.Default);
            }
            if (arg is NumericLiteral numericLiteral) {
                (id, _) = numericLiteral.Compile(context);
                return new ArgumentData(id,ArgumentType.Literal);
            }
            return new ArgumentData(arg.Compile(context).id);
        }

        public static ArgumentData Compile(this (PassType, IExpression) arg,CompilingContext context) {
            ushort id;
            switch (arg.Item2) {
                case OutVarNameExpression outVarNameExpression:
                    id = context.AddVariable(Variable.MustBeZero);
                    context.Define(outVarNameExpression.Name,id);
                    return new ArgumentData(id, ArgumentType.Out);
                case NullExpression:
                    return new ArgumentData(1, ArgumentType.Null);
                case DefaultExpression:
                    return new ArgumentData(1, ArgumentType.Default);
            }

            var item2 = arg.Item2;
            if (item2 is NumericLiteral numericLiteral) {
                (id, _) = numericLiteral.Compile(context);
                 return new ArgumentData(id,ArgumentType.Literal);
            }
            (id, _)= item2.Compile(context);
            switch (arg.Item1) {
                case 0: return new ArgumentData(id);
                case PassType.In:return new ArgumentData(id, ArgumentType.In);
                case PassType.Ref: {
                    if (context.IsConst(id)) throw new Exception("const ref is not allowed");
                    return new ArgumentData(id, ArgumentType.Ref);
                }
                case PassType.Out: {
                    if (context.IsConst(id)) throw new Exception("const out is not allowed");
                    return new ArgumentData(id, ArgumentType.Out);
                }
            }

            throw new Exception(nameof(arg.Item1));
        }

        public static (ushort id, Variable variable) GetCopy((ushort id, Variable variable) src, CompilingContext context) {
            var copied = src.variable.New();
            var copy=context.AddVariable(copied);
            context.EmitCopy(copy,src.id);
            return (copy,copied);
        }
        public static int AssignmentScore(Type target, Type src) {
            if(target==src)return 10;
            if(target==typeof(Variable))return 9;
            if (src.IsValueType) {
                if(target.IsPrimitive) {
                    if (target == typeof(float) && (src == typeof(int))) return 4;
                    if (target == typeof(double)) {
                        if (src == typeof(int)) return 2;
                        if(src == typeof(float))return 3;
                    }  
                    return 0;
                }
                if (target.IsAssignableFrom(src)) return 1;
            }else {
                if (target.IsAssignableFrom(src)) return 8 ;
            }
            return 0;
            
        }
        
        static int AssignmentScore(ParamDescription target, Type src,ArgumentType argumentType) {
            if(target.PassType == PassType.Value) {
                return argumentType switch {
                    ArgumentType.Default => 6,
                    ArgumentType.Null => target.Type.IsValueType ? 0 : 6,
                    ArgumentType.Ref or ArgumentType.In or ArgumentType.Out => 0,
                    _ => AssignmentScore(target.Type, src)
                };
            }
            if (target.PassType == PassType.Out) {
                if (argumentType != ArgumentType.Out) return 0;
                if (src is null) return 7;
                return (target.Type == src)?10:0;
            }
            
            if (target.Type != src) return 0;
            if (argumentType == ArgumentType.Instance) return 10;
            if (target.PassType == PassType.In) return (argumentType is  ArgumentType.In)?10:0;
            return argumentType==ArgumentType.Ref ? 10 : 0;
        }
        
        public static void ValidateAssignment(ushort target, ushort src, CompilingContext context) {
            if (context.IsConst(target)) {
                throw new Exception("const");
            }
            var targetType = context.GetVariable(target).type;
            var srcType = context.GetVariable(src).type;
            if (targetType == srcType) {
                context.EmitCopy(target,src);
                return;
            }
            if (srcType.IsValueType) {
                if(targetType.IsPrimitive) {
                    if (targetType == typeof(float) && (srcType == typeof(int))) {
                        context.EmitData(target,src);
                        context.Emit(FloatToDouble.Id);
                        return ;
                    }
                    if (targetType == typeof(double)) {
                        if (srcType == typeof(int)) {
                            context.EmitData(target,src);
                            context.Emit(IntToDouble.Id);
                            return ;
                        }
                        if(srcType == typeof(float)){
                            context.EmitData(target,src);
                            context.Emit(FloatToDouble.Id);
                            return ;
                        }
                    }  
                    throw new Exception(targetType.Build()+srcType.Build());
                }
                if (targetType.IsAssignableFrom(srcType)) {
                    var newSrc= context.AddBoxed(src);
                    context.EmitCopyObject(target,newSrc);
                    return;
                }
            }else {
                if (targetType.IsAssignableFrom(srcType)) {
                    context.EmitCopyObject(target,src);
                    return;
                }
            }
            throw new Exception();
        }public static void ValidateAssignment((ushort id,Variable variable) target, (ushort id,Variable variable) src, CompilingContext context) {
            if (context.IsConst(target)) {
                throw new Exception("const");
            }
            var targetType =target.variable.type;
            var srcType = src.variable.type;
            if (targetType == srcType) {
                context.EmitCopy(target.id,src.id);
                return;
            }
            if (srcType.IsValueType) {
                if(targetType.IsPrimitive) {
                    if (targetType == typeof(float) && (srcType == typeof(int))) {
                        context.EmitData(target.id,src.id);
                        context.Emit(FloatToDouble.Id);
                        return ;
                    }
                    if (targetType == typeof(double)) {
                        if (srcType == typeof(int)) {
                            context.EmitData(target.id,src.id);
                            context.Emit(IntToDouble.Id);
                            return ;
                        }
                        if(srcType == typeof(float)){
                            context.EmitData(target.id,src.id);
                            context.Emit(FloatToDouble.Id);
                            return ;
                        }
                    }  
                    throw new Exception(targetType.Build()+srcType.Build());
                }
                if (targetType.IsAssignableFrom(srcType)) {
                    var newSrc= context.AddBoxed(src.id);
                    context.EmitCopyObject(target.id,newSrc);
                    return;
                }
            }else {
                if (targetType.IsAssignableFrom(srcType)) {
                    context.EmitCopyObject(target.id,src.id);
                    return;
                }
            }
            throw new Exception();
        }

        public static  (ushort id ,Variable variable) GetReturnId(MethodData function, Span<ArgumentData> arguments,
            CompilingContext context) {
            var resultVariable = Variable.New(function.ReturnType);
            var VariableAddress= context.AddVariable(resultVariable);
            context.EmitData(function.Index,VariableAddress);
            foreach (var arg in arguments) {
                context.EmitData(arg);
            }
            foreach (var Variable in function.GetDefaultValuesToSet(arguments.Length)) {
                context.EmitData(context.AddVariable(Variable));
            }
            context.Emit(function.InstructionId);
            return (VariableAddress, resultVariable);
        }

        public static void ValidateReturnAssignment( (ushort id ,Variable variable) target, MethodData function, ArgumentData argument,
            CompilingContext context) {
            Span<ArgumentData> span = stackalloc ArgumentData[1];
            span[0] = argument;
             ValidateReturnAssignment(target, function, span, context);
        }

        public static void ValidateReturnAssignment( (ushort id ,Variable variable) target, MethodData function, Span<ArgumentData> arguments,
            CompilingContext context) {
           
            var methodDescription = function;
            var srcType = methodDescription.ReturnType;
            if (target.id == 0) {
                context.EmitData(function.Index);
                if(srcType is not null) context.EmitData(0);
                foreach (var arg in arguments) {
                    context.EmitData(arg);
                }
                foreach (var Variable in function.GetDefaultValuesToSet(arguments.Length)) {
                    context.EmitData(context.AddVariable(Variable));
                }
                context.Emit(function.InstructionId);
                return;
            }
            if (context.IsConst(target)) {
                throw new Exception("const");
            }
            var targetType = target.variable.type;
            var score = AssignmentScore(targetType, srcType);
            if (8 <= score) {
                context.EmitData(function.Index);
                context.EmitData(target.id);
                foreach (var arg in arguments) {
                    context.EmitData(arg);
                }
                foreach (var Variable in function.GetDefaultValuesToSet(arguments.Length)) {
                    context.EmitData(context.AddVariable(Variable));
                }

                context.Emit(function.InstructionId);
                return;
            }

            var (result,_) = context.AddVariable(srcType);
            context.EmitData(function.Index);
            context.EmitData(result);
            foreach (var arg in arguments) {
                context.EmitData(arg);
            }

            foreach (var Variable in function.GetDefaultValuesToSet(arguments.Length)) {
                context.EmitData(context.AddVariable(Variable));
            }

            context.Emit(function.InstructionId);
            switch (score) {
                case 1: {
                    context.EmitCopy(target.id, context.AddBoxed(result));
                    return;
                }
                case 2: {
                    var newValueId = context.AddVariable<double>().id;
                    context.EmitData(newValueId, result);
                    context.Emit(IntToDouble.Id);
                    context.EmitCopy(target.id, newValueId);
                    return;
                }
                case 3: {
                    var newValueId = context.AddVariable<double>().id;
                    context.EmitData(newValueId, result);
                    context.Emit(FloatToDouble.Id);
                    context.EmitCopy(target.id, newValueId);
                    return;
                }
                case 4: {
                    var newValueId = context.AddVariable<float>().id;
                    context.EmitData(newValueId, result);
                    context.Emit(IntToFloat.Id);
                    context.EmitCopy(target.id, newValueId);
                    return;
                }
            }
        }
        

    static MethodData FindAndValidateFunction(List<MethodData> functions) {
        foreach (var function in functions) {
            var methodDescription = function;
            if (methodDescription.DefaultValueCount == methodDescription.ParameterCount) {
                return function;
            }
        }
        return null;
    }
    public  static MethodData FindAndValidateFunction(List<MethodData> functions,Span<ArgumentData> arguments, CompilingContext context) {
        if (arguments.Length == 0) return FindAndValidateFunction(functions);
            using var type = new ValueList<Type>(arguments.Length);
            Span<int> bestScores = stackalloc int[arguments.Length];
            Span<int> tempScores = stackalloc int[arguments.Length];
            foreach (var arg in arguments) {
                type.Add(context.GetVariable(arg.Id).type);
            }
            var typeSpan = type.AsSpan();
            var bestIndex = -1;
            var ambiguousMatch = false;
            var firstType = arguments[0].ArgumentType;
            for (var index = 0; index < functions.Count; index++) {
                var function = functions[index];
                var methodDescription = function;
                if (methodDescription.IsArgumentCountValidWith(arguments.Length)) {
                    switch (methodDescription.MethodType) {
                        case MethodType.Instance when firstType!=ArgumentType.Instance:
                        case MethodType.Static when firstType==ArgumentType.Instance:
                            goto Next;
                    }
                    var targetArray = methodDescription.ParamData;
                    if (methodDescription.MethodType == MethodType.Instance) {
                        int score = AssignmentScore(methodDescription.DeclaringType, typeSpan[0]);
                        if (score == 0) goto Next;
                        for (int i = 1; i < arguments.Length; i++) {
                            score = AssignmentScore(targetArray[i-1], typeSpan[i],arguments[i].ArgumentType);
                            if (score == 0) goto Next;
                           
                            tempScores[i] = score;
                        }
                    }
                    else{
                        for (int i = 0; i < arguments.Length; i++) {
                            var score = AssignmentScore(targetArray[i], typeSpan[i],arguments[i].ArgumentType);
                            if (score == 0) goto Next;
                           
                            tempScores[i] = score;
                        }
                    }
                    for (int i = 0; i < arguments.Length; i++) {
                        var b = bestScores[i];
                        var t = tempScores[i];
                        if(t<b) goto Next;
                        if (b == t) continue;
                        ambiguousMatch = false;
                        bestIndex = index;
                        
                        tempScores.CopyTo(bestScores);
                        goto Next;
                    }
                    ambiguousMatch = true;
                }
                Next: ;
            }
        
            if (bestIndex < 0) return null;
            if (ambiguousMatch) throw new Exception("AmbiguousMatch");
            var bestFunction = functions[bestIndex];
          var isInstanceMethod=    bestFunction.MethodType==MethodType.Instance;
            for (int i = 0; i < arguments.Length; i++) {
                var bestScore = bestScores[i];
                switch (bestScore) {
                    case 1: {
                        var newValue = context.AddBoxed(arguments[i].Id);
                        arguments[i] = newValue;
                        break;
                    }
                    case 2: {
                       
                        var newValueId = context.AddVariable<double>().id;
                        context.EmitData(newValueId,arguments[i].Id);
                        arguments[i] = newValueId;
                        context.Emit(IntToDouble.Id);
                        break;
                    }
                    case 3: {
                        var newValueId = context.AddVariable<double>().id;
                        context.EmitData(newValueId, arguments[i].Id);
                        arguments[i] = newValueId;
                        context.Emit(FloatToDouble.Id);
                        break;
                    }
                    case 4: {
                        var argument = arguments[i];
                        if (argument.ArgumentType==ArgumentType.Literal) {
                           var current= context.GetVariable(argument.Id).As<int>();
                           var newVariable = new Variable<float>(current);
                           context.Set(argument.Id,newVariable);
                        }
                        else {
                            var newValueId = context.AddVariable<float>().id;
                            context.EmitData(newValueId, argument.Id);
                            arguments[i] = newValueId;
                            context.Emit(IntToFloat.Id);
                        }
                        break;
                    }
                    case 7: {
                       context.Engine.Variables[arguments[i].Id]=Variable.New(bestFunction.ParamData[isInstanceMethod?i-1:i].Type);
                       break;
                   }
                }
            }
            
            return bestFunction;
        }
        public static bool ValidateFunction(MethodData function,Span<ArgumentData> arguments,   CompilingContext context) {
            var methodDescription = function;
            if (!methodDescription.IsArgumentCountValidWith(arguments.Length)) return false;
            switch (methodDescription.MethodType) {
                case MethodType.Instance when arguments[0].ArgumentType!=ArgumentType.Instance:
                case MethodType.Static when arguments[0].ArgumentType==ArgumentType.Instance:
                    return false;
            }
            using var type = new ValueList<Type>(arguments.Length);
            Span<int> scores = stackalloc int[arguments.Length];
            foreach (var arg in arguments) {
                type.Add(context.GetVariable(arg.Id).type);
            }
            var typeSpan = type.AsSpan();
            var targetArray = methodDescription.ParamData;
            if (methodDescription.MethodType == MethodType.Instance) {
                int score = AssignmentScore(methodDescription.DeclaringType, typeSpan[0]);
                if (score == 0)return false;
                for (int i = 1; i < arguments.Length; i++) {
                    score = AssignmentScore(targetArray[i-1], typeSpan[i],arguments[i].ArgumentType);
                    if (score == 0) return false;
                           
                    scores[i] = score;
                }
            }
            else{
                for (int i = 0; i < arguments.Length; i++) {
                    var score = AssignmentScore(targetArray[i], typeSpan[i],arguments[i].ArgumentType);
                    if (score == 0)return false;
                           
                    scores[i] = score;
                }
            }
            for (int i = 0; i < arguments.Length; i++) {
                var bestScore = scores[i];
                switch (bestScore) {
                    case 1: {
                        var newValue = context.AddBoxed(arguments[i].Id);
                        arguments[i] = newValue;
                        break;
                    }
                    case 2: {
                        var newValueId = context.AddVariable<double>().id;
                        context.EmitData(newValueId,arguments[i].Id);
                        arguments[i] = newValueId;
                        context.Emit(IntToDouble.Id);
                        break;
                    }
                    case 3: {
                        var newValueId = context.AddVariable<double>().id;
                        context.EmitData(newValueId,arguments[i].Id);
                        arguments[i] = newValueId;
                        context.Emit(FloatToDouble.Id);
                        break;
                    }
                    case 4: {
                        var argument = arguments[i];
                        if (argument.ArgumentType==ArgumentType.Literal) {
                            var current= context.GetVariable(argument.Id).As<int>();
                            if (current == 0) {
                                var newVariable = Constants.FloatZero;
                                context.Set(argument.Id,newVariable);
                            }
                            else {
                                var newVariable = new Variable<float>(current);
                                context.Set(argument.Id,newVariable);
                            }
                        }
                        else {
                            var newValueId = context.AddVariable<float>().id;
                            context.EmitData(newValueId, argument.Id);
                            arguments[i] = newValueId;
                            context.Emit(IntToFloat.Id);
                        }
                        break;
                    }
                    case 7: {
                        context.Engine.Variables[arguments[i].Id]=Variable.New(function.ParamData[i].Type);
                        break;
                    }
                }
            }
            return true;
        }
    }
}