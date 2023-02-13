using System;
using UnityEngine;

namespace YS {
    public readonly struct Timer {
        public Timer(float duration) {
           StartTime = CurrentTime;
           TargetTime = StartTime + duration;
        }
#if UNITY_EDITOR
        static long _firstTick;
#endif
        static double CurrentTime {
            get {
#if UNITY_EDITOR
                if (!Application.isPlaying) {
                    if (_firstTick == 0) {
                        _firstTick= System.Diagnostics.Stopwatch.GetTimestamp();
                    }
                    return (new System.TimeSpan(System.Diagnostics.Stopwatch.GetTimestamp()-_firstTick).TotalSeconds);
                }
#endif
                return Time.timeAsDouble;
            }
        }
        public bool IsExpired => TargetTime< CurrentTime;
        public readonly double StartTime;
        public readonly double TargetTime;
        public float Elapsed =>(float) Math.Min(TargetTime, (CurrentTime - StartTime));

        public float GetEasedElapsedRatio(EasingType easingType) => Easing.Ease(ElapsedRatio, easingType);
        public float GetCurvedElapsedRatio(AnimationCurve animationCurve) => animationCurve.Evaluate(ElapsedRatio);
        public float ElapsedRatio => (float)Math.Min(1, ((CurrentTime - StartTime) / (TargetTime - StartTime))) ;
        public float Remain =>(float) Math.Max(0,(TargetTime - CurrentTime));
        public float RemainRatio  => (float) Math.Max(0,((TargetTime - CurrentTime) / (TargetTime - StartTime))) ;
        
    }
}