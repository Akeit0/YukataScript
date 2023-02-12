using System;
using UnityEngine;

namespace YS {
    public readonly struct ElapsedTimeLogger :IDisposable{
        public static Action<string> Logger = Debug.Log;
        readonly ValueStopwatch _stopwatch;
        public readonly string Label;
        ElapsedTimeLogger(ValueStopwatch stopwatch) {
            _stopwatch = stopwatch;
            Label = null;
        }
        ElapsedTimeLogger(string label,ValueStopwatch stopwatch) {
            _stopwatch = stopwatch;
            Label = label;
        }
        public static ElapsedTimeLogger StartNew() =>new (ValueStopwatch.StartNew());
        public static ElapsedTimeLogger StartNew(string label) =>new (label,ValueStopwatch.StartNew());

        public void Dispose() {
           Log();
        }
        public void Log() {
            if (Label != null)
                Logger(Label + " : " + _stopwatch.GetElapsedTime().TotalMilliseconds + " ms");
            else  Logger(_stopwatch.GetElapsedTime().TotalMilliseconds + " ms");
        }
    }
}