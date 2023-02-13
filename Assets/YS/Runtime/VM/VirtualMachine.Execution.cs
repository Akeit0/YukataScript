using System;
using System.Text;
using UnityEngine;
using YS.Instructions;

namespace YS.VM {
    public  unsafe partial class VirtualMachine {
        internal ushort* UnmanagedDataPtr;
        internal ushort* CurrentDataPtr;
        public ushort ReadUshort() => *CurrentDataPtr++;
        public void ReadUshort(int count) => CurrentDataPtr+=count;
        public Variable ReadVariable() =>Variables[ReadUshort()];
        public object ReadObject() =>Variables[ReadUshort()].ToObject();

        public IInstruction CurrentInstruction => IInstruction.Instructions[Codes[CurrentInstructionIndex]];
        public T Read<T>() where T:unmanaged{
            var p=(T*) CurrentDataPtr;
            var value = *p;
            CurrentDataPtr = (ushort*) (++p);
            return value;
        }
    
        public void ExecuteUntil(ushort end) {
            var codes = Codes;
            while (CurrentInstructionIndex<end) {
                IInstruction.Instructions[codes[CurrentInstructionIndex]].Execute(this);
                ++CurrentInstructionIndex;
            }
        }
        public void ExecuteUntilEnd() {
            var codes = Codes;
            while (CurrentInstructionIndex<OpCount) {
                IInstruction.Instructions[codes[CurrentInstructionIndex]].Execute(this);
                ++CurrentInstructionIndex;
            }
        }
         
    
        public int CurrentDataProgress() => (int)(CurrentDataPtr - UnmanagedDataPtr);
        void ExecutePartialFor(in RestartForEachData restartData) {
            if (RestartForEachDataList.TryPop(out var restartData2)) {
                ExecutePartialFor(restartData2);
                if (State == ProcessState.Await) {
                    RestartForEachDataList.Add(restartData);
                    return;
                }
                ++CurrentInstructionIndex;
            }
            var data = restartData;
          
            var startPtr = UnmanagedDataPtr+data.DataStartID;
            var dataLength = startPtr[-1];
            var start = data.OpStartID;
            var end = (ushort)(start + startPtr[-2]);
          
            ref var current = ref CurrentInstructionIndex;
           
                var enumerator = data.Enumerator;
                ExecuteUntil(end);
                switch (State) {
                    case ProcessState.Continue:
                        State = ProcessState.Next;
                        break;
                    case ProcessState.Return:
                        return;
                    case ProcessState.Break:
                        State = ProcessState.Next;
                        goto endloop;
                    case ProcessState.Await: {
                        RestartForEachDataList.Add(new RestartForEachData(enumerator,start,startPtr,this));
                        current = (ushort)Codes.Length;
                        return;
                    }
                }
                while (enumerator.MoveNext()) {
                    current = start;
                    CurrentDataPtr = startPtr;
                    ExecuteUntil(end);
                    switch (State) {
                        case ProcessState.Continue:
                            State = ProcessState.Next;
                            break;
                        case ProcessState.Return:
                            return;
                        case ProcessState.Break:
                            State = ProcessState.Next;
                            goto endloop;
                        case ProcessState.Await: {
                            RestartForEachDataList.Add(new RestartForEachData(enumerator,start,startPtr,this));
                            current = (ushort)Codes.Length;
                            return;
                        }
                    }
                }
                endloop:
               
                current = (ushort)(end - 1); 
                CurrentDataPtr = startPtr + dataLength;
         }
    }
    
}
