﻿using System;

namespace YS.Collections {
    /// <summary>
    /// Set of integer 0 ~ 65535. Smaller memory than HashSet&lt;ushort&gt;.
    ///  If element _count is lower than 1000, binary set is better.
    /// </summary>
    public class UshortSet {
        struct Entry {
            public ushort Next;
            public ushort Key;
        }
        ushort?[] _buckets;
        Entry[] _entries;
        int _count;
        int _version;
        int _freeList;
        int _freeCount;
        public int Count => _count - _freeCount;
        int GetIndex(ushort key) {
            return ((177573 ^ key) * 1566083941)& (_buckets.Length - 1);
        }
        public UshortSet() {
        }
        public UshortSet(int capacity) {
            Initialize(capacity);
        }
        void Initialize(int capacity) {
            int size = 1;
            while (size < capacity) {
                size <<= 1;
            }
            _buckets = new ushort?[size];
            
            _entries = new Entry[size];
            for (ushort i = 0; i < _buckets.Length; i++) {
                _buckets[i] =null;
                _entries[i].Next = i;
            }
            _freeList = -1;
        }
        int FindEntry(ushort key) {
            var i = _buckets?[GetIndex(key)];
            if (i == null) return -1;
            var index = i.Value;
            var current = _entries[index];
            if (current.Key == key) return index;
            while ( current.Next != index) {
                current = _entries[index=current.Next];
                if (current.Key == key) return index;
            }
            return -1;
        }
        public bool Add(ushort key) {
            if (_entries == null) Initialize(4);
            var targetBucket = GetIndex(key);
            var i = _buckets[targetBucket];
            if(i.HasValue) {
                var currentIndex = i.Value;
                var current = _entries[currentIndex];
                if (current.Key == key) return false;
                while ( current.Next != currentIndex) {
                      current = _entries[currentIndex=current.Next];
                    if (current.Key == key) return false;
                }
            }
            int index;
            if (_freeCount > 0) {
                index = _freeList;
                var next = _entries[index].Next;
                _freeList = next==index?-1:next;
                _freeCount--;
            }
            else {
                if (_count >= _entries.Length*0.8f) {
                    Resize();
                    targetBucket =  GetIndex(key );
                }
                index = _count;
                _count++;
            }

            if (_buckets[targetBucket].HasValue)
                _entries[index].Next = (ushort) _buckets[targetBucket];
            else 
                _entries[index].Next = (ushort)index;
            _entries[index].Key = key;
            _buckets[targetBucket] =(ushort) index;
            _version++;
            return true; 
        }
        public bool Remove(ushort key) {
            if (_buckets == null) return false;
            var targetBucket = GetIndex(key);
            var i = _buckets[targetBucket];
            if (!i.HasValue) {
                return false;
            }
            var index = i.Value;
            var current = _entries[index];
            if (current.Key == key) {
                if (current.Next == index) {
                    _buckets[targetBucket] = null;
                }
                else {
                    _buckets[targetBucket] = current.Next;
                }
                _entries[index].Next = _freeList >= 0 ? (ushort) _freeList : index;
                _freeList = index;
                _freeCount++;
                _version++;
                return true;
            }
            while (current.Next != index) {
                int last = index;
                current = _entries[index = current.Next];
                if (current.Key == key) {
                    if (current.Next == index) {
                        _entries[last].Next = (ushort)last;
                    }
                    else {
                        _entries[last].Next = current.Next;
                    }
                    _entries[index].Next = _freeList >= 0 ? (ushort) _freeList : index;
                    _freeList = index;
                    _freeCount++;
                    _version++;
                    return true;
                }
            }

            return false;
        }
        void Resize() {
            var newSize = _count * 2;
            ushort?[] newBuckets = new  ushort?[newSize];
            _buckets = newBuckets;
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(_entries, 0, newEntries, 0, _count);
            for (ushort i = 0; i < _count; i++) {
                int bucket =GetIndex(newEntries[i].Key);
                var bucketTarget = newBuckets[bucket];
                if(bucketTarget.HasValue)
                    newEntries[i].Next = (ushort)bucketTarget;
                else {
                    newEntries[i].Next = i;
                }
                newBuckets[bucket] = i;
            }
            _entries = newEntries;
        }
        public void Clear() {
            if (_count > 0) {
                Array.Clear(_buckets,0,_buckets.Length);
                for (ushort i = 0; i < _count; i++) {
                    _entries[i].Next = i;
                }
                _freeList = -1;
                _count = 0;
                _freeCount = 0;
                _version++;
            }
        }
      
      
        public bool Contains(ushort key) {
            int i = FindEntry(key);
            return i >= 0;
        }
    }
}