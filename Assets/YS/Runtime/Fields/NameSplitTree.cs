using System;
using System.Collections.Generic;
using UnityEngine;
using YS.Collections;
using YS.Text;

namespace YS {
    public class NameSplitTree<T> {
        public  StringDictionary< T> Leaves =new  StringDictionary< T>();
        public  StringDictionary< Node> Children = new  StringDictionary< Node>();
        public void Clear() {
            Leaves.Clear();
            Children.Clear();
        }
        public bool TryGetValue(int hashCode,ReadOnlySpan<char> span, out T value) {
            value = default;
            var segment = span;
            if (Leaves != null) {
                if (Leaves.TryGetValue(hashCode,segment, out  value)) {
                    return true;
                }
            }
            
            return false;
        }
        public bool TryGetValueOrNode(int hashCode,ReadOnlySpan<char> span, out T value,
            out Node node) {
            node = null;
            value = default;
            var segment = span;
            if (Leaves != null) {
                if (Leaves.TryGetValue(hashCode,segment, out  value)) {
                    return true;
                }
            }
            if (Children != null) {
                return Children.TryGetValue(hashCode,segment, out node);
            }
            return false;
        }
        public bool TryGetValueOrNode(ReadOnlySpan<char> name, out T value,out Node node) {
            node = null;
            value = default;
            if (!name.Contains('.')) {
                if (Leaves.TryGetValue(name, out value)) {
                    return true;
                }
                if (Children.TryGetValue(name, out node)) {
                    return true;
                }
            }
            Span<Range> maxRanges = stackalloc Range[20];
            var array = name.Split('.',maxRanges);
            if (Children.TryGetValue(array[0],out  node)) {
                return node.TryGetValueOrNode(array, 1, out value, out node);
            }
            return false;
        }
        public bool TryGetNode(ReadOnlySpan<char> name, out Node node) {
            node = null;
            var depth = name.Count('.') + 1;
            if (depth==1) {
                if (Children.TryGetValue(name, out node)) {
                    return true;
                }
            }
            Span<Range> maxRanges = stackalloc Range[depth];
            var array = name.Split('.',maxRanges);
            if (Children.TryGetValue(array[0],out  node)) {
                return node.TryGetNode(array, 1, out node);
            }
            return false;
        }
        public bool TryGetValue(ReadOnlySpan<char> name, out T value) {
            value = default;
            var depth = name.Count('.') + 1;
            if (depth==1) {
                if (Leaves.TryGetValue(name, out value)) {
                    return true;
                }
            }
            Span<Range> maxRanges = stackalloc Range[depth];
            var array = name.Split('.',maxRanges);
            if (Children.TryGetValue(array[0],out var node)) {
                return node.TryGetValue(array, 1, out value);
            }
            return false;
        }
        public NameSplitTree(){}
        public NameSplitTree(IEnumerable<(string name, T item)> pairs,int maxDepth=20) {
            Span<Range> maxRanges = stackalloc Range[maxDepth];
            
            foreach (var pair in pairs) {
                if (!pair.name.Contains('.')) {
                    Leaves[pair.name] = pair.item;
                }
                else {
                    var array = pair.name.Split('.',maxRanges);
                    if (Children.TryGetValue(array[0],out var node)) {
                        node.Add(array,pair,1);
                    }
                    else {
                        var newNode = new Node(array[0].ToString());
                        newNode.Add(array,pair,1);
                        Children.Add(array[0].ToString(),newNode);
                    }
                }
            }
        }public NameSplitTree(IEnumerable<KeyValuePair<string,T>> pairs,int maxDepth=20) {
            Span<Range> maxRanges = stackalloc Range[maxDepth];
            
            foreach (var pair in pairs) {
                if (!pair.Key.Contains('.')) {
                    Leaves[pair.Key] = pair.Value;
                }
                else {
                    var array = pair.Key.Split('.',maxRanges);
                    if (Children.TryGetValue(array[0],out var node)) {
                        node.Add(array,(pair.Key,pair.Value),1);
                    }
                    else {
                        var newNode = new Node(array[0].ToString());
                        newNode.Add(array,(pair.Key,pair.Value),1);
                        Children.Add(array[0].ToString(),newNode);
                    }
                }
            }
        }
         public void Add(IEnumerable<(string name, T item)> pairs,int maxDepth=20) {
             Span<Range> maxRanges = stackalloc Range[maxDepth];
             
             foreach (var pair in pairs) {
                 if (!pair.name.Contains('.')) {
                     Leaves[pair.name] = pair.item;
                 }
                 else {
                     var array = pair.name.Split('.',maxRanges);
                     var nodeName = array[0];
                     var hash = nodeName.GetHashCode();
                     if (Children.TryGetValue(hash,nodeName,out var node)) {
                         node.Add(array,pair,1);
                     }
                     else {
                         var newNode = new Node(nodeName.ToString());
                         newNode.Add(array,pair,1);
                         Children.Add(hash,nodeName.ToString(),newNode);
                     }
                 }
             }
        }
         public void Add(string name, T item) {
             var depth = name.AsSpan().Count('.') + 1;
             if (depth==1) {
                     Leaves[name] = item;
             }
             else {
                 Span<Range> maxRanges = stackalloc Range[depth];
                 var array = name.Split('.',maxRanges);
                 var nodeName = array[0];
                 var hash = nodeName.GetHashCode();
                 var pair = (name, item);
                 if (Children.TryGetValue(hash,nodeName,out var node)) {
                     node.Add(array,pair,1);
                 }
                 else {
                     var newNode = new Node(nodeName.ToString());
                     newNode.Add(array,pair,1);
                     Children.Add(hash,nodeName.ToString(),newNode);
                 }
             }
             
        }
        
