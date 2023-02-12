using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using YS.Text;
using static YS.Collections.CollectionsUtility;
namespace YS.Collections {
    
    
    public  class FixedArrayDictionary<T> {
        public readonly struct Entry {
            public readonly int HashCode; 
            public readonly string Key; 
            public readonly T Value;
            public Entry(int hashCode, string key, T value) {
                HashCode = hashCode;
                Key = key;
                Value = value;
            }
        }
        readonly (int start,int length)[] _ranges;
        readonly Entry[] _entries;
        readonly int _indexFor;
        public int Length => _entries.Length;
        public FixedArrayDictionary(ReadOnlySpan<KeyValuePair<string, T>> pairs, float loadFactor = 0.75f) {
            var initialCapacity = (int)(pairs.Length / loadFactor);
            var capacity = 1;
            while (capacity < initialCapacity)
            {
                capacity <<= 1;
            }
            _indexFor = capacity - 1;
           
            _ranges = new (int,int)[capacity];
            RentSpan<int> rentSpan =pairs.Length > 128
                ? ( RentUnmanagedSpan<int>(pairs.Length))
                :new RentSpan<int>(stackalloc int[pairs.Length]) ;
            Span<int> hashSpan = rentSpan.Span;
            for (int i = 0; i < pairs.Length; i++) {
                var hash= (pairs[i].Key.GetExHashCode());
                hashSpan[i] = hash;
                var index = _indexFor & hash;
                _ranges[index].length+=1;
            }
            var lastStart = 0;
            for (var i = 0; i < _ranges.Length; i++) {
                _ranges[i].start = lastStart;
                lastStart += _ranges[i].length;
            }
            _entries = new Entry[pairs.Length];
            Sort(hashSpan,pairs,_entries,_ranges);
            rentSpan.DisposeIfRent();
        }
        public T this[string key] {
            get {
                int i = FindEntry(key);
                if (i >= 0) {
                    return _entries[i].Value;
                }
                throw new KeyNotFoundException(key);
            }
        }
        int FindEntry(ReadOnlySpan<char> key) {
            if (key == null) throw new NullReferenceException(nameof(key));
            
            var hash = key.GetExHashCode();
            var range=_ranges[_indexFor & hash];
            for (int i = range.start; i < range.start+range.length; i++) {
                if (_entries[i].HashCode==hash&&_entries[i].Key.Equals(key)) {
                    return i;
                }
            }
            return -1;
        }
        unsafe int FindEntry(char*key, int length) {
            if (key == null) throw new NullReferenceException(nameof(key));
            
            var hash = StringEx.GetExHashCode(key, length);
            var range=_ranges[_indexFor & hash];
            for (int i = range.start; i < range.start+range.length; i++) {
                if (_entries[i].HashCode==hash&&_entries[i].Key.Equals(key,length)) {
                    return i;
                }
            }
            return -1;
        }

        int FindEntry(string key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }
            var hash = key.GetExHashCode();
            var range=_ranges[_indexFor & hash];
            for (int i = range.start; i < range.start+range.length; i++) {
                if (_entries[i].HashCode==hash&&_entries[i].Key==key) {
                    return i;
                }
            }
            return -1;
        }
        public bool Contains(ReadOnlySpan<char> key) {
            int i = FindEntry(key);
            return i >= 0;
        }

        public bool Contains(string key) {
            int i = FindEntry(key);
            return i >= 0;
        }

        public bool TryGetValue(string key, out T value) {
            int i = FindEntry(key);
            if (i >= 0) {
                value = _entries[i].Value;
                return true;
            }
            value = default;
            return false;
        }
        public bool TryGetValue(ReadOnlySpan<char> key, out T value) {
            int i = FindEntry(key);
            if (i >= 0) {
                value = _entries[i].Value;
                return true;
            }
            value = default;
            return false;
        }
        public unsafe bool TryGetValue(char* key,int length, out T value) {
            int i = FindEntry(key,length);
            if (i >= 0) {
                value = _entries[i].Value;
                return true;
            }
            value = default;
            return false;
        }
       
