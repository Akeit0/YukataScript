using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YS.Async;

namespace YS.VM {
    public interface IAwaiterSource {
        public Type ResultType { get; }
        public bool UseCustomLoop { get; }
        
        public void Run(Variable srcVariable,Variable resultVariable,VirtualMachine engine);
    }

    public interface ILoopAwaitable {
        public bool IsCompleted { get; }
        public bool CheckToken(short token);
        
        public void SetResult(Variable target);
    }

    public struct Awaiter {
        public ILoopAwaitable Awaitable;
        public short token;
        public bool IsCompleted => Awaitable.IsCompleted;
    }
    public interface ILoopAwaitable<T> {
        
        public bool IsCompleted { get; }
        public void SetResult(Variable target);
    }

    public class NextFrame : ILoopAwaitable {
        public bool IsCompleted { get; }


        public bool CheckToken(short token) => true;

        public void SetResult(Variable target) {
            
        }
    }
    public class AwaiterSource : IAwaiterSource {
        public Type ResultType => null;
        public bool UseCustomLoop => false;
        public void Run(Variable srcVariable, Variable resultVariable, VirtualMachine engine) {
            var item=  srcVariable.As<Awaiter>();
            if (item.IsCompleted) return;
            engine.SetAsyncState();
           
        }
    }
    public class TimeSpanSource : IAwaiterSource {
        public Type ResultType => null;
        public bool UseCustomLoop => false;
        public void Run(Variable srcVariable, Variable resultVariable, VirtualMachine engine) {
          var item=  WaitRealTime.Create(srcVariable.As<TimeSpan>());
          item.AddToLoop(engine);
          engine.SetAsyncState();
        }
    }public class UniTimeAwaitSource : IAwaiterSource {
        public Type ResultType => null;
        public bool UseCustomLoop => false;
        public void Run(Variable srcVariable, Variable resultVariable, VirtualMachine engine) {
          var item=  WaitUniTime.Create(srcVariable.As<UniTime>());
          item.AddToLoop(engine);
          engine.SetAsyncState();
        }
    }
     public class EnumeratorSource : IAwaiterSource {
        public Type ResultType => null;
        public bool UseCustomLoop => false;
        public void Run(Variable srcVariable, Variable resultVariable, VirtualMachine engine) {
            var item=  WaitEnumerator.Create(srcVariable.As<IEnumerator>());
          item.AddToLoop(engine);
          engine.SetAsyncState();
        }
    }
    
    
    
    public class UnityEventSource : IAwaiterSource {
        public Type ResultType => null;
        public bool UseCustomLoop => false;
        public void Run(Variable srcVariable, Variable resultVariable, VirtualMachine engine) {
         var b=srcVariable.As<UnityEvent>();
         var item =  WaitCallBack.Create();
        var action = (UnityAction) item.SetTrue;
        item.userdata = action;
         b.AddListener(action);
         item.action=(o)=>b.RemoveListener((UnityAction)o); 
         item.AddToLoop(engine);
         engine.SetAsyncState();
         
        }
    }
   
    
    
    public class TaskYSAwaiterSource : IAwaiterSource {
        public Type ResultType => null;
        public bool UseCustomLoop => true;
        
        public void Run(Variable srcVariable, Variable resultVariable, VirtualMachine engine) {
            var task = srcVariable.As<Task>();
            var awaiter=task.GetAwaiter();
            if (awaiter.IsCompleted) return;
            engine.SetAsyncState();
            RunAsync(task,engine);
        }
        public async void RunAsync(Task task, VirtualMachine engine) {
            try {
                await task;
            }
            catch (Exception e) {
                Debug.LogException(e);
                engine.State = ProcessState.Exception;
                engine.Exception = (e);
            }
            finally {
                engine.ProcessFromLast();
            }
        }
    }
  

}