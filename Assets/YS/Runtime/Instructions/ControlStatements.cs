using System;
using System.Text;
using System.Threading;
using UnityEngine;
using YS.VM;

namespace YS.Instructions {
    
    

    public sealed class Return : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static Return (){
            IInstruction.Instructions[Id]= new Return();
        }
        public void Execute(VirtualMachine vm) {
            vm.CurrentInstructionIndex = 30000;
            vm.State = ProcessState.Return;
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            builder.Append("return");
        }

    }
    public sealed class Break : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static Break (){
            IInstruction.Instructions[Id]= new Break();
        }
        public void Execute(VirtualMachine vm) {
            vm.CurrentInstructionIndex = 30000;
            vm.State = ProcessState.Break;
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            builder.Append("break");
        }
    }public sealed class BreakIf : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static BreakIf (){
            IInstruction.Instructions[Id]= new BreakIf();
        }
        public void Execute(VirtualMachine vm) {
            if (vm.ReadUshort() == 0 ^ vm.ReadVariable().As<bool>()) {
                vm.CurrentInstructionIndex = 30000;
                vm.State = ProcessState.Break;
            }
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            builder.Append("if ");
            if(vm.ReadUshort() == 0  )builder.Append("not ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" break");
        }
    }
    public sealed class Continue : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static Continue (){
            IInstruction.Instructions[Id]= new Continue();
        }
        public void Execute(VirtualMachine vm) {
            vm.CurrentInstructionIndex = 30000;
            vm.State = ProcessState.Continue;
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            builder.Append("continue");
        }
    }

    public sealed unsafe class InfiniteLoop : IInstruction {
        public static readonly InfiniteLoop Instance = new InfiniteLoop();
        public static readonly byte Id = IInstruction.Count++;
        static InfiniteLoop (){
            IInstruction.Instructions[Id]= Instance;
        }
        public void Execute(VirtualMachine vm) {
            ref var current = ref vm.CurrentInstructionIndex;
            ++current;
            var start = current;
            var end = (ushort)(start + vm.ReadUshort());
            var dataLength = vm.ReadUshort();
            var startPtr = vm.CurrentDataPtr;
            try {
                while (true) {
                    current = start;
                    vm.CurrentDataPtr = startPtr;
                    vm.ExecuteUntil(end);
                    switch (vm.State) {
                        case ProcessState.Continue:
                            vm.State = ProcessState.Next;
                            break;
                        case ProcessState.Return:
                            return;
                        case ProcessState.Break:
                            vm.State = ProcessState.Next;
                            goto endloop;
                        case ProcessState.Await: {
                            vm.RestartForEachDataList.Add(new RestartForEachData(default,start,startPtr,vm));
                            return;
                        }
                    }
                }
                endloop:
                current = (ushort)(end - 1);
                vm.CurrentDataPtr = startPtr + dataLength;
                
            }
            catch (Exception) {
                var op = vm.Codes[current];
                if (op == 0) Debug.LogError(current + "st op is null");
                else Debug.LogError(current + " : " + op);
                throw;
            }
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            builder.Append("loop ");
            ref var current = ref vm.CurrentInstructionIndex;
            var end = (ushort)(++current + vm.ReadUshort());
            vm.ReadUshort();
            builder.AppendLine();
            vm.ToCode(builder,indentLevel+1,end);
            --current;
        }
    }

    public sealed class ForEach :IInstruction {
        public static readonly ForEach Instance = new ForEach();
        public static readonly byte Id = IInstruction.Count++;
        static ForEach (){
            IInstruction.Instructions[Id]= Instance;
        }
        public unsafe void Execute(VirtualMachine vm) {
            var enumeratorSource=  DelegateLibrary.Enumerators[vm.ReadUshort()];
            var enumerable = vm.ReadVariable();
            var currentRef = vm.ReadVariable();
            ref var current = ref vm.CurrentInstructionIndex;
            ++current;
            var start = current;
            var end = (ushort)(start + vm.ReadUshort());
            var dataLength = vm.ReadUshort();
            var startPtr = vm.CurrentDataPtr;
            try {
                var enumerator = new YSEnumerator(enumeratorSource,enumerable, currentRef);
                enumerator.Reset();
                while (enumerator.MoveNext()) {
                    current = start;
                    vm.CurrentDataPtr = startPtr;
                    vm.ExecuteUntil(end);
                    switch (vm.State) {
                        case ProcessState.Continue:
                            vm.State = ProcessState.Next;
                            break;
                        case ProcessState.Return:
                            return;
                        case ProcessState.Break:
                            vm.State = ProcessState.Next;
                            goto endloop;
                        case ProcessState.Await: {
                            vm.RestartForEachDataList.Add(new RestartForEachData(enumerator,start,startPtr,vm));
                            return;
                        }
                    }
                }
                endloop:
                current = (ushort)(end - 1);
                vm.CurrentDataPtr = startPtr + dataLength;
                
            }
            catch (Exception) {
                var op = vm.Codes[current];
                if (op == 0) Debug.LogError(current + "st op is null");
                else Debug.LogError(current + " : " + op);
                throw;
            }
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            builder.Append("for ");
            vm.ReadUshort();
            var enumerator = vm.ReadUshort();
            var iterator = vm.ReadUshort();
            IInstruction.AppendVariable(iterator, builder, vm);
            builder.Append(" in  ");
            IInstruction.AppendVariable(enumerator, builder, vm);
            builder.Append(' ');
            ref var current = ref vm.CurrentInstructionIndex;
            var end = (ushort)(++current + vm.ReadUshort());
            vm.ReadUshort();
            builder.AppendLine();
         
            vm.ToCode(builder,indentLevel+1,end);
            --current;
            
        }
    }
    
    public sealed class IfInstruction :IInstruction{
        public static readonly byte Id = IInstruction.Count++;
        static IfInstruction (){
            IInstruction.Instructions[Id]= new IfInstruction();
        }
        public  unsafe void Execute(VirtualMachine vm) {
            if (vm.ReadUshort()==0^vm.ReadVariable().As<bool>()) {
                ref var current = ref vm.CurrentInstructionIndex;
                ++current;
                var start = current;
                var end = start + vm.ReadUshort();
                var codes = vm.Codes;
                var startPtr = ++vm.CurrentDataPtr;
                try {
                    vm.CurrentDataPtr = startPtr;
                    current = start;
                    vm.ExecuteUntil((ushort)end);
                    if (vm.State != ProcessState.Next) return;
                    current = (ushort) (end - 1);
                }
                catch (Exception) {
                    var op = codes[current];
                    if (op == 0) Debug.LogError(current + "st op is null");
                    else Debug.LogError(current + " : " + op);
                    throw;
                }
            }
            else {
                vm.CurrentInstructionIndex+=vm.ReadUshort();
                var dataLength=vm.ReadUshort();
                vm.CurrentDataPtr+=dataLength;
            }
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            builder.Append(vm.ReadUshort()==0?"if not (":"if (");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(')');
            ref var current = ref vm.CurrentInstructionIndex;
            var end = (ushort)(++current + vm.ReadUshort());
            vm.ReadUshort();
            builder.AppendLine();
            vm.ToCode(builder,indentLevel+1,end);
            current = (ushort) (end - 1);
        }

    }
    public sealed class IfElseInstruction :IInstruction{
        public static readonly byte Id = IInstruction.Count++;
        static IfElseInstruction (){
            IInstruction.Instructions[Id]= new IfElseInstruction();
        }
        public  unsafe void Execute(VirtualMachine vm) {
            if (vm.ReadUshort()==0^vm.ReadVariable().As<bool>()) {
                ref var current = ref vm.CurrentInstructionIndex;
                ++current;
                var start = current;
                var end = start + vm.ReadUshort();
                var codes = vm.Codes;
                var startPtr = ++vm.CurrentDataPtr;
                try {
                    vm.CurrentDataPtr = startPtr;
                    current = start;
                    vm.ExecuteUntil((ushort)end);
                    if (vm.State != ProcessState.Next) return;
                    //if(current==end) {
                        current+=vm.ReadUshort();
                        var dataLength=vm.ReadUshort();
                        vm.CurrentDataPtr+=dataLength;
                    //}
                }
                catch (Exception) {
                    var op = codes[current];
                    if (op == 0) Debug.LogError(current + "st op is null");
                    else Debug.LogError(current + " : " + op);
                    throw;
                }
            }
            else {
                ref var current = ref vm.CurrentInstructionIndex;
                current+=2;
                current+=vm.ReadUshort();
                vm.CurrentDataPtr+=vm.ReadUshort()+1;
                var start = current;
                var end = start + vm.ReadUshort();
                var codes = vm.Codes;
                var startPtr = ++vm.CurrentDataPtr;
                try {
                    vm.CurrentDataPtr = startPtr;
                    current = start;
                    vm.ExecuteUntil((ushort)end);
                    if (vm.State != ProcessState.Next) return;
                    current = (ushort) (end - 1);
                }
                catch (Exception) {
                    var op = codes[current];
                    if (op == 0) Debug.LogError(current + "st op is null");
                    else Debug.LogError(current + " : " + IInstruction.Instructions[op]);
                    throw;
                }
            }
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
            builder.Append(vm.ReadUshort()==0?"if not (":"if (");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(')');
            ref var current = ref vm.CurrentInstructionIndex;
            var end = (ushort)(++current + vm.ReadUshort());
            vm.ReadUshort();
            builder.AppendLine();
            vm.ToCode(builder,indentLevel+1,end);
            builder.AppendLine();
            IInstruction.AppendIndent(builder, indentLevel);
            builder.Append("else \n");
            end = (ushort)(++current + vm.ReadUshort());
            vm.ReadUshort();
            vm.ToCode(builder,indentLevel+1,end);
            current = (ushort) (end - 1);
        }
    }

    public sealed class ElseInstruction : IInstruction {
        public static readonly byte Id = IInstruction.Count++;

        static ElseInstruction() {
            IInstruction.Instructions[Id] = new ElseInstruction();
        }

        public unsafe void Execute(VirtualMachine vm) {
            vm.CurrentInstructionIndex+=vm.ReadUshort();
            var dataLength=vm.ReadUshort();
            vm.CurrentDataPtr+=dataLength;
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            throw new NotImplementedException();
        }
    }
}