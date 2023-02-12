using System;
using System.Reflection;
using System.Text;
using YS.Modules;
using YS.VM;

namespace YS.Instructions {
    public class MethodInfoInvoker:IInstruction {

        [ThreadStatic]
        static object[][] _arguments;

        static object[][] arguments {
            get {
                if (_arguments != null) return _arguments;
                _arguments = new object[6][];
                _arguments[0] = new object[1];
                _arguments[1] = new object[2];
                _arguments[2] = new object[3];
                _arguments[3] = new object[4];
                _arguments[4] = new object[5];
                _arguments[5] = new object[6];
                return _arguments;
            }
        }
        

        static object[] GetArray(object element) {
            var array = arguments[0];
            array[0] = element;
            return array;
        }
        static object[] GetArray(object element1,object element2) {
            var array = arguments[1];
            array[0] = element1;
            array[1] = element2;
            return array;
        }
        static object[] GetArray(object element1,object element2,object element3) {
            var array = arguments[2];
            array[0] = element1;
            array[1] = element2;
            array[2] = element3;
            return array;
        }
        static object[] GetArray(object element1,object element2,object element3,object element4) {
            var array = arguments[3];
            array[0] = element1;
            array[1] = element2;
            array[2] = element3;
            array[3] = element4;
            return array;
        }
        static object[] GetArray(object element1,object element2,object element3,object element4,object element5) {
            var array = arguments[4];
            array[0] = element1;
            array[1] = element2;
            array[2] = element3;
            array[3] = element4;
            array[4] = element5;
            return array;
        }
        
        
        
        public static readonly byte Id = IInstruction.Count++;
        static MethodInfoInvoker (){
            IInstruction.Instructions[Id]= new MethodInfoInvoker();
        }
        public void Execute(VirtualMachine vm) {
            var data = DelegateLibrary.MethodInfos[vm.ReadUshort()];
            if (data.Data.HasReturnValue)
                CallFunc(vm, data);
            else  
                CallAction(vm,data);
            
        }

