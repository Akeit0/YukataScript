using System;
using System.Buffers;

namespace YS.Text {
    public static  class StringEx {

        static readonly string[] _alphabets =  {
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"
        };
        static readonly string[] _digits =  {
            "0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20"
        };
        
        public static string ToStringCash(this char c) {
            if (c is <= 'z' and >= 'a') return _alphabets[c - 'a'];
            if (c is <= '9' and >= '0') return _digits[c - '0'];
            return string.Intern(c.ToString());
        }
        public static string ToStringCash(this int i) {
            if(0<=i&&i<21) return _digits[i];
            return i.ToString();
        }
        
        public static ReadOnlyStringSegmentSpan Split(this string source,char separator, Span<Range> maxRanges) {
            var index = 0;
            int start = 0;
            int end = 0;
            int length = source.Length;
            int maxIndex = maxRanges.Length-1;
            while (end<length) {
                if (source[end++] != separator) continue;
                if (index == maxIndex) goto last;
                maxRanges[index++] = start..(end-1);
                while (end<length&&source[end] == separator) {
                    if (index == maxIndex) {
                        start = end;
                        goto last;
                    }
                    maxRanges[index++] = end..end++;
                }
                start = end++;
            }
            last:
            end = length; 
            maxRanges[index++] = start..end;
            return new ReadOnlyStringSegmentSpan(source, maxRanges[..index]);
        } 
        public static ReadOnlySpanSegments<char> Split(this ReadOnlySpan<char> source,char separator, Span<Range> maxRanges) {
            var index = 0;
            int start = 0;
            int end = 0;
            int length = source.Length;
            int maxIndex = maxRanges.Length-1;
            while (end<length) {
                if (source[end++] != separator) continue;
                if (index == maxIndex) goto last;
                maxRanges[index++] = start..(end-1);
                while (end<length&&source[end] == separator) {
                    if (index == maxIndex) {
                        start = end;
                        goto last;
                    }
                    maxRanges[index++] = end..end++;
                }
                start = end++;
            }
            last:
            end = length; 
            maxRanges[index++] = start..end;
            return new ReadOnlySpanSegments<char> (source, maxRanges[..index]);
        }
        public static ReadOnlyStringSegmentSpan Split(this string source,ReadOnlySpan<char> separators, Span<Range> maxRanges) {
            var index = 0;
            int start = 0;
            int end = 0;
            int length = source.Length;
            int maxIndex = maxRanges.Length-1;
            while (end<length) {
                if (!separators.Contains(source[end++])) continue;
                if (index == maxIndex) goto last;
                maxRanges[index++] = start..(end-1);
                while (end<length&&separators.Contains( source[end])) {
                    if (index == maxIndex) {
                        start = end;
                        goto last;
                    }
                    maxRanges[index++] = end..end++;
                }
                start = end++;
            }
            last:
            end = length; 
            maxRanges[index++] = start..end;
            return new ReadOnlyStringSegmentSpan(source, maxRanges[..(index)]);
        }

        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> src) {
            var index = 0;
            if (src.Length == 0) return src;
            while (src[index]==' ') {
                ++index;
            }
            return src.Slice(index);

        }
        public static bool Contains(this ReadOnlySpan<char> span, char target) {
            foreach (var c in span) {
                if (c == target) return true;
            }
            return false;
        }
        public static int Count(this ReadOnlySpan<char> span, char target) {
            int count = 0;
            foreach (var c in span) {
                if (c == target) ++count;
            }
            return count;
        }
        public static string Concat(this ReadOnlySpan<char> left, ReadOnlySpan<char> right) {
            var length = left.Length + right.Length;
            if (length < 512) {
                Span<char> concatSpan = stackalloc char[length];
                left.CopyTo(concatSpan.Slice(0,left.Length));
                right.CopyTo(concatSpan.Slice(left.Length,right.Length));
                return new string(concatSpan);
            }
            else {
                var concatArray=ArrayPool<char>.Shared.Rent(length);
                left.CopyTo(concatArray.AsSpan(0,left.Length));
                right.CopyTo(concatArray.AsSpan(left.Length,right.Length));
                var result=new string(concatArray.AsSpan(0,length));
                ArrayPool<char>.Shared.Return(concatArray);
                return result;
            }
        }

