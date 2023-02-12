using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using YS.VM;

namespace YS.Async {
    public struct ProcessHandler {
        public VirtualMachine Engine;
        public bool IsDisposed => Engine == null;
        public void Dispose() {
            Engine = null;
        }
        public  Awaiter GetAwaiter() {
            return new Awaiter(this);
        }
        public readonly struct Awaiter : ICriticalNotifyCompletion
        {
            readonly  ProcessHandler _processHandler;
            public Awaiter(ProcessHandler processHandler) {
                _processHandler = processHandler;

            }
            public bool IsCompleted => !_processHandler.Engine.IsRunning;

            public void GetResult()
            {
                if (_processHandler.Engine==null)
                {
                    return ;
                }
            
                var engine = _processHandler.Engine;
               
                if (engine.Exception != null) {
                    var ex = engine.Exception;
                    engine.Exception = null;
                    throw new Exception("async",ex);
                }
               
            }
            public void OnCompleted(Action continuation)
            {
                UnsafeOnCompleted(continuation);
            }
            public void UnsafeOnCompleted(Action continuation) {
                _processHandler.Engine.OnComplete += continuation;
            }
        }
    }
}