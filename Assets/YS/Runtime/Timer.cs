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
        public bool IsEnd => TargetTime< CurrentTime;
        public readonly double StartTime;
        public readonly double TargetTime;
        public float Elapsed => (float)(CurrentTime - StartTime);

        public float GetEasedElapsedRatio(EasingType easingType) => Easing.Ease(ElapsedRatio, easingType);
        public float GetCurvedElapsedRatio(AnimationCurve animationCurve) => animationCurve.Evaluate(ElapsedRatio);
        public float ElapsedRatio => (float) ((CurrentTime - StartTime) / (TargetTime - StartTime));
        public float Remain =>(float) (TargetTime - CurrentTime) ;
        public float RemainRatio  => (float) ((TargetTime - CurrentTime) / (TargetTime - StartTime)) ;
        
    }
}