        public static string Concat(this string left, ReadOnlySpan<char> right) => left.AsSpan().Concat(right);
        public static unsafe bool Equals(this string text, ReadOnlySpan<char> span) {
            if (text.Length != span.Length) return false;
            int width = sizeof(nuint) / sizeof(char);
            var count = text.Length / width;
            var rem = text.Length % width;
            fixed (char* ptr1 = text) {
                fixed(char*ptr2=span)
                {
                    var lPtr1 = (nuint*)ptr1;
                    var lPtr2 = (nuint*)ptr2;
                    for (int i = 0; i < count; i++)
                    {
                        if (lPtr1[i] != lPtr2[i])
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < rem; i++)
                    {
                        if ((ptr1)[count * width + i] != (ptr2)[count * width + i])
                        {
                            return false;
                        }
                    }
                    
                }
            }

            return true;
        } 
        public static unsafe bool Equals(this string text, char* ptr,int length) {
            if (text.Length != length) return false;
            int width = sizeof(nuint) / sizeof(char);
            var count = text.Length / width;
            var rem = text.Length % width;
            fixed (char* ptr1 = text) {
                var lPtr1 = (nuint*)ptr1;
                var lPtr2 = (nuint*)ptr;
                for (int i = 0; i < count; i++)
                {
                    if (lPtr1[i] != lPtr2[i])
                    {
                        return false;
                    }
                }
                for (int i = 0; i < rem; i++)
                {
                    if ((ptr1)[count * width + i] != (ptr)[count * width + i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public static  unsafe bool Equals(this ReadOnlySpan<char> left, ReadOnlySpan<char> right) {
            if (left.Length != right.Length) return false;
            int width = sizeof(nuint) / sizeof(char);
            var count = left.Length / width;
            var rem = left.Length % width;
            fixed (char* ptr1 = left) {
                fixed(char*ptr2=right) {
                    var lPtr1 = (nuint*)ptr1;
                    var lPtr2 = (nuint*)ptr2;
                    for (int i = 0; i < count; i++) {
                        if (lPtr1[i] != lPtr2[i]) return false;
                    }
                    if (rem == 0) return true;
                    for (int i = 0; i < rem; i++)
                    {
                        if ((ptr1)[count * width + i] != (ptr2)[count * width + i])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        
        
       public static  unsafe int GetExHashCode(this string t) {
            var length = t.Length;
            switch (length) {
                case 0: return 0;
                case 1: return (((5381 << 5) + 5381) ^ t[0]) * 1566083941;
                case 2:
                    var hash = ((5381 << 5) + 5381) ^ t[0];
                    return ((hash << 5) + hash) ^ t[1] * 1566083941;
                case 3:
                    hash = ((5381 << 5) + 5381) ^ t[0];
                    hash = ((hash << 5) + hash) ^ t[1];
                    return ((hash << 5) + hash) ^ t[2] * 1566083941;
                default:
                    fixed (char* ptr = t) {
                        hash = 5381;
                        var s = (int*) ptr;
                        var end = s + (length + 1) / 2;
                        do {
                            hash = ((hash << 5) + hash) ^ s[0];
                        } while (end > ++s);
                        return (hash * 1566083941);
                    }
            }
        }

       
       public static  unsafe int GetExHashCode(this ReadOnlySpan<char> t) {
            var length = t.Length;
            switch (length) {
                case 0: return 0;
                case 1: return (177573 ^ t[0]) * 1566083941;
                case 2:
                    var hash = ( 177573) ^ t[0];
                    return ((hash << 5) + hash) ^ t[1] * 1566083941;
                case 3:
                    hash =  177573^ t[0];
                    hash = ((hash << 5) + hash) ^ t[1];
                    return ((hash << 5) + hash) ^ t[2] * 1566083941;
                default:
                    fixed (char* ptr = t) {
                        hash = 5381;
                        var s = (int*) ptr;
                        var end = s + length / 2;
                        do {
                            hash = ((hash << 5) + hash) ^ s[0];
                        } while (end > ++s);
                        if (length % 2 != 0) {
                            hash = ((hash << 5) + hash) ^ (s[0] & 0xFFFF);
                        }
                        return (hash * 1566083941);
                    }
            }
        }
        
        
       public static unsafe  int GetExHashCode(char* ptr,int length) {
             switch (length) {
                 case 0: return 0;
                 case 1: return (((5381 << 5) + 5381) ^ ptr[0]) * 1566083941;
                 case 2:
                     var hash = ((5381 << 5) + 5381) ^ ptr[0];
                     return ((hash << 5) + hash) ^ ptr[1] * 1566083941;
                 case 3:
                     hash = ((5381 << 5) + 5381) ^ ptr[0];
                     hash = ((hash << 5) + hash) ^ ptr[1];
                     return ((hash << 5) + hash) ^ ptr[2] * 1566083941;
                 default:
                     hash = 5381;
                     var s = (int*) ptr;
                     var end = s + length / 2;
                     do {
                         hash = ((hash << 5) + hash) ^ s[0];
                     } while (end > ++s);
                     if (length % 2 != 0) {
                         hash = ((hash << 5) + hash) ^ (s[0] & 0xFFFF);
                     }
                     return (hash * 1566083941);
             }
        }
    }
}