        static void CallAction(VirtualMachine vm, (MethodInfo MethodInfo,MethodData Data ) data) {
            var description = data.Data;
            if (description.MethodType == MethodType.Instance) {
                switch (description.ParameterCount) {
                    case 0: {
                        var instanceVariable = vm.ReadVariable();
                        var instance = instanceVariable.ToObject();
                        data.MethodInfo.Invoke(vm.ReadObject(), null);
                        if ((description.DeclaringType.IsValueType)) {
                            instanceVariable.SetObject(instance);
                        }
                        return;
                    }
                    case 1: {
                        var instanceVariable = vm.ReadVariable();
                        var instance = instanceVariable.ToObject();
                        var variable = vm.ReadVariable();
                        var array = GetArray(variable.ToObject());
                        data.MethodInfo.Invoke(instance, array);
                        if ((description.DeclaringType.IsValueType)) {
                            instanceVariable.SetObject(instance);
                        }
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable.SetObject(array[0]);
                        }
                        return;
                    }
                    case 2:{
                        var instanceVariable = vm.ReadVariable();
                        var instance = instanceVariable.ToObject();
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(),variable1.ToObject());
                        data.MethodInfo.Invoke(instance, array);
                        if ((description.DeclaringType.IsValueType)) {
                            instanceVariable.SetObject(instance);
                        }
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }
                        return;
                    }
                    case 3:{
                        var instanceVariable = vm.ReadVariable();
                        var instance = instanceVariable.ToObject();
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var variable2 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(),variable1.ToObject(),variable2.ToObject());
                        data.MethodInfo.Invoke(instance, array);
                        if ((description.DeclaringType.IsValueType)) {
                            instanceVariable.SetObject(instance);
                        }
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }if ((description.ParamData[2].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[2]);
                        }
                        return;
                    }
                    case 4:{
                        var instanceVariable = vm.ReadVariable();
                        var instance = instanceVariable.ToObject();
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var variable2 = vm.ReadVariable();
                        var variable3 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(),variable1.ToObject(),variable2.ToObject(),variable3.ToObject());
                        data.MethodInfo.Invoke(instance, array);
                        if ((description.DeclaringType.IsValueType)) {
                            instanceVariable.SetObject(instance);
                        }
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }if ((description.ParamData[2].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[2]);
                        }if ((description.ParamData[3].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[3]);
                        }
                        return;
                    }
                }
            }
            else {
                switch (description.ParameterCount) {
                    case 0:
                        data.MethodInfo.Invoke(null, null);
                        return;
                    case 1: {
                        var variable = vm.ReadVariable();
                        var array = GetArray(variable.ToObject());
                        data.MethodInfo.Invoke(null, array);
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable.SetObject(array[0]);
                        }

                        return;
                    }
                    case 2: {
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(), variable1.ToObject());
                        data.MethodInfo.Invoke(null, array);
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }

                        if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }

                        return;
                    }
                    case 3: {
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var variable2 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(), variable1.ToObject(), variable2.ToObject());
                        data.MethodInfo.Invoke(null, array);
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }

                        if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }

                        if ((description.ParamData[2].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[2]);
                        }

                        return;
                    }
                    case 4: {
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var variable2 = vm.ReadVariable();
                        var variable3 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(), variable1.ToObject(), variable2.ToObject(),
                            variable3.ToObject());
                        data.MethodInfo.Invoke(null, array);
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }

                        if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }

                        if ((description.ParamData[2].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[2]);
                        }

                        if ((description.ParamData[3].PassType is PassType.Ref or PassType.Out)) {
                            variable3.SetObject(array[3]);
                        }

                        return;
                    }
                    case 5: {
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var variable2 = vm.ReadVariable();
                        var variable3 = vm.ReadVariable();
                        var variable4 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(), variable1.ToObject(), variable2.ToObject(),
                            variable3.ToObject(), variable4.ToObject());
                        data.MethodInfo.Invoke(null, array);
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }

                        if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }

                        if ((description.ParamData[2].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[2]);
                        }

                        if ((description.ParamData[3].PassType is PassType.Ref or PassType.Out)) {
                            variable3.SetObject(array[3]);
                        }

                        if ((description.ParamData[4].PassType is PassType.Ref or PassType.Out)) {
                            variable4.SetObject(array[4]);
                        }

                        return;
                    }
                }
            }
        }
        static void CallFunc(VirtualMachine vm, (MethodInfo MethodInfo,MethodData Data ) data) {
            var description = data.Data;
            var result = vm.ReadVariable();
            if (description.MethodType == MethodType.Instance) {
                switch (description.ParameterCount) {
                    case 0: {
                        var instanceVariable = vm.ReadVariable();
                        var instance = instanceVariable.ToObject();
                        result.SetObject(data.MethodInfo.Invoke(instance, null)); 
                        if ((description.DeclaringType.IsValueType)) {
                            instanceVariable.SetObject(instance);
                        }
                        return;
                    }
                    case 1: {
                        var instanceVariable = vm.ReadVariable();
                        var instance = instanceVariable.ToObject();
                        var variable = vm.ReadVariable();
                        var array = GetArray(variable.ToObject());
                        result.SetObject(data.MethodInfo.Invoke(instance, array)); 
                        if ((description.DeclaringType.IsValueType)) {
                            instanceVariable.SetObject(instance);
                        }
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable.SetObject(array[0]);
                        }
                        return;
                    }
                    case 2:{
                        var instanceVariable = vm.ReadVariable();
                        var instance = instanceVariable.ToObject();
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(),variable1.ToObject());
                        result.SetObject(data.MethodInfo.Invoke(instance, array)); 
                        if ((description.DeclaringType.IsValueType)) {
                            instanceVariable.SetObject(instance);
                        }
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }
                        return;
                    }
                    case 3:{
                        var instanceVariable = vm.ReadVariable();
                        var instance = instanceVariable.ToObject();
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var variable2 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(),variable1.ToObject(),variable2.ToObject());
                        result.SetObject(data.MethodInfo.Invoke(instance, array)); 
                        if ((description.DeclaringType.IsValueType)) {
                            instanceVariable.SetObject(instance);
                        }
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }if ((description.ParamData[2].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[2]);
                        }if ((description.ParamData[3].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[3]);
                        }
                        return;
                    }
                    case 4: {
                        var instanceVariable = vm.ReadVariable();
                        var instance = instanceVariable.ToObject();
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var variable2 = vm.ReadVariable();
                        var variable3 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(), variable1.ToObject(), variable2.ToObject(),
                            variable3.ToObject());
                        result.SetObject(data.MethodInfo.Invoke(instance, array)); 
                        if ((description.DeclaringType.IsValueType)) {
                            instanceVariable.SetObject(instance);
                        }
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }if ((description.ParamData[2].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[2]);
                        }if ((description.ParamData[3].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[3]);
                        }
                        if ((description.ParamData[4].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[4]);
                        }

                        return;
                    }
                }
            }
            else {
                switch (description.ParameterCount) {
                    case 0:
                        vm.ReadVariable().SetObject( data.MethodInfo.Invoke(null, null));
                        return;
                    case 1: {
                        var variable = vm.ReadVariable();
                        var array = GetArray(variable.ToObject());
                        result.SetObject(data.MethodInfo.Invoke(null, array));  ;
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable.SetObject(array[0]);
                        }

                        return;
                    }
                    case 2: {
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(), variable1.ToObject());
                        result.SetObject(data.MethodInfo.Invoke(null, array));
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }

                        if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }

                        return;
                    }
                    case 3: {
                   
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var variable2 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(), variable1.ToObject(), variable2.ToObject());
                        result.SetObject( data.MethodInfo.Invoke(null, array));
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }

                        if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }

                        if ((description.ParamData[2].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[2]);
                        }

                        return;
                    }
                    case 4: {
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var variable2 = vm.ReadVariable();
                        var variable3 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(), variable1.ToObject(), variable2.ToObject(),
                            variable3.ToObject());
                        result.SetObject(data.MethodInfo.Invoke(null, array));
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }
                        if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }
                        if ((description.ParamData[2].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[2]);
                        }
                        if ((description.ParamData[3].PassType is PassType.Ref or PassType.Out)) {
                            variable3.SetObject(array[3]);
                        }
                        return;
                    }
                    case 5: {
                        var variable0 = vm.ReadVariable();
                        var variable1 = vm.ReadVariable();
                        var variable2 = vm.ReadVariable();
                        var variable3 = vm.ReadVariable();
                        var variable4 = vm.ReadVariable();
                        var array = GetArray(variable0.ToObject(), variable1.ToObject(), variable2.ToObject(),
                            variable3.ToObject(), variable4.ToObject());
                        result.SetObject(data.MethodInfo.Invoke(null, array)); 
                        if ((description.ParamData[0].PassType is PassType.Ref or PassType.Out)) {
                            variable0.SetObject(array[0]);
                        }

                        if ((description.ParamData[1].PassType is PassType.Ref or PassType.Out)) {
                            variable1.SetObject(array[1]);
                        }

                        if ((description.ParamData[2].PassType is PassType.Ref or PassType.Out)) {
                            variable2.SetObject(array[2]);
                        }

                        if ((description.ParamData[3].PassType is PassType.Ref or PassType.Out)) {
                            variable3.SetObject(array[3]);
                        }

                        if ((description.ParamData[4].PassType is PassType.Ref or PassType.Out)) {
                            variable4.SetObject(array[4]);
                        }

                        return;
                    }
                }
            }
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            switch (vm.ReadUshort()) {
                case 0:   IInstruction.AppendVariable(builder, vm);
                    builder.Append("()");
                    return;
                case 1: IInstruction.AppendVariable(builder, vm);
                    builder.Append('(');
                    IInstruction.AppendVariable(builder, vm);
                    builder.Append(')');
                    return;
                case 2: IInstruction.AppendVariable(builder, vm);
                    builder.Append('(');
                    IInstruction.AppendVariableAndComma(builder, vm);
                    IInstruction.AppendVariable(builder, vm);
                    builder.Append(')');
                    return;
                case 3: IInstruction.AppendVariable(builder, vm);
                    builder.Append('(');
                    IInstruction.AppendVariableAndComma(builder, vm);
                    IInstruction.AppendVariableAndComma(builder, vm);
                    IInstruction.AppendVariable(builder, vm);
                    builder.Append(')');
                    return;
                case 4:IInstruction.AppendVariable(builder, vm);
                    builder.Append('(');
                    IInstruction.AppendVariableAndComma(builder, vm);
                    IInstruction.AppendVariableAndComma(builder, vm);
                    IInstruction.AppendVariableAndComma(builder, vm);
                    IInstruction.AppendVariable(builder, vm);
                    builder.Append(')');
                    return;
                case 5: IInstruction.AppendVariable(builder, vm);
                    builder.Append('(');
                    IInstruction.AppendVariableAndComma(builder, vm);
                    IInstruction.AppendVariableAndComma(builder, vm);
                    IInstruction.AppendVariableAndComma(builder, vm);
                    IInstruction.AppendVariableAndComma(builder, vm);
                    IInstruction.AppendVariable(builder, vm);
                    builder.Append(')');
                    return;
                
            }
        }
    }
}