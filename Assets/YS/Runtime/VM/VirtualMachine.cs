using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using YS.Async;
using YS.Collections;
using YS.Instructions;
using YS.Modules;

namespace YS.VM {
    
    public unsafe partial class VirtualMachine {
        public byte[] Codes;
        public int OpCount { get; private set; } = 0;
        public ushort[] UnmanagedData;
        public int DataCount { get; private set; } = 0;
        public Variable[] Variables;
        public int VarCount { get; private set; } = 2;
        
        
        public readonly StringDictionary<ushort> Dictionary=new StringDictionary<ushort>(16);
        public readonly SimpleList<ushort> CodeToLine=new SimpleList<ushort> (16);

        public Variable Result;
        public Variable Awaiter;
        public Variable<ushort> AwaiterMethod;

        public Variable<Action> Continuation;

       
        public ProcessState State;
        
       //For Async
       public ListLike<RestartForEachData> RestartForEachDataList=new ListLike<RestartForEachData>(4);
        public bool IsRunning => State is ProcessState.Await or ProcessState.Stop;
      
        public CancellationTokenSource CancellationTokenSource;
        ProcessHandler Handler;
        public Exception Exception;
        public Action OnComplete;

        public ushort RestartOpIndex;
        public ushort RestartDataIndex;
        
        
        
        public ushort CurrentInstructionIndex;

        public void SetAsyncState() {
            State = ProcessState.Await;
            RestartOpIndex = (ushort) (CurrentInstructionIndex + 1);
            RestartDataIndex = (ushort) (CurrentDataProgress());
            CurrentInstructionIndex = 30000;

        }
        public CancellationToken GetCancellationToken() {
            if (CancellationTokenSource == null) {
                CancellationTokenSource = new CancellationTokenSource();
            }
            else if (CancellationTokenSource.IsCancellationRequested) {
                CancellationTokenSource.Dispose();
                CancellationTokenSource = new CancellationTokenSource();
            }

            return CancellationTokenSource.Token;
        }

        public void Cancel() {
            if (IsRunning&&CancellationTokenSource is {IsCancellationRequested: false}) {
                CancellationTokenSource.Cancel();
                CancellationTokenSource.Dispose();
                RestartForEachDataList.Clear();
            }
            State = ProcessState.Next;
            CurrentInstructionIndex = 10000;
        }

        public string GetCurrentDebugData() {
            return "line :" + CodeToLine[CurrentInstructionIndex]+", operatorIndex :" + CurrentInstructionIndex;
        }
        public void Stop() {
            if (State == ProcessState.Await) {
                State = ProcessState.Stop;
            }
        }
        public void Restart() {
            if (State == ProcessState.Stop) {
                State = ProcessState.Await;
            }
        }

        public Variable GetVariable(ushort id) {
            return Variables[id];
        }
        
        public ushort LastVarId => (ushort)(VarCount - 1);
        public ushort LastOpId => (ushort)(OpCount - 1);
        public ushort LastDataId => (ushort)(DataCount - 1);

        public VirtualMachine(int opCount,int dataCount, int varCount) {
            Codes = new byte[opCount];
            UnmanagedData = new ushort[dataCount];
            Variables = new Variable[varCount+2];
            Variables[0] = Variable.Discarded;
            Variables[1] = Variable.MustBeZero;
            Continuation = new Variable<Action>(ProcessFromLast);
        }
        public   void Process() {
            if(IsRunning) {
                Debug.LogError("This is running");
                return;
            }
            Handler = default;
            OnComplete = null;
            CurrentInstructionIndex = 0;
            fixed (ushort* ptr = UnmanagedData) {
                UnmanagedDataPtr = ptr;
                CurrentDataPtr = ptr;
                try {
                    ExecuteUntilEnd();
                }
                catch (Exception) {
                    Debug.LogError(" op :"+ CurrentInstructionIndex.ToString()+" line :" + CodeToLine[CurrentInstructionIndex]);
                    throw ;
                }
               
            }
        }

        public ProcessHandler RunAsync() {
            if(IsRunning) {
                throw new Exception("This is running");
            }
            Handler.Engine = this;
            OnComplete = null;
            CurrentInstructionIndex = 0;
            fixed (ushort* ptr = UnmanagedData) {
                UnmanagedDataPtr = ptr;
                CurrentDataPtr = ptr;
                ExecuteUntilEnd();
            }
            return Handler;

        }

        internal void ProcessFromLast() {
            Process(RestartOpIndex, RestartDataIndex);
        }

        internal void Throw(ushort start, ushort dataStart,Exception exception) {
            
            Debug.LogError("line :" + CodeToLine[Mathf.Min(CodeToLine.Count-1,(start - 1))]+", operatorIndex :" + start);
            Exception = exception;
            if (!Handler.IsDisposed) {
                   OnComplete.Invoke();
            }
            else {
                throw exception;
            }
        }
        internal void Throw(Exception exception) {
            
            Debug.LogError(GetCurrentDebugData());
            Exception = exception;
            if (!Handler.IsDisposed) {
                   OnComplete.Invoke();
            }
            else {
                throw exception;
            }
        }

