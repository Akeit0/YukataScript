using System;
using System.Collections;
using System.Diagnostics;
using YS.Collections;
using YS.VM;

namespace YS.Async {
    public interface ILoopItem {
        public bool MoveNext();
    }
    
    

    public abstract class Awaitable : ILoopItem {
        public void AddToLoop(VirtualMachine engine) {
            _data = new ReStartData(engine); 
            YukataPlayerLoop.AddAction(this);
        }
        protected ReStartData _data;
        public abstract bool MoveNext();
    }
   

    public class WaitCallBack : Awaitable ,IPoolNode<WaitCallBack>{
        static Pool<WaitCallBack> _pool;
        public ref WaitCallBack NextNode => ref _next;
        WaitCallBack _next;
        public bool t;
        public object userdata;
        public Action<object> action;
        public static WaitCallBack Create() {
            if (!_pool.TryPop(out var item)) return new WaitCallBack();
            item.t = false;
            return item;
        }
        public override bool MoveNext() {
            if (_data.IsStopped) return true;
            if (_data.IsCanceled) {
                _pool.TryPush(this);
                t = false;
                userdata = null;
                action = null;
                _data.Throw(new OperationCanceledException());
                return false;
            }
            if (!t) {
                return true;
            }
            action(userdata);
            _pool.TryPush(this);
            _data.Execute();
            t = false;
            userdata = null;
            action = null;
            return false;
        }

        public void SetTrue() => t = true;
    }

   
    
    public class WaitRealTime : Awaitable,IPoolNode<WaitRealTime> {
        static Pool<WaitRealTime> _pool;
        public ref WaitRealTime NextNode => ref _next;
        WaitRealTime _next;
        long ticks;
        public static WaitRealTime Create(TimeSpan timeSpan) {
            if (!_pool.TryPop(out var item)) return new WaitRealTime(timeSpan);
            item.ticks=Stopwatch.GetTimestamp() + timeSpan.Ticks;
            return item;
        }
        WaitRealTime(TimeSpan timeSpan) {
            ticks = Stopwatch.GetTimestamp() + timeSpan.Ticks;
        }
        public override bool MoveNext() {
            if (_data.IsStopped) return true;
            if (_data.IsCanceled) {
                _data.Throw(new OperationCanceledException());
                return false;
            }
            if (Stopwatch.GetTimestamp() < ticks) {
                return true;
            }
            _pool.TryPush(this);
            _data.Execute();
            return false;
        }
    }

  
    public class WaitEnumerator : Awaitable,IPoolNode<WaitEnumerator> {
        static Pool<WaitEnumerator> _pool;
        public ref WaitEnumerator NextNode => ref _next;
        WaitEnumerator _next;
        IEnumerator coroutine;
        public static WaitEnumerator Create(IEnumerator coroutine) {
            coroutine.Reset();
            if (!_pool.TryPop(out var item)) return new WaitEnumerator(coroutine);
            item.coroutine=coroutine;
            
            return item;
        }
        WaitEnumerator(IEnumerator coroutine) {
            this.coroutine=coroutine;
        }
        public override bool MoveNext() {
            if (_data.IsStopped) return true;
            if (_data.IsCanceled) {
                _data.Throw(new OperationCanceledException());
                (coroutine as IDisposable)?.Dispose();
                return false;
            }
            if (coroutine.MoveNext()) {
                return true;
            }
            _pool.TryPush(this);
            
            (coroutine as IDisposable)?.Dispose();
            _data.Execute();
            
            return false;
        }

    }
  
}