using System;
using System.Runtime.CompilerServices;

namespace YS {
    /// <summary>
    /// This class is to use YukataScript with IDE. 
    /// </summary>
    public static class Extension {
        public static IntRange range(int start, int end) => new IntRange(start, end);
        public static IntRange range(int end) => new IntRange(0, end);
        public static StepIntRange range(int start, int end,int step) => new StepIntRange(start, end,step);
        public static  RangeEnumerator GetEnumerator(this Range range) => new RangeEnumerator(range.Start.Value, range.End.Value);

        public static  TimeSpanAwaiter GetAwaiter(this TimeSpan timeSpan)
        {
            return new TimeSpanAwaiter();
        }
        public struct TimeSpanAwaiter:ICriticalNotifyCompletion {
            public bool IsCompleted => true;
            public void OnCompleted(Action continuation) {
                throw new NotImplementedException();
            }
            public void UnsafeOnCompleted(Action continuation) {
                throw new NotImplementedException();
            }
            public void GetResult() { }
        }
        
        public static  StepIntRange With(this Range range,int step)
        {
            return new StepIntRange(range.Start.Value, range.End.Value,step);
        }

        public static TimeSpan ms(this double ms)=>TimeSpan.FromMilliseconds(ms);
        public static TimeSpan ms(this int ms)=>TimeSpan.FromMilliseconds(ms);
        public static TimeSpan s(this double s)=>TimeSpan.FromSeconds(s);
        public static TimeSpan s(this int s)=>TimeSpan.FromSeconds(s);
    }
}