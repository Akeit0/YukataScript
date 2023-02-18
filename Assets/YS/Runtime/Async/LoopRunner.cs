using System;
using System.Diagnostics;
using UnityEngine;
using YS.Collections;
using Debug = UnityEngine.Debug;

namespace YS.Async {
    public class LoopRunner {
        ListLike<ILoopItem> Current =new  (10);
        ListLike<ILoopItem> Wait =new  (10);
        
        readonly object runningAndQueueLock = new ();
        readonly object arrayLock = new ();
        readonly Action<Exception> unhandledExceptionCallback=Debug.LogException;
        
        bool running = false;
        public LoopRunner() {
#if UNITY_EDITOR
            if(!Application.isPlaying) {
                UnityEditor.EditorApplication.update += EditUpdate;
             
            }
#endif
        }
#if UNITY_EDITOR
       
        void EditUpdate() {
           if(0<Current.Count) {
               Run();
               UnityEditor.SceneView.RepaintAll();
           }
              
          
        }
#endif
        public void AddAction(ILoopItem item)
        {
            lock (runningAndQueueLock)
            {
                if (running)
                {
                    Wait.Add(item);
                    return;
                }
            }
            lock (arrayLock)
            {
                Current.Add(item);
            }
        }
        static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;
        public long Ticks;
        public void Run() {
            Ticks = 0;
            var startTimestamp = Stopwatch.GetTimestamp();
            lock (runningAndQueueLock) {
                running = true;
            }
            lock (arrayLock) {
                var tail = Current.Count-1;
                var span = Current.AsSpan();
                for (int i = tail; 0<=i; --i) {
                    ref var item =ref  span[i];
                    try {
                        if (!item.MoveNext()) {
                            --tail;
                            Current.RemoveLast();
                        } else break;
                    }
                    catch (Exception ex)
                    {
                        item = null;
                        try { unhandledExceptionCallback(ex); }
                        catch {
                            // ignored
                        }
                    }
                }
                for (int i = tail; 0<=i; --i) {
                    ref var item =ref  span[i];
                    if (item == null) {
                        item = Current.Pop();
                        continue;
                    }
                    try {
                        if ( !item.MoveNext()) {
                            item = Current.Pop();
                        } 
                    }
                    catch (Exception ex)
                    {
                        item = null;
                        try { unhandledExceptionCallback(ex); }
                        catch {
                            // ignored
                        }
                    }
                }
            } lock (runningAndQueueLock)
            {
                running = false;
                Current.AddRange(Wait.AsSpan());
                Wait.Clear();
            }
            var delta = Stopwatch.GetTimestamp() - startTimestamp;
            Ticks= (long)(delta * TimestampToTicks);

        }
        

    }
}