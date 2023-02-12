using System;
using System.Collections.Generic;
using System.Linq;
using YS.Collections;
using YS.Modules;
using YS.VM;
using YS.Text;
namespace YS {
    public class NameSpaceTreeRoot :NameSpace {
        public void Clear() {
            Leaves?.Clear();
            Children?.Clear();
        }
        public bool TryGetNameSpace(ReadOnlySpan<char> name, out NameSpace node) {
            node = null;
            var depth = name.Count('.') + 1;
            Children ??=new StringDictionary<NameSpace>();
            if (depth==1) {
                if (Children.TryGetValue(name, out node)) {
                    return true;
                }
            }
            Span<Range> maxRanges = stackalloc Range[depth];
            var array = name.Split('.',maxRanges);
            if (Children.TryGetValue(array[0],out  node)) {
                return node.TryGetNameSpace(array, 1, out node);
            }
            return false;
        }
        public bool TryGetType(ReadOnlySpan<char> name, out Type value) {
            value = default;
            var depth = name.Count('.') + 1;
            if (depth==1) {
                Leaves ??= new StringDictionary<(string name, Type item)>();
                if (Leaves.TryGetValue(name, out var pair)) {
                    value = pair.item;
                    return true;
                }
            }
            Span<Range> maxRanges = stackalloc Range[depth];
            var array = name.Split('.',maxRanges);
            if (Children.TryGetValue(array[0],out var node)) {
                return node.TryGetType(array, 1, out value);
            }
            return false;
        }

