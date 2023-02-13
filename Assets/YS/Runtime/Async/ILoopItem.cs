using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using YS.Collections;
using YS.VM;
using Debug = UnityEngine.Debug;

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

    public class AnimationCurveSync:ILoopItem, IPoolNode<AnimationCurveSync> {
        static Pool<AnimationCurveSync> _pool;
        public ref AnimationCurveSync NextNode => ref _next;
        AnimationCurveSync _next;
        AnimationCurve _animationCurve;
        public float StartTime;
        public float Duration;
        Variable<float> _variable;
        public static AnimationCurveSync Create(Variable<float> variable,AnimationCurve animationCurve,float duration) {
            if (!_pool.TryPop(out var item)) return new AnimationCurveSync(variable,animationCurve,duration);
            item._variable = variable;
            item._animationCurve=animationCurve;
            item.StartTime=Time.time;
            item.Duration=duration;
            return item;
        }
        public void AddToLoop() {
            YukataPlayerLoop.AddAction(this);
        }
        AnimationCurveSync(Variable<float> variable,AnimationCurve animationCurve,float duration) {
            _variable = variable;
           _animationCurve=animationCurve;
           StartTime=Time.time;
           Duration=duration;
        }
        public  bool MoveNext() {
            if(_animationCurve==null) {
                _pool.TryPush(this);
                return false;
            }
            var progress = (Time.time - StartTime) / Duration;
            if (1 < progress) {
                _variable.value = _animationCurve.Evaluate(1);
                _pool.TryPush(this);
                return false;
            }
            _variable.value = _animationCurve.Evaluate(progress);
            return true;
        }
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