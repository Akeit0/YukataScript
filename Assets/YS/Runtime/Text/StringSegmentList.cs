using System;
using YS.Collections;

namespace YS.Text {
    public class StringSegmentList {
        private ListLike<Range> _ranges;
        public string Source;

        public StringSegmentList(int capacity=4) {
            _ranges = new ListLike<Range>(capacity);
        }
        public StringSegmentList(string source) {
            Source = source;
            _ranges = new ListLike<Range>(4);
        }

        public int Count => _ranges.Count;
        public void Add(Range range) => _ranges.Add(range);
        public StringSegment this[int index] => new StringSegment(Source, _ranges[index]);


        public ReadOnlyStringSegmentSpan AsSegmentSpan() => new ReadOnlyStringSegmentSpan(Source, _ranges.AsSpan());

        public void Clear() {
            _ranges.Clear();
            Source = null;
        }
    }
}