using UnityEngine;

namespace YS {
    public readonly struct Timer {
        public Timer(float duration) {
           StartTime = Time.time;
           TargetTime = StartTime + duration;
        }

        public bool IsEnd => Remain < 0;
        public readonly float StartTime;
        public readonly float TargetTime;
        public float Elapsed => Time.time - StartTime;
        public float ElapsedRatio => (Time.time - StartTime)/(TargetTime-StartTime);
        
        public float Remain =>TargetTime- Time.time ;
        public float RemainRatio  => (TargetTime - Time.time)/(TargetTime-StartTime) ;
    }
}