using System;
using System.Collections;
using System.Collections.Generic;
namespace YS.Collections {
    public class TypeKeyAddOnlyDictionary<TValue> {
        struct Entry {
            public int HashCode;
            public int Next;
            public Type Key;
            public TValue Value;
        }
        int[] _buckets;
        Entry[] _entries;
        int _count;

        public int Count => _count;

        public TypeKeyAddOnlyDictionary() {
            
        }
        public TypeKeyAddOnlyDictionary(int capacity) {
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
        }

       
      
        int FindEntry(Type key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }
            if (_buckets != null) {
                int hashCode = key.GetHashCode();
                for (int i = _buckets[hashCode & (_buckets.Length - 1)]; i >= 0; i = _entries[i].Next) {
                    if (_entries[i].HashCode == hashCode && (_entries[i].Key == key)) return i;
                }
            }
            return -1;
        }
        int FindEntry(int hashCode,Type key) {
            if (key == null) {
                throw new NullReferenceException(nameof(key));
            }
            if (_buckets != null) {
                for (int i = _buckets[hashCode & (_buckets.Length - 1)]; i >= 0; i = _entries[i].Next) {
                    if (_entries[i].HashCode == hashCode && (_entries[i].Key == key)) return i;
                }
            }
            return -1;
        }

        void Insert(Type key, TValue value, bool add) {
            if (key == null) {
                throw new Exception();
            }
           
            if (_buckets == null) Initialize(0);
            int hashCode = key.GetHashCode();
            int targetBucket = hashCode & (_buckets.Length - 1);
            for (int i = _buckets[targetBucket]; i >= 0; i = _entries[i].Next) {
                if (_entries[i].HashCode == hashCode && _entries[i].Key == key) {
                    if (add) throw new ArgumentException(key.ToString());
                    _entries[i].Value = value;
                   
                    return;
                }
            }
            if (_count >= _entries.Length*0.8f) {
                Resize();
                targetBucket = hashCode & (_buckets.Length - 1);
            }

            var index = _count;
            _count++;
            _entries[index].HashCode = hashCode;
            _entries[index].Next = _buckets[targetBucket];
            _entries[index].Key = key;
            _entries[index].Value = value;
            _buckets[targetBucket] = index;
        }
        void Insert(int hashCode,Type key, TValue value, bool add) {
            if (key == null) {
                throw new Exception();
            }
            if (_buckets == null) Initialize(0);
            int targetBucket = hashCode & (_buckets.Length - 1);
            for (int i = _buckets[targetBucket]; i >= 0; i = _entries[i].Next) {
                if (_entries[i].HashCode == hashCode && _entries[i].Key == key) {
                    if (add) throw new ArgumentException(key.ToString());
                    _entries[i].Value = value;
                
                    return;
                }
            }
            int index;
            {
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
            _entries[index].Value = value;
            _buckets[targetBucket] = index;
        }
        
       public bool TryAdd(Type key, TValue value) {
            if (key == null) {
                throw new Exception();
            }

            if (_buckets == null) Initialize(0);
            int hashCode = key.GetHashCode();
            int targetBucket = hashCode & (_buckets.Length - 1);
            for (int i = _buckets[targetBucket]; i >= 0; i = _entries[i].Next) {
                if (_entries[i].HashCode == hashCode && _entries[i].Key == key) {
                    return false;
                }
            }

            int index;
            {
                if (_count >= _entries.Length*0.8f) {
                    Resize();
                    targetBucket = hashCode & (_buckets.Length - 1);
                }

                index = _count;
                _count++;
            }

            _entries[index].HashCode = hashCode;
            _entries[index].Next = _buckets[targetBucket];
            _entries[index].Key = key;
            _entries[index].Value = value;
            _buckets[targetBucket] = index;
            return true;
        }

        void Resize() {
            var newSize = _count * 2;
            int[] newBuckets = new int[newSize];
            for (int i = 0; i < newBuckets.Length; i++) newBuckets[i] = -1;
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(_entries, 0, newEntries, 0, _count);
            for (int i = 0; i < _count; i++) {
                int bucket = newEntries[i].HashCode & (newSize - 1);
                newEntries[i].Next = newBuckets[bucket];
                newBuckets[bucket] = i;
            }

            _buckets = newBuckets;
            _entries = newEntries;
        }

        public void Add(Type key, TValue value) {
            Insert(key, value, true);
        }
     
      public bool TryGetValue(int hashCode,Type key, out TValue value) {
            int i = FindEntry(hashCode,key);
            if (i >= 0) {
                value = _entries[i].Value;
                return true;
            }

            value = default;
            return false;
        }
      

        public bool TryGetValue(Type key, out TValue value) {
            int i = FindEntry(key);
            if (i >= 0) {
                value = _entries[i].Value;
                return true;
            }

            value = default;
            return false;
        }

        public TValue this[Type key] {
            get {
                int i = FindEntry(key);
                if (i >= 0) {
                   return _entries[i].Value;
                }
                throw new KeyNotFoundException(key.ToString());
            }
            set => Insert(key, value, false);
        }
        

        public bool ContainsKey(Type key) {
            int i = FindEntry(key);
            return i >= 0;
        }
    }
}