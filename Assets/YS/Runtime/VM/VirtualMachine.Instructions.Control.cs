using System;
using UnityEngine;

namespace YS.VM {
    public unsafe partial class VirtualMachine {
        static void Return(VirtualMachine vm) {
            vm.CurrentInstructionIndex = 30000;
            vm.State = ProcessState.Return;
        }
        static void Break(VirtualMachine vm) {
            vm.CurrentInstructionIndex = 30000;
            vm.State = ProcessState.Break;
        }
        static void BreakIf(VirtualMachine vm) {
            if (vm.ReadUshort() == 0 ^ vm.ReadVariable().As<bool>()) {
                vm.CurrentInstructionIndex = 30000;
                vm.State = ProcessState.Break;
            }
        }
        static void Continue(VirtualMachine vm) {
            vm.CurrentInstructionIndex = 30000;
            vm.State = ProcessState.Continue;
        }
        
        #region InfiniteLoop

            static void InfiniteLoop(VirtualMachine vm) {
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

    #endregion

        #region ForEach

            static void ForEach(VirtualMachine vm) {
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
    #endregion

        #region If
        static void If(VirtualMachine vm) {
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
        #endregion

        #region IfElse
        static void IfElse(VirtualMachine vm) {
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
                    else Debug.LogError(current + " : " + YS.Instructions.IInstruction.Instructions[op]);
                    throw;
                }
            }
        }

        #endregion
        static void Else(VirtualMachine vm) {
            vm.CurrentInstructionIndex+=vm.ReadUshort();
            var dataLength=vm.ReadUshort();
            vm.CurrentDataPtr+=dataLength;
        }
    }
}