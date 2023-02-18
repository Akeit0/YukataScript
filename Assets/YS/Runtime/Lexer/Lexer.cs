using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using YS.Collections;
using YS.Text;
using static YS.Parser.NumericParser;
namespace YS.Lexer {
    
    public unsafe ref struct Lexer {
        int _currentLine;
        int _position;
        readonly int _length;
        readonly char* _bufferStart;
        char* _buffer;
        readonly SimpleList<TokenInfo> _tokens;
        readonly StringSegmentList _list;

        AddableSpan<char> _builder;
        public static void Tokenize(string code, SimpleList<TokenInfo> tokens, StringSegmentList list) {
            tokens.Clear();
            list.Clear();
            list.Source = code;
            var spanList = new AddableSpan<char>(stackalloc char[30]);
            fixed (char* c = code) {
                var lexer = new Lexer(c, code.Length, tokens, list,spanList);
                lexer.Tokenize();
            }
        }

        Lexer(char* buffer, int length, SimpleList<TokenInfo> tokens, StringSegmentList list,AddableSpan<char> builder) {
            _currentLine = 1;
            _position = 0;
            _length = length;
            _bufferStart = buffer;
            _buffer = buffer;
            _list = list;
            _tokens = tokens;
            _builder = default;
            _builder = builder ;
        }

        bool AtEof => _position >= _length;

        void Tokenize() {
            SkipSpace();
            var i = 0;
            while (LexNext()) {
                if (1000 < i++) throw new Exception("over");
            }

            _tokens.Add(TokenInfo.New(TokenType.EndOfLineToken, _currentLine));
            _tokens.Add(TokenInfo.New(TokenType.EndOfFileToken));
        }


        bool LexNext() {
            if (ToSkip(_buffer)) {
                goto NextLabel;
            }

            var character = *_buffer;
            switch (character) {
                case '/':
                    AdvanceUnsafe();
                    if (_buffer[0] == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.SlashEqualsToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.SlashToken));
                    }

                    break;
                case '.':
                    AdvanceUnsafe();
                    if (_buffer[0] == '.') {
                        AdvanceUnsafe();
                        if (_buffer[0] == '.') {
                            throw new Exception("...");
                        }

                        _tokens.Add(TokenInfo.New(TokenType.DotDotToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.DotToken));
                    }


                    break;
                case ',':
                    AdvanceUnsafe();
                    _tokens.Add(TokenInfo.New(TokenType.CommaToken));
                    break;

                case ':':
                    AdvanceUnsafe();
                    if (_buffer[0] == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.ColonEqualsToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.ColonToken));
                    }

                    break;
                case ';':
                    AdvanceUnsafe();
                    _tokens.Add(TokenInfo.New(TokenType.SemicolonToken));
                    break;

                case '~':
                    AdvanceUnsafe();
                    _tokens.Add(TokenInfo.New(TokenType.TildeToken));
                    break;

                case '!':
                    AdvanceUnsafe();
                    if (_buffer[0] == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.ExclamationEqualsToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.ExclamationToken));
                    }

                    break;

                case '=':
                    AdvanceUnsafe();
                    if ((character = _buffer[0]) == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.EqualsEqualsToken));
                    }
                    else if (character == '>') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.EqualsGreaterThanToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.EqualsToken));
                    }

                    break;

                case '*':
                    AdvanceUnsafe();
                    if (_buffer[0] == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.AsteriskEqualsToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.AsteriskToken));
                    }

                    break;

                case '(':
                    AdvanceUnsafe();
                    _tokens.Add(TokenInfo.New(TokenType.OpenParenToken));
                    break;

                case ')':
                    AdvanceUnsafe();
                    _tokens.Add(TokenInfo.New(TokenType.CloseParenToken));
                    break;

                case '{':
                    AdvanceUnsafe();
                    _tokens.Add(TokenInfo.New(TokenType.OpenBraceToken));
                    break;

                case '}':
                    AdvanceUnsafe();
                    _tokens.Add(TokenInfo.New(TokenType.CloseBraceToken));
                    break;

                case '[':
                    AdvanceUnsafe();
                    _tokens.Add(TokenInfo.New(TokenType.OpenBracketToken));
                    break;

                case ']':
                    AdvanceUnsafe();
                    _tokens.Add(TokenInfo.New(TokenType.CloseBracketToken));
                    break;

                case '?':
                    AdvanceUnsafe();
                    if (_buffer[0] == '?') {
                        AdvanceUnsafe();
                        if (_buffer[0] == '=') {
                            AdvanceUnsafe();
                            _tokens.Add(TokenInfo.New(TokenType.QuestionQuestionEqualsToken));
                        }
                        else {
                            _tokens.Add(TokenInfo.New(TokenType.QuestionQuestionToken));
                        }
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.QuestionToken));
                    }

                    break;

                case '+':
                    AdvanceUnsafe();
                    if ((character = _buffer[0]) == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.PlusEqualsToken));
                    }
                    else if (character == '+') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.PlusPlusToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.PlusToken));
                    }

                    break;

                case '-':
                    AdvanceUnsafe();
                    if ((character = _buffer[0]) == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.MinusEqualsToken));
                    }
                    else if (character == '-') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.MinusMinusToken));
                    }
                    else if (character == '>') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.MinusGreaterThanToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.MinusToken));
                    }

                    break;

                case '%':
                    AdvanceUnsafe();
                    if (_buffer[0] == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.PercentEqualsToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.PercentToken));
                    }

                    break;

                case '&':
                    AdvanceUnsafe();
                    if ((character = _buffer[0]) == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.AmpersandEqualsToken));
                    }
                    else if (character == '&') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.AmpersandAmpersandToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.AmpersandToken));
                    }

                    break;

                case '^':
                    AdvanceUnsafe();
                    if (_buffer[0] == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.CaretEqualsToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.CaretToken));
                    }

                    break;

                case '|':
                    AdvanceUnsafe();
                    if (_buffer[0] == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.BarEqualsToken));
                    }
                    else if (_buffer[0] == '|') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.BarBarToken));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.BarToken));
                    }

                    break;
                case '<':
                    AdvanceUnsafe();
                    if (_buffer[0] == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.LessThanEqualsToken));
                    }
                    else if (_buffer[0] == '<') {
                        AdvanceUnsafe();
                        if (_buffer[0] == '=') {
                            AdvanceUnsafe();
                            _tokens.Add(TokenInfo.New(TokenType.LessThanLessThanEqualsToken));
                        }
                        else {
                            _tokens.Add(TokenInfo.New(TokenType.LessThanLessThanToken));
                        }
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.LessThanToken));
                    }

                    break;

                case '>':
                    AdvanceUnsafe();
                    if (_buffer[0] == '=') {
                        AdvanceUnsafe();
                        _tokens.Add(TokenInfo.New(TokenType.GreaterThanEqualsToken));
                    }
                    else if (_buffer[0] == '>') {
                        AdvanceUnsafe();
                        if (_buffer[0] == '=') {
                            AdvanceUnsafe();
                            _tokens.Add(TokenInfo.New(TokenType.GreaterThanGreaterThanEqualsToken));
                        }
                        else {
                            _tokens.Add(TokenInfo.New(TokenType.GreaterThanGreaterThanToken));
                        }
                    }
                    else {
                        _tokens.Add(TokenInfo.New(TokenType.GreaterThanToken));
                    }

                    break;

                case '@':
                    AdvanceUnsafe();
                    break;

                case '$':
                    AdvanceUnsafe();
                    break;
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                    var length = 0;
                    while (!ToSkip(_buffer) && IsUsableForName(*_buffer)) {
                        ++length;
                        if (Advance()) continue;
                        break;
                    }

                    if (length != 0) {
                        if (KeyWordDictionary.TryGetValue(_buffer - length, length, out var tokenType2)) {
                            _tokens.Add(TokenInfo.New(tokenType2));
                        }
                        else {
                            _tokens.Add(TokenInfo.New(TokenType.Identifier, _list.Count));
                            var start = (int)(_buffer - _bufferStart- length);
                            _list.Add(start..(start + length));
                        }
                    }

                    break;
                case '0':
                    LexDigit0();
                    break;
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    LexDigit();
                    break;
                default:
                    length = 0;
                    while (!ToSkip(_buffer) && IsUsableForName(*_buffer)) {
                        ++length;
                        if (Advance()) continue;
                        break;
                    }

                    if (length != 0) {
                        _tokens.Add(TokenInfo.New(TokenType.Identifier, _list.Count));
                        var start = (int)(_buffer - _bufferStart- length);
                        _list.Add(start..(start + length));
                    }

                    break;
            }

            NextLabel:
            SkipSpace();
            return !AtEof;
        }


        bool LexDigit0() {
            var next = _buffer[1];
            switch (next) {
                case 'f':
                    Advance(2);
                    _tokens.Add(TokenInfo.New(0f));
                    return true;
                case 'd':
                    Advance(2);
                    _tokens.Add(TokenInfo.New(0d));
                    return true;
            }

            if (CannotBeANumber(next) || (next == '.' && _buffer[2] == '.')) {
                _tokens.Add(TokenInfo.New(0));
                Advance();
                return true;
            }

            _builder.Clear();
            bool isHex = false;
            bool isBinary = false;
            bool hasDecimal = false;
            bool hasExponent = false;
            var ch = next;
            if (ch == 'x' || ch == 'X') {
                Advance(2);
                isHex = true;
            }
            else if (ch == 'b' || ch == 'B') {
                Advance(2);
                isBinary = true;
            }


            if (isHex || isBinary) {
                ScanNumericLiteralSingleInteger(isHex, isBinary);
                var valueText = _builder.AsSpan();
                if (isHex) {
                    _tokens.Add(TokenInfo.New(TokenType.IntLiteral, int.Parse(valueText, NumberStyles.HexNumber)));
                }
                else {
                    if (TryParseBinaryUInt64(valueText, out var val)) {
                        _tokens.Add(TokenInfo.New((int)val));
                    }
                }
            }
            else {
                ScanNumericLiteralSingleIntegerDecimal();

                if (_buffer[0] == '.') {
                    if ((next = _buffer[1]) >= '0' && next <= '9') {
                        _builder.Add('.');
                        hasDecimal = true;
                        Advance();
                        ScanNumericLiteralSingleIntegerDecimal();
                    }
                    else if (next == '.') {
                        _tokens.Add(TokenInfo.New(int.Parse(_builder.ToString())));
                        return true;
                    }
                }

                if ((ch = _buffer[0]) == 'E' || ch == 'e') {
                    _builder.Add(ch);
                    Advance();
                    hasExponent = true;
                    if ((ch = _buffer[0]) == '-' || ch == '+') {
                        _builder.Add(ch);
                        Advance();
                    }

                    if (((ch = _buffer[0]) >= '0' && ch <= '9') || ch == '_') {
                        ScanNumericLiteralSingleIntegerDecimal();
                    }
                    else {
                        throw new Exception();
                    }
                }

                var valueText = _builder.AsSpan();
                if (hasExponent || hasDecimal) {
                    if ((ch = _buffer[0]) == 'f' || ch == 'F') {
                        Advance();
                        _tokens.Add(TokenInfo.New(
                            float.Parse(valueText, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)));
                    }
                    else if (ch == 'D' || ch == 'd') {
                        Advance();
                        _tokens.Add(TokenInfo.New(
                            double.Parse(valueText, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)));
                    }
                    else if (ch == 'm' || ch == 'M') {
                        Advance();
                        _tokens.Add(TokenInfo.New(
                            decimal.Parse(valueText, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(
                            double.Parse(valueText, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)));
                    }
                }
                else if ((ch = _buffer[0]) == 'f' || ch == 'F') {
                    Advance();
                    _tokens.Add(TokenInfo.New(float.Parse(valueText)));
                }
                else if (ch == 'D' || ch == 'd') {
                    Advance();
                    _tokens.Add(TokenInfo.New( double.Parse(valueText)));
                }
                else if (ch == 'm' || ch == 'M') {
                    Advance();
                    _tokens.Add(TokenInfo.New( decimal.Parse(valueText)));
                }
                else if (ch == 'L' || ch == 'l') {
                    if (ch == 'l') {
                        throw new Exception();
                    }

                    Advance();
                    if ((ch = _buffer[0]) == 'u' || ch == 'U') {
                        Advance();

                        _tokens.Add(TokenInfo.New( ulong.Parse(valueText)));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(long.Parse(valueText)));
                    }
                }
                else if (ch == 'u' || ch == 'U') {
                    Advance();
                    if ((ch = _buffer[0]) == 'L' || ch == 'l') {
                        Advance();
                        _tokens.Add(TokenInfo.New(ulong.Parse(valueText)));
                    }
                    else {
                        _tokens.Add(TokenInfo.New(long.Parse(valueText)));
                    }
                }
                else {
                    _tokens.Add(TokenInfo.New(int.Parse(valueText)));
                }
            }

            return false;
        }

        bool LexDigit() {
            char ch = _buffer[0];
            var next = _buffer[1];
            switch (next) {
                case 'f':
                    Advance(2);
                    _tokens.Add(TokenInfo.New((float)(ch - '0')));
                    return true;
                case 'd':
                    Advance(2);
                    _tokens.Add(TokenInfo.New((double)(ch - '0')));
                    return true;
            }
            if (CannotBeANumber(next) || (next == '.' && _buffer[2] == '.')) {
                _tokens.Add(TokenInfo.New( (ch - '0')));
                Advance();
                return true;
            }

            _builder.Clear();
            bool hasDecimal = false;
            bool hasExponent = false;
            ScanNumericLiteralSingleIntegerDecimal();
            if (_buffer[0] == '.') {
                if ((next = _buffer[1]) >= '0' && next <= '9') {
                    _builder.Add('.');
                    hasDecimal = true;
                    Advance();
                    ScanNumericLiteralSingleIntegerDecimal();
                }
                else if (next == '.') {
                    _tokens.Add(TokenInfo.New( int.Parse(_builder.ToString())));
                    return true;
                }
            }

            if ((ch = _buffer[0]) == 'E' || ch == 'e') {
                _builder.Add(ch);
                Advance();
                hasExponent = true;
                if ((ch = _buffer[0]) == '-' || ch == '+') {
                    _builder.Add(ch);
                    Advance();
                }

                if (((ch = _buffer[0]) >= '0' && ch <= '9') || ch == '_') {
                    ScanNumericLiteralSingleIntegerDecimal();
                }
                else {
                    throw new Exception();
                }
            }

            var valueText = (ReadOnlySpan<char>)_builder.AsSpan();

            if (hasExponent || hasDecimal) {
                if ((ch = _buffer[0]) == 'f' || ch == 'F') {
                    Advance();
                    _tokens.Add(TokenInfo.New(
                        float.Parse(valueText, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)));
                }
                else if (ch is 'd' or 'D') {
                    Advance();
                    _tokens.Add(TokenInfo.New(
                        double.Parse(valueText, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)));
                }
                else if (ch is 'm' or 'M') {
                    Advance();
                    _tokens.Add(TokenInfo.New(
                        decimal.Parse(valueText, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)));
                }
                else {
                    _tokens.Add(TokenInfo.New(
                        double.Parse(valueText, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)));
                }
            }
            else if ((ch = _buffer[0]) == 'f' || ch == 'F') {
                Advance();
                _tokens.Add(TokenInfo.New(float.Parse(valueText)));
            }
            else if (ch is 'd' or 'D') {
                Advance();
                _tokens.Add(TokenInfo.New(double.Parse(valueText)));
            }
            else if (ch is 'm' or 'M') {
                Advance();
                _tokens.Add(TokenInfo.New(decimal.Parse(valueText)));
            }
            else if (ch is 'L' or 'l') {
                if (ch == 'l') {
                    throw new Exception();
                }

                Advance();
                if ((ch = _buffer[0]) == 'u' || ch == 'U') {
                    Advance();

                    _tokens.Add(TokenInfo.New(ulong.Parse(valueText)));
                }
                else {
                    _tokens.Add(TokenInfo.New(long.Parse(valueText)));
                }
            }
            else if (ch == 'u' || ch == 'U') {
                Advance();
                if ((ch = _buffer[0]) == 'L' || ch == 'l') {
                    Advance();
                    _tokens.Add(TokenInfo.New(ulong.Parse(valueText)));
                }
                else {
                    _tokens.Add(TokenInfo.New(long.Parse(valueText)));
                }
            }
            else {
                _tokens.Add(TokenInfo.New(int.Parse(valueText)));
            }


            return false;
        }

        void ScanNumericLiteralSingleInteger(bool isHex, bool isBinary) {
            if (_buffer[0] == '_') {
                if (isHex || isBinary) {
                }
                else {
                    throw new Exception("underscoreInWrongPlace");
                }
            }

            bool lastCharWasUnderscore = false;
            while (true) {
                char ch = _buffer[0];
                if (ch == '_') {
                    lastCharWasUnderscore = true;
                }
                else if (!(isHex ? 0 <= ToHex(ch) :
                             isBinary ? ch is '0' or '1' :
                             IsNumber(ch))) {
                    break;
                }
                else {
                    _builder.Add(ch);
                    lastCharWasUnderscore = false;
                }

                Advance();
            }

            if (lastCharWasUnderscore) {
                throw new Exception("underscoreInWrongPlace");
            }
        }

        void ScanNumericLiteralSingleIntegerDecimal() {
            bool lastCharWasUnderscore = false;
            while (true) {
                char ch = _buffer[0];
                if (ch == '_') {
                    lastCharWasUnderscore = true;
                }
                else if (!IsNumber(ch)) {
                    break;
                }
                else {
                    _builder.Add(ch);
                    lastCharWasUnderscore = false;
                }

                Advance();
            }

            if (lastCharWasUnderscore) {
                throw new Exception("underscoreInWrongPlace");
            }
        }

        static bool TryParseBinaryUInt64(ReadOnlySpan<char> text, out ulong value) {
            value = 0;
            foreach (char c in text) {
                if ((value & 0x8000000000000000) != 0) {
                    return false;
                }

                var bit = (ulong) (c - '0');
                value = (value << 1) | bit;
            }

            return true;
        }

        static bool ToSkip(char* buffer) {
            if (buffer[0] == '/') {
                return (buffer[1] is '/' or '*');
            }
            return buffer[0] is '"' or ' ' or '\t' or '\r' or '\n' or '\'';
        }

        static bool IsEnd(char c) {
            return (c is ' ' or '\t' or '\r' or '\n');
        }

        static bool CannotBeANumber(char c) {
            return c is not ('.' or '_' or (>= '0' and < '9') or (>= 'A' and < 'z'));
        }

        void SkipSpace() {
            bool isEnd = false;
            while (!isEnd) {
                isEnd = true;
                while (IsEnd(*_buffer)) {
                    if (!AdvanceWithToken()) return;
                    isEnd = false;
                }
                if (*_buffer == '/') {
                    var next = *(_buffer + 1);
                    switch (next) {
                        case '/':
                            Advance(2);
                            SkipComment();
                            break;
                        case '*':
                            Advance(2);
                            SkipMultiLineComment();
                            break;
                        default: continue;
                    }

                    isEnd = false;
                }

                if (*_buffer == '"') {
                    if (!Advance()) return;
                    var currentPos = _position;
                    SkipString();
                    var length = _position - currentPos;
                    _tokens.Add(TokenInfo.New(TokenType.StringLiteral, _list.Count));
                    var start = (int)(_buffer - _bufferStart- length);
                    _list.Add(start..(start + length-1));
                    isEnd = false;
                }

                if (*_buffer == '\'') {
                    if (!Advance()) return;
                    var currentPos = _position;
                    SkipChar();
                    var length = _position - currentPos;
                    if (length == 1) {
                        _tokens.Add(TokenInfo.New( *(_buffer - 1)));
                    }
                    else {
                        var t = Regex.Unescape(new string((_buffer - length), 0, length - 1));
                        _tokens.Add(TokenInfo.New( t[0]));
                    }

                    isEnd = false;
                }
            }
        }

        void SkipComment() {
            while (!(*_buffer is '\n' or '\r') && Advance()) {
            }
        }

        void SkipMultiLineComment() {
            while (true) {
                if (*_buffer == '*' && *(_buffer + 1) == '/') {
                    Advance();
                    Advance();
                    return;
                }

                if (!Advance()) {
                    throw new Exception("comment is not end");
                }
            }
        }

        void SkipString() {
            while (true) {
                if (*_buffer == '"') {
                    Advance();
                    return;
                }

                Advance();
            }
        }

        void SkipChar() {
            while (true) {
                if (*_buffer == '\'') {
                    Advance();
                    return;
                }

                Advance();
            }
        }

        bool Advance() {
            if (*_buffer == '\n') {
                ++_currentLine;
            }

            ++_position;
            ++_buffer;
            return !(AtEof);
        }

        void AdvanceUnsafe() {
            ++_position;
            ++_buffer;
        }

        bool AdvanceWithToken() {
            if (*_buffer == '\n') {
                _tokens.Add(TokenInfo.New(TokenType.EndOfLineToken, _currentLine));
                ++_currentLine;
            }

            ++_position;
            ++_buffer;
            return !(AtEof);
        }

        bool Advance(int count) {
            _position += count;
            _buffer += count;
            return !(AtEof);
        }

        



        public static bool IsUsableForName(char c) {
            return c switch {
                >= '0' and <= '9' => true,
                >= 'a' and <= 'z' => true,
                >= 'A' and <= 'Z' => true,
                '_' => true,
                _ => c > '\u009F'
            };
        }

        public static bool IsComparer(TokenType type) {
            switch (type) {
                case TokenType.EqualsEqualsToken:
                case TokenType.ExclamationEqualsToken:
                case TokenType.GreaterThanToken:
                case TokenType.LessThanToken:
                case TokenType.GreaterThanEqualsToken:
                case TokenType.LessThanEqualsToken: return true;
            }


            return false;
        }

        public static FixedArrayDictionary<TokenType> KeyWordDictionary = new FixedArrayDictionary<TokenType>(
            new KeyValuePair<string, TokenType>[] {
                new("var", TokenType.VarKeyword),
                new("global", TokenType.GlobalKeyword),
                new("using", TokenType.UsingKeyword),
                new("nameof", TokenType.NameofKeyword),
                new("typeof", TokenType.TypeOfKeyword),
                new("nuint", TokenType.NuintKeyword),
                new("static", TokenType.StaticKeyword),
                new("public", TokenType.PublicKeyword),
                new("private", TokenType.PrivateKeyword),
                new("void", TokenType.VoidKeyword),
                new("true", TokenType.TrueKeyword),
                new("false", TokenType.FalseKeyword),
                new("async", TokenType.AsyncKeyword),
                new("await", TokenType.AwaitKeyword),
                new("class", TokenType.ClassKeyword),
                new("if", TokenType.IfKeyword),
                new ("while",TokenType.WhileKeyword),
                new("not", TokenType.NotKeyword),
                new("in", TokenType.InKeyword),
                new("new", TokenType.NewKeyword),
                new("else", TokenType.ElseKeyword),
                new("for", TokenType.ForKeyword),
                new("return", TokenType.ReturnKeyword),
                new("break", TokenType.BreakKeyword),
                new("continue", TokenType.ContinueKeyword),
                new("bool", TokenType.BoolKeyword),
                new("int", TokenType.IntKeyword),
                new("float", TokenType.FloatKeyword),
                new("object", TokenType.ObjectKeyword),
                new("string", TokenType.StringKeyword),
                new("double", TokenType.DoubleKeyword),
                new("decimal", TokenType.DecimalKeyword),
                new("long", TokenType.LongKeyword),
                new("ulong", TokenType.ULongKeyword),
                new("default", TokenType.DefaultKeyword),
                new("null", TokenType.NullKeyword),
                new("const", TokenType.ConstKeyword),
                new("ref", TokenType.RefKeyword),
                new("in", TokenType.InKeyword),
                new("out", TokenType.OutKeyword),
            }
        );
    }
}