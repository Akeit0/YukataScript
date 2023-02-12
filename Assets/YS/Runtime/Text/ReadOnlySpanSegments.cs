using System;
using YS.Collections;
namespace YS.Text {
    public readonly ref struct ReadOnlySpanSegments<T> {
        public readonly  ReadOnlySpan<T> Source;
        public readonly ReadOnlySpan<Range> Ranges;
        public int Length => Ranges.Length;
        public ReadOnlySpanSegments(ReadOnlySpan<T> source, ReadOnlySpan<Range> ranges) {
            Source = source;
            Ranges = ranges;
        }
        public ReadOnlySpan<T> this[int index] => Source[Ranges[index]];
        public ReadOnlySpanSegments<T> this[Range range] => new ReadOnlySpanSegments<T>(Source, Ranges[range]);
        public ReadOnlySpan<T> AsSpan() => Source[Ranges[0].Start..Ranges[^1].End];

        public override string ToString() {
            return AsSpan().ToString();
        }

        public Enumerator GetEnumerator() => new Enumerator(this);
        public ref struct Enumerator {
            private readonly ReadOnlySpanSegments<T> _segments;
            private int _index;
            private ReadOnlySpan<T> _current;
            public Enumerator(ReadOnlySpanSegments<T> segments) {
                _segments = segments;
                _index = 0;
                _current = default;
            }
            public bool MoveNext() {
                if (_index >= _segments.Length) return false;
                _current = _segments[_index++];
                return true;
            }
            public ReadOnlySpan<T> Current => _current;


        }
    }
}