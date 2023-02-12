using System;

namespace YS {
         
        public readonly struct IntRange:IEquatable<IntRange> {
           
            public readonly int Start;
            public readonly int End;
            public IntRange(int start,int end) {
                if (end - start < 0) throw new IndexOutOfRangeException();
                Start = start;
                End = end;
            }
            
            public override bool Equals(object obj) {
                return obj is IntRange other && Equals(other);
            }

            public bool Equals(IntRange other) {
                return Start == other.Start && End == other.End;
            }

            public override int GetHashCode() {
                return HashCode.Combine(Start, End);
            }
            public  RangeEnumerator GetEnumerator()
            {
                return new RangeEnumerator(Start, End);
            }

            public override string ToString() {
                return Start+" .. "+End;
            }
        }
        public readonly struct StepIntRange:IEquatable<StepIntRange> {
          
            public readonly int Start;
            public readonly int End;
            public readonly int Step;
            
            public StepIntRange(int start,int end,int step) {
                switch (step) {
                    case 0:
                        throw new IndexOutOfRangeException(nameof(step));
                    case > 0: {
                        if(end - start < 0) throw new IndexOutOfRangeException(nameof(step));
                        break;
                    }
                    default: {
                        if (0<end - start ) throw new IndexOutOfRangeException(nameof(step));
                        break;
                    }
                }

                Start = start;
                End = end;
                Step = step;
            }
    
            
            public override bool Equals(object obj) {
                return obj is StepIntRange other && Equals(other);
            }

            public bool Equals(StepIntRange other) {
                return Start == other.Start && End == other.End&&Step==other.Step;
            }

            public override int GetHashCode() {
                return HashCode.Combine(Start, End,Step);
            }
            public  StepRangeEnumerator GetEnumerator()
            {
           
                return new StepRangeEnumerator(Start, End,Step);
            }
        }

        public  struct RangeEnumerator 
        {
            readonly int _end;
            int _current;
            public RangeEnumerator(int start, int end)
            {
                _current = start - 1; 
                _end = end;
            }
            public int Current => _current;
            public bool MoveNext() => ++_current < _end;
        }
        public  struct StepRangeEnumerator 
        {
            readonly int _end;
            int _current;
            readonly int _step;
            public StepRangeEnumerator(int start, int end,int step) {
                _current = start - step; 
                _end = end;
                _step = step;
            }
            public int Current => _current;
            public bool MoveNext() {
                _current += _step;
                if(0<_step) return _current < _end;
                return _current > _end;
            }
        }
}