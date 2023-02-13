using System;
using UnityEngine;
namespace YS {
    public readonly struct UniTime {
        public readonly float Seconds;
        public readonly bool IgnoreScale;
   
#if UNITY_EDITOR
        public static long FirstTick {
            get {
                if (_firstTick == 0) {
                    _firstTick= System.Diagnostics.Stopwatch.GetTimestamp();
                }
                return _firstTick;
            }
        }
        static long _firstTick;
#endif
        public UniTime(float seconds ) {
            Seconds = seconds;
            IgnoreScale = false;
        }
        public UniTime(float seconds,bool ignoreScale) {
            Seconds = seconds;
            IgnoreScale = ignoreScale;
        }
        public static double CurrentTime(bool ignoreScale) {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                if (FirstTick == 0) {
                    return TimeSpan.Zero.TotalSeconds;
                }
                return (new TimeSpan(System.Diagnostics.Stopwatch.GetTimestamp()-FirstTick).TotalSeconds);
            }
#endif
            return ignoreScale? Time.unscaledTimeAsDouble:Time.timeAsDouble;
        }
        
        public double CurrentTime() => CurrentTime(IgnoreScale);
        public UniTime FromNow() {
            
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                return new UniTime(Seconds+(float)(new TimeSpan(System.Diagnostics.Stopwatch.GetTimestamp()-FirstTick).TotalSeconds)) ;
            }
#endif
            return new UniTime(Seconds + (IgnoreScale ? Time.unscaledTime : Time.time), IgnoreScale);
        }
        
    }
}