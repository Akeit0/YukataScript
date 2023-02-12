using System;

namespace YS.VM {
    public enum ProcessState :byte{
        Next,
        Continue,
        Break,
        Return,
        Await,
        Complete,
        Exception,
        Stop
    }

    public readonly struct ReStartData {
        private readonly ushort opStart;
        private readonly ushort dataStart;
        public readonly VirtualMachine Engine;
        public ReStartData(VirtualMachine engine) {
            opStart = (ushort) (engine.CurrentInstructionIndex + 1);
            dataStart = (ushort) (engine.CurrentDataProgress());
            this.Engine = engine;
        }

        public bool IsCanceled =>Engine.State!=ProcessState.Await|| Engine.GetCancellationToken().IsCancellationRequested;
        public bool IsStopped => Engine.State==ProcessState.Stop;
        public void Execute() {
            Engine.Process(opStart,dataStart);
        }
        public void Throw(Exception exception) {
            Engine.Throw(opStart,dataStart,exception);
        }
    }
    public unsafe struct RestartForEachData {
        public YSEnumerator Enumerator;
        public readonly ushort OpStartID;
        public readonly ushort DataStartID;
        public RestartForEachData(YSEnumerator enumerator,int startId,ushort* startPtr, VirtualMachine engine) {
            Enumerator = enumerator;
            OpStartID = (ushort)startId;
            var unmanagedDataPtr =engine.UnmanagedDataPtr;
            DataStartID = (ushort) (startPtr - unmanagedDataPtr);
        }
       
    }

}