using System;
using System.Collections;
using System.Collections.Generic;
using YS.Text;
namespace YS.Collections {
    public class StringDictionary<TValue>:IEnumerable<KeyValuePair<string,TValue>> {
        public struct Entry {
            public int hashCode;
            public int next;
            public string key;
            public TValue value;
        }
        
        int[] buckets;
        Entry[] entries;
        int count;
        int version;
        int freeList;
        int freeCount;

        public int Count => count - freeCount;

        public ReadOnlySpan<Entry> ReadEntries() => entries.AsSpan()[..count];

        public StringDictionary() {
            
        }
        public StringDictionary(int capacity) {
            Initialize(capacity);
        }

        void Initialize(int capacity) {
            int size = 1;
            while (size < capacity) {
                size <<= 1;
            }

            buckets = new int[size];
            for (int i = 0; i < buckets.Length; i++) buckets[i] = -1;
            entries = new Entry[size];
            freeList = -1;
        }

        int FindEntry(ReadOnlySpan<char> key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }
            if (buckets != null) {
                int hashCode = key.GetExHashCode();
                for (int i = buckets[hashCode & (buckets.Length - 1)]; i >= 0; i = entries[i].next) {
                    if (entries[i].hashCode == hashCode && entries[i].key.Equals(key)) return i;
                }
            }
            return -1;
        }
        int FindEntry(int hashCode,ReadOnlySpan<char> key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }
            if (buckets != null) {
                for (int i = buckets[hashCode & buckets.Length - 1]; i >= 0; i = entries[i].next) {
                    if (entries[i].hashCode == hashCode && (entries[i].key.Equals(key))) return i;
                }
            }
            return -1;
        }

        int FindEntry(string key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }
            if (buckets != null) {
                int hashCode = key.GetExHashCode();
                for (int i = buckets[hashCode & (buckets.Length - 1)]; i >= 0; i = entries[i].next) {
                    if (entries[i].hashCode == hashCode && (entries[i].key == key)) return i;
                }
            }
            return -1;
        }
        int FindEntry(int hashCode,string key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }
            if (buckets != null) {
                for (int i = buckets[hashCode & (buckets.Length - 1)]; i >= 0; i = entries[i].next) {
                    if (entries[i].hashCode == hashCode && (entries[i].key == key)) return i;
                }
            }
            return -1;
        }

        void Insert(string key, TValue value, bool add) {
            if (key == null) {
                throw new Exception();
            }
            Insert( key.GetExHashCode(),key,value,add);
        }
        void Insert(int hashCode,string key, TValue value, bool add) {
            if (key == null) {
                throw new Exception();
            }
            if (buckets == null) Initialize(0);
            int targetBucket = hashCode & (buckets.Length - 1);
            for (int i = buckets[targetBucket]; i >= 0; i = entries[i].next) {
                if (entries[i].hashCode == hashCode && entries[i].key == key) {
                    if (add) throw new ArgumentException(key);
                    entries[i].value = value;
                    version++;
                    return;
                }
            }
            int index;
            if (freeCount > 0) {
                index = freeList;
                freeList = entries[index].next;
                freeCount--;
            }
            else {
                if (count >= entries.Length*0.8f) {
                    Resize();
                    targetBucket = hashCode & (buckets.Length - 1);
                }
                index = count;
                count++;
            }
            entries[index].hashCode = hashCode;
            entries[index].next = buckets[targetBucket];
            entries[index].key = key;
            entries[index].value = value;
            buckets[targetBucket] = index;
            version++;
        }
        void Insert(ReadOnlySpan<char> key, TValue value) {
            if (key == null) {
                throw new Exception();
            }
            if (buckets == null) Initialize(4);
            int hashCode = key.GetExHashCode();
            int targetBucket = hashCode & (buckets.Length - 1);
            for (int i = buckets[targetBucket]; i >= 0; i = entries[i].next) {
                if (entries[i].hashCode == hashCode && entries[i].key.Equals(key) ) {
                    entries[i].value = value;
                    version++;
                    return;
                }
            }
            int index;
            if (freeCount > 0) {
                index = freeList;
                freeList = entries[index].next;
                freeCount--;
            }
            else {
                if (count >= entries.Length*0.8f) {
                    Resize();
                    targetBucket = hashCode & (buckets.Length - 1);
                }

                index = count;
                count++;
            }
            entries[index].hashCode = hashCode;
            entries[index].next = buckets[targetBucket];
            entries[index].key = key.ToString();
            entries[index].value = value;
            buckets[targetBucket] = index;
            version++;

        }

        public bool TryAdd(string key, TValue value) {
            if (key == null) {
                throw new Exception();
            }

            if (buckets == null) Initialize(0);
            int hashCode = key.GetExHashCode();
            int targetBucket = hashCode & (buckets.Length - 1);
            for (int i = buckets[targetBucket]; i >= 0; i = entries[i].next) {
                if (entries[i].hashCode == hashCode && entries[i].key == key) {
                    return false;
                }
            }

            int index;
            if (freeCount > 0) {
                index = freeList;
                freeList = entries[index].next;
                freeCount--;
            }
            else {
                if (count >= entries.Length*0.8f) {
                    Resize();
                    targetBucket = hashCode & (buckets.Length - 1);
                }

                index = count;
                count++;
            }

            entries[index].hashCode = hashCode;
            entries[index].next = buckets[targetBucket];
            entries[index].key = key;
            entries[index].value = value;
            buckets[targetBucket] = index;
            version++;
            return true;
        }

        void Resize() {
            var newSize = count * 2;
            int[] newBuckets = new int[newSize];
            for (int i = 0; i < newBuckets.Length; i++) newBuckets[i] = -1;
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(entries, 0, newEntries, 0, count);
            for (int i = 0; i < count; i++) {
                int bucket = newEntries[i].hashCode & (newSize - 1);
                newEntries[i].next = newBuckets[bucket];
                newBuckets[bucket] = i;
            }

            buckets = newBuckets;
            entries = newEntries;
        }

        public void Add(string key, TValue value) {
            Insert(key, value, true);
        }
        public void Add(int hashCode,string key, TValue value) {
            Insert(hashCode,key, value, true);
        }



        public bool Remove(ReadOnlySpan<char> key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }
            if (buckets != null) {
                int hashCode = key.GetExHashCode();
                int bucket = hashCode & (buckets.Length - 1);
                int last = -1;
                for (int i = buckets[bucket]; i >= 0; last = i, i = entries[i].next) {
                    if (entries[i].hashCode == hashCode && entries[i].key.Equals(key)) {
                        if (last < 0) {
                            buckets[bucket] = entries[i].next;
                        }
                        else {
                            entries[last].next = entries[i].next;
                        }
                        entries[i].hashCode = 0;
                        entries[i].next = freeList;
                        entries[i].key = default;
                        entries[i].value = default(TValue);
                        freeList = i;
                        freeCount++;
                        version++;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Remove(string key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }
            if (buckets != null) {
                int hashCode = key.GetExHashCode();
                int bucket = hashCode & (buckets.Length - 1);
                int last = -1;
                for (int i = buckets[bucket]; i >= 0; last = i, i = entries[i].next) {
                    if (entries[i].hashCode == hashCode && entries[i].key == key) {
                        if (last < 0) {
                            buckets[bucket] = entries[i].next;
                        }
                        else {
                            entries[last].next = entries[i].next;
                        }
                        entries[i].hashCode = 0;
                        entries[i].next = freeList;
                        entries[i].key = default;
                        entries[i].value = default;
                        freeList = i;
                        freeCount++;
                        version++;
                        return true;
                    }
                }
            }

            return false;
        }
        public void Clear() {
            if (count > 0) {
                for (int i = 0; i < buckets.Length; i++) buckets[i] = -1;
                Array.Clear(entries, 0, count);
                freeList = -1;
                count = 0;
                freeCount = 0;
                version++;
            }
        }
        public bool TryGetValue(ReadOnlySpan<char> key, out TValue value) {
            int i = FindEntry(key);
            if (i >= 0) {
                value = entries[i].value;
                return true;
            }

            value = default;
            return false;
        }
      public bool TryGetValue(int hashCode,ReadOnlySpan<char> key, out TValue value) {
            int i = FindEntry(hashCode,key);
            if (i >= 0) {
                value = entries[i].value;
                return true;
            }

            value = default;
            return false;
        }
      public bool TryGetValue(int hashCode,string key, out TValue value) {
            int i = FindEntry(hashCode,key);
            if (i >= 0) {
                value = entries[i].value;
                return true;
            }

            value = default;
            return false;
        }
      

        public bool TryGetValue(string key, out TValue value) {
            int i = FindEntry(key);
            if (i >= 0) {
                value = entries[i].value;
                return true;
            }

            value = default;
            return false;
        }

        public TValue this[string key] {
            get {
                int i = FindEntry(key);
                if (i >= 0) {
                   return entries[i].value;
                }
                throw new KeyNotFoundException(key);
            }
            set => Insert(key, value, false);
        }
        public TValue this[ReadOnlySpan<char> key] {
            get {
                int i = FindEntry(key);
                if (i >= 0) {
                   return entries[i].value;
                }
                throw new KeyNotFoundException(key.ToString());
            }
            set => Insert(key, value);
        }

        public bool ContainsKey(string key) {
            int i = FindEntry(key);
            return i >= 0;
        }
        public bool ContainsKey(ReadOnlySpan<char> key) {
            int i = FindEntry(key);
            return i >= 0;
        }
        

        public Enumerator GetEnumerator() => new Enumerator(this);
        
        public KeyValuePair<string, TValue> First() {
            var index = 0;
            while (index < count) {
                var entry = entries[index++];
                if (entry.key != null) {
                    return new KeyValuePair<string, TValue>(entry.key, entry.value);
                }
            }

            throw new Exception();
        }
        public KeyValuePair<string, TValue> First(Func<KeyValuePair<string, TValue>,bool> func) {
            var index = 0;
            while (index < count) {
                var entry = entries[index++];
                if (entry.key != null) {
                    var pair= new KeyValuePair<string, TValue>(entry.key, entry.value);
                    if (func(pair)) return pair;
                }
            }
            throw new Exception();
        }
        public KeyValuePair<string, TValue> First(Func<string,bool> func) {
            var index = 0;
            while (index < count) {
                var entry = entries[index++];
                if (entry.key != null) {
                    if (func(entry.key)) return new KeyValuePair<string, TValue>(entry.key, entry.value);
                }
            }
            throw new Exception();
        }
        
        IEnumerator<KeyValuePair<string, TValue>> IEnumerable<KeyValuePair<string, TValue>>.GetEnumerator() {
            return new Enumerator(this);
        }      
        
        IEnumerator IEnumerable.GetEnumerator() {
            return new Enumerator(this);
        }
        public struct Enumerator: IEnumerator<KeyValuePair<string,TValue>>
        {
            StringDictionary<TValue> _dictionary;
            int _version;
            int _index;
            KeyValuePair<string,TValue> _current;
            internal Enumerator(StringDictionary<TValue> dictionary) {
                _dictionary = dictionary;
                _version = dictionary.version;
                _index = 0;
                _current = default;
            }
            public bool MoveNext() {
                if (_version != _dictionary.version) {
                    throw new InvalidOperationException();
                }
                while (_index < _dictionary.count) {
                    var entry = _dictionary.entries[_index];
                    if (entry.key != null) {
                        _current = new KeyValuePair<string, TValue>(entry.key, entry.value);
                        _index++;
                        return true;
                    }
                    _index++;
                }

                _index = _dictionary.count + 1;
                _current = new KeyValuePair<string, TValue>();
                return false;
            }

            public KeyValuePair<string, TValue> Current => _current;

            public void Dispose() {
            }

            object IEnumerator.Current {
                get { 
                    if( _index == 0 || (_index == _dictionary.count + 1)) {
                        throw new InvalidOperationException();                    
                    }
                    return new KeyValuePair<string, TValue>(_current.Key, _current.Value);
                }
            }
            void IEnumerator.Reset() {
                if (_version != _dictionary.version) {
                    throw new InvalidOperationException();
                }
                _index = 0;
                _current = new KeyValuePair<string, TValue>();    
            }
        }
    }
}