        public class Node {
            public string Name;
            public StringDictionary<(string name, T item)> Leaves;
            public StringDictionary< Node> Children;
            public Node GetDeep() {
                if (Leaves == null && Children.Count == 1) {
                    return Children.First().Value.GetDeep();
                }
                return this;
            }
            public Node(string nameSpace) {
                Name = nameSpace;
            }
            public void Add(ReadOnlyStringSegmentSpan segmentSpan, (string name, T item) leaf, int depth) {
                if (depth + 1 == segmentSpan.Length) {
                    Leaves ??= new StringDictionary<(string name, T item)>();
                    Leaves.Add(segmentSpan[depth].ToString(), leaf);
                }
                else {
                    Children ??= new StringDictionary<Node>();
                    var nodeName = segmentSpan[depth];
                    var hash = nodeName.GetHashCode();
                    if (Children.TryGetValue(hash,nodeName, out var node)) {
                        node.Add(segmentSpan, leaf, depth + 1);
                    }
                    else {
                        var newNode = new Node(segmentSpan[..(depth + 1)].ToString());
                        newNode.Add(segmentSpan, leaf, depth + 1);
                        Children.Add(hash,nodeName.ToString(), newNode);
                    }
                }
            }
            public void Add(ReadOnlySpanSegments<char> segmentSpan, (string name, T item) leaf, int depth) {
                if (depth + 1 == segmentSpan.Length) {
                    Leaves ??= new StringDictionary<(string name, T item)>();
                    Leaves.Add(segmentSpan[depth].ToString(), leaf);
                }
                else {
                    Children ??= new StringDictionary<Node>();
                    if (Children.TryGetValue(segmentSpan[depth], out var node)) {
                        node.Add(segmentSpan, leaf, depth + 1);
                    }
                    else {
                        var newNode = new Node(segmentSpan[..(depth + 1)].ToString());
                        newNode.Add(segmentSpan, leaf, depth + 1);
                        Children.Add(segmentSpan[depth].ToString(), newNode);
                    }
                }
            }

            public bool TryGetValueOrNode(ReadOnlySpanSegments<char> segmentSpan, int depth, out T value,
                out Node node) {
                node = null;
                value = default;
                var segment = segmentSpan[depth];
                var hash = segment.GetExHashCode();
                if (depth + 1 == segmentSpan.Length) {
                    if (Leaves != null) {
                        if (Leaves.TryGetValue(hash,segment, out var pair)) {
                            value = pair.item;
                            return true;
                        }
                    }
                    if (Children != null) {
                        return Children.TryGetValue(hash,segment, out node);

                    }
                }
                else {
                    if (Children != null) {
                        if (Children.TryGetValue(hash,segment, out node)) {
                            return node.TryGetValueOrNode(segmentSpan, depth + 1, out value, out node);
                        }
                    }
                }
                return false;
            }
            public bool TryGetValueOrNode(ReadOnlySpan<char> span, out T value,
                out Node node) {
                node = null;
                value = default;
                var segment = span;
                var hash = segment.GetExHashCode();
                if (Leaves != null) {
                    if (Leaves.TryGetValue(hash,segment, out var pair)) {
                        value = pair.item;
                        return true;
                    }
                }
                if (Children != null) {
                    return Children.TryGetValue(hash,segment, out node);

                }
                return false;
            }
            public bool TryGetValueOrNode(int hashCode,ReadOnlySpan<char> span, out T value,
                out Node node) {
                node = null;
                value = default;
                var segment = span;
                if (Leaves != null) {
                    if (Leaves.TryGetValue(hashCode,segment, out var pair)) {
                        value = pair.item;
                        return true;
                    }
                }
                if (Children != null) {
                    return Children.TryGetValue(hashCode,segment, out node);
                }
                return false;
            }
            public bool TryGetValue(int hashCode,ReadOnlySpan<char> span, out T value) {
                value = default;
                var segment = span;
                if (Leaves != null) {
                    if (Leaves.TryGetValue(hashCode,segment, out var pair)) {
                        value = pair.item;
                        return true;
                    }
                }
                return false;
            }
            public bool TryGetValue(ReadOnlySpanSegments<char> segmentSpan, int depth,  out T value) {
                value = default;
                var segment = segmentSpan[depth];
                var hash = segment.GetExHashCode();
                if (depth + 1 == segmentSpan.Length) {
                    if (Leaves != null) {
                        if (Leaves.TryGetValue(hash, segment, out var pair)) {
                            value = pair.item;
                            return true;
                        }
                    }
                }
                else {
                    if (Children != null) {
                        if (Children.TryGetValue(hash,segment, out var node)) {
                            return node.TryGetValue(segmentSpan, depth + 1, out value);
                        }
                    }
                }
                return false;
            }
            public bool TryGetNode(ReadOnlySpanSegments<char> segmentSpan, int depth, 
                out Node node) {
                node = null;
             
                var segment = segmentSpan[depth];
                var hash = segment.GetExHashCode();
                if (depth + 1 == segmentSpan.Length) {
                   
                    if (Children != null) {
                        return Children.TryGetValue(hash,segment, out node);

                    }
                }
                else {
                    if (Children != null) {
                        if (Children.TryGetValue(hash,segment, out node)) {
                            return node.TryGetNode(segmentSpan, depth + 1, out node);
                        }
                    }
                }
                return false;
            }
        }
    }
    
}