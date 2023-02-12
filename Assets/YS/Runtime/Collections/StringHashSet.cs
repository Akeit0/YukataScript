using System;
using YS.Text;
namespace YS.Collections {
    public class StringHashSet {
        struct Entry {
            public int HashCode; 
            public int Next; 
            public string Key; 
        }
        int[] _buckets;
        Entry[] _entries;
        int _count;
        int _version;
        int _freeList;
        int _freeCount;
        public int Count => _count - _freeCount;
        
        public StringHashSet() { }
        public StringHashSet(int capacity) {
            Initialize(capacity);
        }

        void Initialize(int capacity) {
            int size = 1;
            while (size < capacity) {
                size <<= 1;
            }

            _buckets = new int[size];
            for (int i = 0; i < _buckets.Length; i++) _buckets[i] = -1;
            _entries = new Entry[size];
            _freeList = -1;
        }

        int FindEntry(ReadOnlySpan<char> key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }

            if (_buckets != null) {
                int hashCode = key.GetExHashCode();
                for (int i = _buckets[hashCode & (_buckets.Length - 1)]; i >= 0; i = _entries[i].Next) {
                    if (_entries[i].HashCode == hashCode && (_entries[i].Key.Equals(key))) return i;
                }
            }

            return -1;
        }

        int FindEntry(string key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }

            if (_buckets != null) {
                int hashCode = key.GetExHashCode();
                for (int i = _buckets[hashCode & (_buckets.Length - 1)]; i >= 0; i = _entries[i].Next) {
                    if (_entries[i].HashCode == hashCode && (_entries[i].Key == key)) return i;
                }
            }

            return -1;
        }

        public bool Add(string key) {
            if (key == null) {
                throw new Exception();
            }
            if (_buckets == null) Initialize(0);
            int hashCode = key.GetExHashCode();
            int targetBucket = hashCode & (_buckets.Length - 1);
            for (int i = _buckets[targetBucket]; i >= 0; i = _entries[i].Next) {
                if (_entries[i].HashCode == hashCode && _entries[i].Key == key) {
                   
                    return false;
                }
            }
            int index;
            if (_freeCount > 0) {
                index = _freeList;
                _freeList = _entries[index].Next;
                _freeCount--;
            }
            else {
                if (_count == _entries.Length) {
                    Resize();
                    targetBucket = hashCode & (_buckets.Length - 1);
                }

                index = _count;
                _count++;
            }
            _entries[index].HashCode = hashCode;
            _entries[index].Next = _buckets[targetBucket];
            _entries[index].Key = key;
            _buckets[targetBucket] = index;
            _version++;
            return true;
        }
        bool Add(ReadOnlySpan<char> key) {
            if (key == null) {
                throw new Exception();
            }
            if (_buckets == null) Initialize(0);
            int hashCode = key.GetExHashCode();
            int targetBucket = hashCode & (_buckets.Length - 1);
            for (int i = _buckets[targetBucket]; i >= 0; i = _entries[i].Next) {
                if (_entries[i].HashCode == hashCode && _entries[i].Key.Equals(key) ) {
                    _version++;
                    return false;
                }
            }
            int index;
            if (_freeCount > 0) {
                index = _freeList;
                _freeList = _entries[index].Next;
                _freeCount--;
            }
            else {
                if (_count == _entries.Length) {
                    Resize();
                    targetBucket = hashCode & (_buckets.Length - 1);
                }

                index = _count;
                _count++;
            }

            _entries[index].HashCode = hashCode;
            _entries[index].Next = _buckets[targetBucket];
            _entries[index].Key = key.ToString();
            _buckets[targetBucket] = index;
            _version++;
            return true;
        }

       

        void Resize() {
            var newSize = _count * 2;
            int[] newBuckets = new int[newSize];
            for (int i = 0; i < newBuckets.Length; i++) newBuckets[i] = -1;
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(_entries, 0, newEntries, 0, _count);
            for (int i = 0; i < _count; i++) {
                if (newEntries[i].Key == null) continue;
                int bucket = newEntries[i].HashCode & (newSize - 1);
                newEntries[i].Next = newBuckets[bucket];
                newBuckets[bucket] = i;
            }

            _buckets = newBuckets;
            _entries = newEntries;
        }

       



        public bool Remove(ReadOnlySpan<char> key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }
            if (_buckets != null) {
                int hashCode = key.GetExHashCode();
                int bucket = hashCode & (_buckets.Length - 1);
                int last = -1;
                for (int i = _buckets[bucket]; i >= 0; last = i, i = _entries[i].Next) {
                    if (_entries[i].HashCode == hashCode && _entries[i].Key.Equals(key)) {
                        if (last < 0) {
                            _buckets[bucket] = _entries[i].Next;
                        }
                        else {
                            _entries[last].Next = _entries[i].Next;
                        }
                        _entries[i].HashCode = 0;
                        _entries[i].Next = _freeList;
                        _entries[i].Key = default;
                        _freeList = i;
                        _freeCount++;
                        _version++;
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
            if (_buckets != null) {
                int hashCode = key.GetExHashCode();
                int bucket = hashCode & (_buckets.Length - 1);
                int last = -1;
                for (int i = _buckets[bucket]; i >= 0; last = i, i = _entries[i].Next) {
                    if (_entries[i].HashCode == hashCode && _entries[i].Key == key) {
                        if (last < 0) {
                            _buckets[bucket] = _entries[i].Next;
                        }
                        else {
                            _entries[last].Next = _entries[i].Next;
                        }
                        _entries[i].HashCode = 0;
                        _entries[i].Next = _freeList;
                        _entries[i].Key = default;
                        _freeList = i;
                        _freeCount++;
                        _version++;
                        return true;
                    }
                }
            }

            return false;
        }
        public void Clear() {
            if (_count > 0) {
                for (int i = 0; i < _buckets.Length; i++) _buckets[i] = -1;
                Array.Clear(_entries, 0, _count);
                _freeList = -1;
                _count = 0;
                _freeCount = 0;
                _version++;
            }
        }
      
      
        public bool Contains(string key) {
            int i = FindEntry(key);
            return i >= 0;
        }
        public bool Contains(ReadOnlySpan<char> key) {
            int i = FindEntry(key);
            return i >= 0;
        }
        public bool Contains(int hashCode) {
            if (_buckets != null) {
                
                for (int i = _buckets[hashCode & (_buckets.Length - 1)]; i >= 0; i = _entries[i].Next) {
                    if (_entries[i].HashCode == hashCode) return true;
                }
            }
            return false;
        }

    }
}