        public NameSpaceTreeRoot() {
            Children =new StringDictionary<NameSpace>();
            Leaves = new StringDictionary<(string name, Type item)>();
        }
        public NameSpaceTreeRoot(IEnumerable<(string name, Type item)> pairs,int maxDepth=20) {
            Span<Range> maxRanges = stackalloc Range[maxDepth];
            
            foreach (var pair in pairs) {
                if (!pair.name.Contains('.')) {
                    Leaves[pair.name] = pair;
                }
                else {
                    var array = pair.name.Split('.',maxRanges);
                    if (Children.TryGetValue(array[0],out var node)) {
                        node.Add(array,pair,1);
                    }
                    else {
                        var newNameSpace = new NameSpace(array[0].ToString());
                        newNameSpace.Add(array,pair,1);
                        Children.Add(array[0].ToString(),newNameSpace);
                    }
                }
            }
        }public NameSpaceTreeRoot(IEnumerable<KeyValuePair<string,Type>> pairs,int maxDepth=20) {
            Span<Range> maxRanges = stackalloc Range[maxDepth];
            
            foreach (var pair in pairs) {
                if (!pair.Key.Contains('.')) {
                    
                    Leaves[pair.Key] = (pair.Key,pair.Value);
                }
                else {
                    var array = pair.Key.Split('.',maxRanges);
                    if (Children.TryGetValue(array[0],out var node)) {
                        node.Add(array,(pair.Key,pair.Value),1);
                    }
                    else {
                        var newNameSpace = new NameSpace(array[0].ToString());
                        newNameSpace.Add(array,(pair.Key,pair.Value),1);
                        Children.Add(array[0].ToString(),newNameSpace);
                    }
                }
            }
        }
         public void Add(IEnumerable<(string name, Type item)> pairs,int maxDepth=20) {
             Span<Range> maxRanges = stackalloc Range[maxDepth];
             
             foreach (var pair in pairs) {
                 if (!pair.name.Contains('.')) {
                    
                     Leaves[pair.name] =pair;
                 }
                 else {
                     var array = pair.name.Split('.',maxRanges);
                     var nodeName = array[0];
                     var hash = nodeName.GetHashCode();
                     if (Children.TryGetValue(hash,nodeName,out var node)) {
                         node.Add(array,pair,1);
                     }
                     else {
                         var newNameSpace = new NameSpace(nodeName.ToString());
                         newNameSpace.Add(array,pair,1);
                         Children.Add(hash,nodeName.ToString(),newNameSpace);
                     }
                 }
             }
        }
         public void Add(string name, Type item) {
             var depth = name.AsSpan().Count('.') + 1;
             if (depth==1) {
                 Leaves ??= new StringDictionary<(string name, Type item)>();
                 Leaves[name] = (name,item);
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
                     var newNameSpace = new NameSpace(nodeName.ToString());
                     newNameSpace.Add(array,pair,1);
                     Children.Add(hash,nodeName.ToString(),newNameSpace);
                 }
             }
             
        }
        
        
    }
    public class NameSpace {
            public string Name;
            public StringDictionary<(string name, Type item)> Leaves;
            public StringDictionary< NameSpace> Children;

            public StringDictionary<object> ExtensionMethods;

            public bool StaticClassActivated = false;

            public void Activate() {
                if(StaticClassActivated)return;
                
                if (Leaves != null) {
                    foreach (var pair in Leaves) {
                        var type = pair.Value.item;
                        if (type.IsSealed && type.IsAbstract)
                            ModuleLibrary.GetModule(type);
                    }
                }

                StaticClassActivated = true;

            }
        
            public NameSpace(string nameSpace) {
                Name = nameSpace;
            }
            protected NameSpace() { }

            public void RegisterExtensionMethod(MethodData function) {
                ExtensionMethods ??= new StringDictionary<object>();
                var  name = function.MethodName;
                if (ExtensionMethods.TryGetValue(name,out var member)) {
                    switch (member) {
                        case MethodData last:
                            ExtensionMethods[name] = new List<MethodData>(2) {last, function};
                            break;
                        case List<MethodData> list:
                            list.Add(function);
                            break;
                        default:
                            throw new Exception(member.ToString());
                    }
                }
                else ExtensionMethods[name] = function;
            }
            
            public bool TryGetExtensionMethods(Type type,int hashCode,ReadOnlySpan<char> name,List<MethodData> targetList) {
                if (ExtensionMethods == null) {
                    return false;
                }
                if (ExtensionMethods.TryGetValue(hashCode,name, out var o)) {
                    if (o is MethodData functionData) {
                        if (type.IsAssignableFrom(functionData.ParamData[0].Type)) {
                            targetList.Add(functionData);
                        }
                    }
                    else {
                        var list = (List<MethodData>) o;
                        foreach (var function in list) {
                            if (type.IsAssignableFrom(function.ParamData[0].Type)) {
                                targetList.Add(function);
                            }
                        }
                    }
                    return true;
                }
                return false;
            }
            public void Add(ReadOnlyStringSegmentSpan segmentSpan, (string name, Type item) leaf, int depth) {
                if (depth + 1 == segmentSpan.Length) {
                    Leaves ??= new StringDictionary<(string name, Type item)>();
                    Leaves.Add(segmentSpan[depth].ToString(), leaf);
                }
                else {
                    Children ??= new StringDictionary<NameSpace>();
                    var nodeName = segmentSpan[depth];
                    var hash = nodeName.GetHashCode();
                    if (Children.TryGetValue(hash,nodeName, out var node)) {
                        node.Add(segmentSpan, leaf, depth + 1);
                    }
                    else {
                        var newNameSpace = new NameSpace(segmentSpan[..(depth + 1)].ToString());
                        newNameSpace.Add(segmentSpan, leaf, depth + 1);
                        Children.Add(hash,nodeName.ToString(), newNameSpace);
                    }
                }
            }
            public void Add(ReadOnlySpanSegments<char> segmentSpan, (string name, Type item) leaf, int depth) {
                if (depth + 1 == segmentSpan.Length) {
                    Leaves ??= new StringDictionary<(string name, Type item)>();
                    Leaves.Add(segmentSpan[depth].ToString(), leaf);
                }
                else {
                    Children ??= new StringDictionary<NameSpace>();
                    if (Children.TryGetValue(segmentSpan[depth], out var node)) {
                        node.Add(segmentSpan, leaf, depth + 1);
                    }
                    else {
                        var newNameSpace = new NameSpace(segmentSpan[..(depth + 1)].ToString());
                        newNameSpace.Add(segmentSpan, leaf, depth + 1);
                        Children.Add(segmentSpan[depth].ToString(), newNameSpace);
                    }
                }
            }

            public bool TryGetTypeOrNameSpace(ReadOnlySpanSegments<char> segmentSpan, int depth, out Type value,
                out NameSpace node) {
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
                            return node.TryGetTypeOrNameSpace(segmentSpan, depth + 1, out value, out node);
                        }
                    }
                }
                return false;
            }
            public bool TryGetTypeOrNameSpace(ReadOnlySpan<char> span, out Type value,
                out NameSpace node) {
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
            public bool TryGetTypeOrNameSpace(int hashCode,ReadOnlySpan<char> span, out Type value,
                out NameSpace node) {
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
            public bool TryGetType(int hashCode,ReadOnlySpan<char> span, out Type value) {
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
            public bool TryGetType(ReadOnlySpanSegments<char> segmentSpan, int depth,  out Type value) {
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
                            return node.TryGetType(segmentSpan, depth + 1, out value);
                        }
                    }
                }
                return false;
            }
            public bool TryGetNameSpace(ReadOnlySpanSegments<char> segmentSpan, int depth, 
                out NameSpace node) {
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
                            return node.TryGetNameSpace(segmentSpan, depth + 1, out node);
                        }
                    }
                }
                return false;
            }
        }
}