

using System;

namespace YS.Text {
    public readonly ref struct ReadOnlyStringSegmentSpan {
        public readonly string Source;
        public readonly ReadOnlySpan<Range> Ranges;

        public int Length => Ranges.Length;
        public ReadOnlyStringSegmentSpan(string source, ReadOnlySpan<Range> ranges) {
            Source = source;
            Ranges = ranges;
        }
        public StringSegment this[int index] => new StringSegment(Source, Ranges[index]);
        public ReadOnlyStringSegmentSpan this[Range range] => new ReadOnlyStringSegmentSpan(Source, Ranges[range]);
        public StringSegment AsSegment() => new StringSegment(Source, Ranges[0].Start..Ranges[^1].End);

        public override string ToString() {
            return AsSegment().ToString();
        }

        public Enumerator GetEnumerator() => new Enumerator(this);
        public ref struct Enumerator {
            private readonly ReadOnlyStringSegmentSpan _segments;
            private int _index;
            private StringSegment _current;
            public Enumerator(ReadOnlyStringSegmentSpan segments) {
                _segments = segments;
                _index = 0;
                _current = default;
            }
            public bool MoveNext() {
                if (_index >= _segments.Length) return false;
                _current = _segments[_index++];
                return true;
            }
            public StringSegment Current => _current;


        }
    }
}