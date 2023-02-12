using System;

namespace YS.Collections {
    
    public readonly ref struct ReversedSpan<T> {
        public readonly Span<T> Span;
        public ReversedSpan(Span<T> span) {
            Span = span;
        }
        
        
        public ref T this[int index] => ref Span[Span.Length-index-1];
        public Enumerator GetEnumerator() => new Enumerator(Span);
        
        public ref struct Enumerator {
            readonly Span<T> _span;
            int index;
            public Enumerator(Span<T> span) {
                _span = span;
                index = span.Length;
               
            }
            public bool MoveNext() {
                return 0 <= --index;
            }
            public ref T Current =>ref  _span[index];
        }
        
    }
}