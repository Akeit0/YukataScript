using System;
namespace YS.Text {
    public readonly struct StringSegment {
        public readonly string Source;
        public readonly Range Range;
        public StringSegment(string source, Range range) {
            Source = source;
            Range = range;
        }

        public int Length => AsSpan().Length;
        public ReadOnlySpan<char> AsSpan() => Source.AsSpan()[Range];
        
        public  static implicit operator ReadOnlySpan<char>  (StringSegment segment)=>segment.AsSpan();
        public  static implicit operator StringSegment  (string text)=>new StringSegment(text,..text.Length);

        public override int GetHashCode() {
            return AsSpan().GetExHashCode();
        }
        public override string ToString() {
            var span = AsSpan();
            if (span.Length == Source.Length) return Source;
            return span.Length == 1 ? span[0].ToStringCash() : span.ToString();
        }
    }
}