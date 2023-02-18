using System;
using UnityEngine;
using static YS.UniTime;
namespace YS {
    public readonly struct Timer {
        public Timer(float duration,bool ignoreScale=false) {
            IgnoreScale = ignoreScale;
           StartTime = CurrentTime(ignoreScale);
           TargetTime = StartTime + duration;
        }
        public Timer(UniTime time) {
            IgnoreScale = time.IgnoreScale;
           StartTime = CurrentTime(IgnoreScale);
           TargetTime = StartTime + time.Seconds;
        }

        
        public bool IsExpired => TargetTime< CurrentTime(IgnoreScale);
        public readonly double StartTime;
        public readonly double TargetTime;
        public readonly bool IgnoreScale;
        public float Elapsed =>(float) Math.Min(TargetTime, (CurrentTime(IgnoreScale) - StartTime));

        public float GetEasedElapsedRatio(EasingType easingType) => Easing.Ease(ElapsedRatio, easingType);
        public float GetCurvedElapsedRatio(AnimationCurve animationCurve) => animationCurve.Evaluate(ElapsedRatio);
        public float ElapsedRatio => (float)Math.Min(1, ((CurrentTime(IgnoreScale) - StartTime) / (TargetTime - StartTime))) ;
        public float Remain =>(float) Math.Max(0,(TargetTime - CurrentTime(IgnoreScale)));
        public float RemainRatio  => (float) Math.Max(0,((TargetTime - CurrentTime(IgnoreScale)) / (TargetTime - StartTime))) ;
        public float GetEasedRemainRatio(EasingType easingType) => Easing.Ease(RemainRatio, easingType);
        public float GetCurvedRemainRatio(AnimationCurve animationCurve) => animationCurve.Evaluate(RemainRatio);
    }
}