        internal   void Process(ushort start,ushort dataStart) {
            if(!IsRunning) {
                if (!Handler.IsDisposed) {
                    Debug.LogError("line :" + CodeToLine[start-1]+", operatorIndex :" + start);
                    Debug.LogError("This is not running");
                    OnComplete.Invoke();
                    return;
                }
                Debug.LogError("line :" + CodeToLine[start-1]+", operatorIndex :" + start);
                throw new Exception("This is not running");
            }
            fixed (ushort* ptr = UnmanagedData) {
                UnmanagedDataPtr = ptr;
                State = ProcessState.Next;
                try {
                    CurrentInstructionIndex = start;
                    CurrentDataPtr = ptr + dataStart;
                    if (RestartForEachDataList.TryPop(out var restartData)) {
                        ExecutePartialFor(restartData);
                        ++CurrentInstructionIndex;
                    }
                    ExecuteUntilEnd();
                }
                catch (Exception e) {
                    Debug.LogError(GetCurrentDebugData());
                    if (!Handler.IsDisposed) {
                        Exception = e;
                       OnComplete.Invoke();
                       return;
                    }
                    throw;
                }
            }
            if (!IsRunning) {
                if (!Handler.IsDisposed) {
                    OnComplete.Invoke();
                }
            }
        }

        public bool TryClear() {
            if (IsRunning) {
                return false;
            }
            OpCount = 0;
            Codes.AsSpan().Clear();
            DataCount = 0;
            Variables.AsSpan(2,VarCount-2).Clear();
            
            VarCount = 2;
            Dictionary.Clear();
            return true;
        }
        public bool TryClearWithOutVariables() {
            if (IsRunning) {
                return false;
            }
            OpCount = 0;
            Codes.AsSpan().Clear();
            DataCount = 0;
            return true;
        }
        public void Emit(byte opId) {
             if(Codes.Length==OpCount) {
                 Array.Resize(ref Codes,OpCount*2);
             }
             Codes[OpCount++] = opId;
        }
        public void Emit(ushort data,byte op) {
             if(Codes.Length==OpCount) {
                 Array.Resize(ref Codes,OpCount*2);
             }
             Codes[OpCount++] = op;
             if(UnmanagedData.Length==DataCount) {
                 Array.Resize(ref UnmanagedData,DataCount*2);
             }
             UnmanagedData[DataCount++] = data;
        }public void Emit(MethodID id) {
            Emit(id.Index, id.InstructionId);
        }
        public  void EmitData<T>(T data)where T:unmanaged {
            var d = data;
            var p = (ushort*) &d;
            for (int i = 0; i < sizeof(T) / 2; i++) {
                EmitData(p[i]);
            }
        }
        public void EmitData(ushort data) {
             if(UnmanagedData.Length==DataCount) {
                 Array.Resize(ref UnmanagedData,DataCount*2);
             }
             UnmanagedData[DataCount++] = data;
        }
        public void EmitData(ushort data0,ushort data1) {
             if(UnmanagedData.Length<=DataCount+1) {
                 Array.Resize(ref UnmanagedData, (DataCount+1)*2);
             }
             UnmanagedData[DataCount++] = data0;
             UnmanagedData[DataCount++] = data1;
        }
        public void EmitData(ushort data0,ushort data1,ushort data2) {
             if(UnmanagedData.Length<=DataCount+2) {
                 Array.Resize(ref UnmanagedData, (DataCount+2)*2);
             }
             UnmanagedData[DataCount++] = data0;
             UnmanagedData[DataCount++] = data1;
             UnmanagedData[DataCount++] = data2;
        }
        
        
        public void EmitCopy(ushort target,ushort src) {
            EmitData(target,src);
            Emit((byte)Instructions.Copy);
        }
        public void EmitReadInt(ushort target,int value) {
            EmitData(target);
            EmitData(value);
            Emit((byte)Instructions.Read32);
        }
        
        
        public ushort AddVariable(Variable variable) {
            if(Variables.Length==VarCount) {
                Array.Resize(ref Variables,VarCount*2);
            }
            Variables[VarCount++] = variable;
            return LastVarId;
        }
        
        public ushort AddVariable(string name,Variable variable) {
            if(Variables.Length==VarCount) {
                Array.Resize(ref Variables,VarCount*2);
            }

            Dictionary[name] = (ushort)VarCount;
            Variables[VarCount++] = variable;
            return LastVarId;
        }
        
        
        
        public (ushort id ,Variable variable) AddVariable(Type type) {
            if(Variables.Length==VarCount) {
                Array.Resize(ref Variables,VarCount*2);
            }

            var variable = Variable.New(type);
            Variables[VarCount++] = variable;
            return (LastVarId,variable);
        }
        public (ushort id ,Variable variable) AddVariable<T>() {
            if(Variables.Length==VarCount) {
                Array.Resize(ref Variables,VarCount*2);
            }

            var variable = new Variable<T>();
            Variables[VarCount++] = new Variable<T>();
            return (LastVarId,variable);
        }

        public void ToCode(StringBuilder builder) {
            CurrentInstructionIndex = 0;
            fixed (ushort* ptr = UnmanagedData) {
                UnmanagedDataPtr = ptr;
                CurrentDataPtr = ptr;
                try {
                    int indentLevel = 0;
                    while (CurrentInstructionIndex < OpCount) {
                        CurrentInstruction.ToCode(this,builder,ref indentLevel);
                        builder.AppendLine();
                        ++CurrentInstructionIndex;
                    }
                }
                catch (Exception) {
                    if(CurrentInstructionIndex<CodeToLine.Count) {
                        Debug.LogError(" op :" + CurrentInstructionIndex.ToString() + " line :" +
                                       CodeToLine[CurrentInstructionIndex]);
                    }
                    else {
                        Debug.LogError(" op :" + CurrentInstructionIndex.ToString());
                    }
                    throw;
                }
                finally {
                    UnmanagedDataPtr = null;
                }
            }
        }

        public void ToCode(StringBuilder builder, int indentLevel, int end) {
            
            while (CurrentInstructionIndex < end) {
                CurrentInstruction.ToCode(this,builder,ref indentLevel);
                if(++CurrentInstructionIndex < end) builder.AppendLine();
            }
        }
        
    }

}