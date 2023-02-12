using System;
using System.Runtime.CompilerServices;

namespace YS.Parser {
    public static class NumericParser {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNumber(char c)
        {
            return c is >= '0' and <= '9';
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToHex(char c) {
            if (c >= '0' && c <= '9') {
                return c - '0';
            }

            if ('a' <= c && c <= 'f') {
                return c - 'a'+10;
            }

            if ('A' <= c && c <= 'F') {
                return  c - 'A'+10;
            }
            return -1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlphabet(char c) {
            
            switch (c) {
                case >= 'a' and <= 'z':
                case >= 'A' and <= 'Z':
                    return true;
                default:
                    return false;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSmallCase(char c) {
            return c is >= 'a' and <= 'z';
        }
         [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLargeCase(char c) {
            return c is >= 'A' and <= 'Z';
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IntParseSimple(ReadOnlySpan<char> span)
        {
            long num = 0L;
            int sign = 1;
            var  haveSign = false;
            if (span[0] == '-')
            {
                sign = -1;
                haveSign = true;
            }
            if (span[0] == '+') {
                haveSign = true;
            }
            for (int i =  haveSign ? 1 : 0; i < span.Length && IsNumber(span[i]); i++)
            {
                num = num * 10 + (span[i] - 48);
            }
            checked
            {
                return (int)(unchecked(num * sign));
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IntParse(ReadOnlySpan<char> span) {
          
            if (span.Length > 2&&span[0]=='0') {
                if (span[1] == 'x') {
                    var value = 0L;
                    for (int i = 2; i < span.Length; i++) {
                        var s = span[i];
                        if(span[i] == '_')continue;
                        var hex = ToHex(s);
                        if (hex == -1) return (int)value;
                        value = unchecked(value * 16 +hex);
                    }

                    return (int)value;
                }
                if (span[1] == 'b') {
                    var value = 0L;
                    for (int i = 2; i < span.Length; i++) {
                        var s = span[i];
                        if(s == '_')continue;
                        if (s == '0') {
                            value = unchecked(value *2);
                        }
                        else if(s=='1'){
                            value = unchecked(value *2+1);
                        }
                        else {
                            break;
                        }
                    }

                    return (int)value;
                }
            }
            return (int)DoubleParse(span);
           
        }

       
  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DoubleParse(ReadOnlySpan<char> span)
        {
            var longValue = 0L;
            var value = 0.0;
            var sign = 1;
            var haveSign = false;
            if (span[0] == '-')
            {
                sign = -1;
                haveSign = true;
            }
            if (span[0] == '+') {
                haveSign = true;
            }
            var point = -1;
            var length = span.Length;
            var isOver = false;
            for (int i = haveSign ? 1 : 0; i < span.Length; i++) {
                var s = span[i];
                if(s == '_') {
                    if (point != -1) point++;
                    continue;
                }
                length = i + 1;
                if (s == '.') {
                    if (point != -1) break;
                      point = i;
                      continue;
                }
                var rest = span.Length - length;
                if (rest>0&&(s == 'e' || s == 'E')) {
                    if(!isOver)value = longValue;
                    var multi = 1.0;
                    int log10M=IntParseSimple(span.Slice(length, rest));
                    if (point == -1) {
                        if (log10M>= 0) {
                            for (int j = 0; j <log10M; j++) {
                                multi *= 10;
                            }
                            return value * multi*sign;
                        }
                        for (int j = 0; j < -log10M; j++) {
                            multi /= 10;
                        }
                        return value * multi*sign;
                    }
                    if (log10M-length + point +2 >= 0) {
                        for (int j = 0; j <log10M-length + point +2; j++) {
                            multi *= 10;
                        }
                        return value * multi*sign;
                    }
                    for (int j = 0; j < -log10M-length + point+4; j++) {
                        multi /= 10;
                    }
                    return value * multi*sign;
                }
                if (!IsNumber(s)) break;
                if(!isOver) {
                    longValue = unchecked(longValue * 10 + ( s- '0'));
                    if (longValue > long.MaxValue / 11) {
                        isOver=true;
                        value=longValue;
                    }
                    continue;
                }
                if(point!=-1&&length-point>15) {
                    length--;
                    break;
                }
                value=value * 10 + (s- '0');
            }
            if(!isOver)
                value = longValue;
            if (point != -1) {
                var divider=1L;
                for (int i = 0; i < length-point-1; i++) {
                    divider *= 10L;
                }
                value /= divider;
            }
            return value * sign;
        }
    }
}