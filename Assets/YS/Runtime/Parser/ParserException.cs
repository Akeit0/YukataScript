using System;
using YS.Lexer;

namespace YS.Parser {
    public class ParseException:Exception {
        public ParseException(string ex) : base(ex){}


        public static ParseException Expect(int line, string excepted) {
            return new ParseException($"line {line} ,"+excepted);
        }
        public static ParseException Expect(int line, TokenType excepted,TokenType real) {
            return new ParseException($"line {line} ,"+TokenInfo.ToString(excepted)+" is expected, not "+TokenInfo.ToString(real));
        }
    }
}