        static void Sort(ReadOnlySpan<int> hashes,ReadOnlySpan<KeyValuePair<string, T>> sources,Entry[] dest,(int start,int)[] ranges) {
            var indexFor = ranges.Length - 1;
            for (int i = 0; i < sources.Length; i++) {
                var hash = hashes[i];
                var index = hash & indexFor;
                var range = ranges[index];
                var targetIndex = range.start;
                while (dest[targetIndex].Key!=default) {
                    ++targetIndex;
                }
                var source = sources[i];
                dest[targetIndex] = new Entry(hash,source.Key, source.Value);
            }
        }
    }

    public sealed class FixedTypeVariableTable : FixedArrayDictionary<Type, Variable> {
        public FixedTypeVariableTable(ReadOnlySpan<KeyValuePair<Type, Variable>> pairs, float loadFactor = 0.75f) : base(pairs, loadFactor) {
        }
        protected override bool Equals(Type left, Type right) => left == right;

        protected override int GetHashCode(Type key) => key.GetHashCode();
    }
    
    public  abstract class FixedArrayDictionary<TKey,TValue>where  TKey:class{
        
        readonly IntRange[] _ranges;
        readonly KeyValuePair<TKey,TValue>[] _pairs;
        readonly int _indexFor;
        public int Length => _pairs.Length;
        public FixedArrayDictionary(ReadOnlySpan<KeyValuePair<TKey,TValue>> pairs, float loadFactor = 0.75f) {
            var initialCapacity = (int)(pairs.Length / loadFactor);
            var capacity = 1;
            while (capacity < initialCapacity)
            {
                capacity <<= 1;
            }
            _indexFor = capacity - 1;
            var hashes = new int[pairs.Length];
            _ranges = new IntRange[capacity];
            for (int i = 0; i < pairs.Length; i++) {
                var hash= (_indexFor & GetHashCode(pairs[i].Key));
                hashes[i] = hash;
                _ranges[hash].length+=1;
            }
            var lastStart = 0;
            for (var i = 0; i < _ranges.Length; i++) {
                _ranges[i].start = lastStart;
                lastStart += _ranges[i].length;
            }
            _pairs = new KeyValuePair<TKey,TValue>[pairs.Length];
            Sort(hashes,pairs,_pairs,_ranges);
        }
       
        public TValue this[TKey key] {
            get {
                var range=_ranges[_indexFor & GetHashCode(key)];
                for (int i = range.start; i < range.start+range.length; i++) {
                    if (Equals(_pairs[i].Key,key)) return _pairs[i].Value;
                }
                return default;
            }
        }


        public bool TryGetValue(TKey key, out TValue value) {
            var range=_ranges[_indexFor & GetHashCode(key)];
            for (int i = range.start; i < range.start+range.length; i++) {
                if (Equals(_pairs[i].Key,key)) {
                    value=_pairs[i].Value;
                    return true;
                }
            }
            value = default;
            return false;
        }
        public bool TryGetIndex(TKey key, out int index) {
            var range=_ranges[_indexFor & GetHashCode(key)];
            for (int i = range.start; i < range.start+range.length; i++) {
                if (Equals(_pairs[i].Key,key)) {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }
        
        public KeyValuePair<TKey,TValue> this[int index] => _pairs[index];


        public IEnumerator<KeyValuePair<TKey,TValue>> GetEnumerator() {

            return _pairs.AsEnumerable().GetEnumerator();
        }

        protected abstract bool Equals(TKey left, TKey right);
        protected abstract  int GetHashCode(TKey key);
        static void Sort(int[] hashes,ReadOnlySpan<KeyValuePair<TKey,TValue>> source,KeyValuePair<TKey,TValue>[] dest,IntRange[] ranges) {
            for (int i = 0; i < source.Length; i++) {
                var hash = hashes[i];
                var range = ranges[hash];
                var targetIndex = range.start;
                while (dest[targetIndex].Key!=null) {
                    ++targetIndex;
                }
                dest[targetIndex] = source[i];
            }
        }
        struct IntRange {
            public int start;
            public int length;

            public IntRange(int start, int length) {
                this.start = start;
                this.length = length;
            }

            public override string ToString() {
                return '('+start.ToString()+',' + length.ToString()+')';
            }
        }
    }
}