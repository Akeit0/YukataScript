using System.Collections.Generic;
using System.Runtime.InteropServices;
using YS.Collections;
using YS.Text;
namespace YS.Lexer {
    public enum TokenType:byte {
        Invalid,
        
        ExclamationToken,
        TildeToken,
        
        PlusToken,
        MinusToken,
        SlashToken,
        AsteriskToken,
        PercentToken,
        AmpersandToken,
        BarToken,
        CaretToken,
        LessThanLessThanToken,
        GreaterThanGreaterThanToken,
        QuestionQuestionToken,
        
        EqualsEqualsToken,
        ExclamationEqualsToken,
        LessThanEqualsToken,
        GreaterThanEqualsToken,
        LessThanToken,
        GreaterThanToken,
        
        AmpersandAmpersandToken,
        BarBarToken,
        
        EqualsToken,
        PlusEqualsToken,
        MinusEqualsToken,
        SlashEqualsToken,
        AsteriskEqualsToken,
        PercentEqualsToken,
        AmpersandEqualsToken,
        BarEqualsToken,
        CaretEqualsToken,
        LessThanLessThanEqualsToken,
        GreaterThanGreaterThanEqualsToken,
        QuestionQuestionEqualsToken,
        
        CloseBraceToken,
        CloseBracketToken,
        CloseParenToken,
        ColonColonToken,
        ColonToken,
        
        ColonEqualsToken,
        
        CommaToken,
        DollarToken,
        DotDotToken,
        DotToken,
        DoubleQuoteToken,
        
        EqualsGreaterThanToken,
        
        HashToken,
        InterpolatedStringStartToken,
        InterpolatedVerbatimStringStartToken,
        MinusGreaterThanToken,
        MinusMinusToken,
        OpenBraceToken,
        OpenBracketToken,
        OpenParenToken,
        
        PlusPlusToken,
        
        QuestionToken,
        SemicolonToken,
        SingleQuoteToken,
        UnderscoreToken,
       
        FnKeyword, //Original,  Not CSharp
        
        BoolKeyword,
        ByteKeyword,
        CharKeyword,
        DecimalKeyword,
        DoubleKeyword,
        FloatKeyword,
        IntKeyword,
        LongKeyword,
        ObjectKeyword,
        SbyteKeyword,
        ShortKeyword,
        StringKeyword,
        UIntKeyword,
        ULongKeyword,
        UshortKeyword,
        VoidKeyword,
        NintKeyword,
        NuintKeyword,
        
        FalseKeyword,
        NullKeyword,
        TrueKeyword,
        DefaultKeyword,
        BreakKeyword,
        ContinueKeyword,
        
        
        
        AbstractKeyword,
        AsKeyword,
        BaseKeyword,
        
        CaseKeyword,
        CatchKeyword,
        
        CheckedKeyword,
        ClassKeyword,
        ConstKeyword,
       
       
        DelegateKeyword,
        DoKeyword,
        
        ElseKeyword,
        EnumKeyword,
        EventKeyword,
        ExplicitKeyword,
        ExternKeyword,
       
        FinallyKeyword,
        FixedKeyword,
        
        ForKeyword,
        ForEachKeyword,
        GotoKeyword,
        IfKeyword,
        ImplicitKeyword,
        InKeyword,
        
        InterfaceKeyword,
        InternalKeyword,
        IsKeyword,
        LockKeyword,
        
        NamespaceKeyword,
        NewKeyword,
        
        
        InstructionKeyword,
        OutKeyword,
        OverrideKeyword,
        ParamsKeyword,
        PrivateKeyword,
        ProtectedKeyword,
        PublicKeyword,
        ReadOnlyKeyword,
        RefKeyword,
        ReturnKeyword,
        
        SealedKeyword,
        
        SizeOfKeyword ,
        StackallocKeyword,
        StaticKeyword,
       
        StructKeyword,
        SwitchKeyword,
        ThisKeyword,
        ThrowKeyword,
        
        TryKeyword,
        TypeOfKeyword,
        
        UncheckedKeyword,
        UnsafeKeyword,
        
        UsingKeyword,
        VirtualKeyword,
        
        VolatileKeyword,
        WhileKeyword,
        
        AddKeyword,
        AndKeyword,
        AliasKeyword,
        AscendingKeyword,
        ArgsKeyword,
        AsyncKeyword,
        AwaitKeyword,
        ByKeyword,
        DescendingKeyword,
        DynamicKeyword,
        EqualsKeyword,
        FromKeyword,
        GetKeyword,
        GlobalKeyword,
        GroupKeyword,
        InitKeyword,
        IntoKeyword,
        JoinKeyword,
        LetKeyword,
        ManagedKeyword,
        NameofKeyword,
        NotKeyword,
        NotnullKeyword,
        OnKeyword,
        OrKeyword,
        OrderbyKeyword,
        PartialKeyword,
        RecordKeyword,
        RemoveKeyword,
        RequiredKeyword,
        SelectKeyword,
        SetKeyword,
        UnManegedKeyword,
        ValueKeyword,
        VarKeyword,
        WhenKeyword,
        WhereKeyword,
        WithKeyword,
        YieldKeyword,
        EndOfFileToken,
        EndOfLineToken,
        Identifier,
        CharLiteral,
        StringLiteral,
        UIntLiteral,
        IntLiteral,
        ULongLiteral,
        LongLiteral,
        SingleLiteral,
        DoubleLiteral,
        DecimalLiteral,
    }
    //TODO: Is variable-length possible?
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct TokenInfo {
        public static bool IsType(TokenType type) => (type is >=TokenType.BoolKeyword and <=TokenType.NuintKeyword);
        public static bool IsKeyword(TokenType type) => type is >= TokenType.FnKeyword and < TokenType.Identifier;
        public static bool IsBinary(TokenType type) => type is >= TokenType.PlusToken and < TokenType.EqualsToken;
        public static bool IsAssign(TokenType type) => type is >= TokenType.EqualsToken and <= TokenType.QuestionQuestionEqualsToken;
        public string GetName(SimpleList<string> names) {
            if (IsType(this.Type)) return ToString(Type);
            if (Type == TokenType.Identifier) return names[IntValue];
            return null;
        }
        public StringSegment GetName(StringSegmentList names) {
            if (IsType(this.Type)) return ToString(Type);
            if (Type == TokenType.Identifier) return names[IntValue];
            return null;
        }
        
        public static readonly string[] TokenStrings = {
            "Invalid",
            
            "!",
            "~",
            
            "+",
            "-",
            "/",
            "*",
            "%",
            "&",
            "|",
            "^",
            "<<",
            ">>",
            "??",
            
            "==",
            "!=",
            "<=",
            ">=",
            "<",
            ">",
            
            "&&",
            "||",
            
            "=",
            "+=",
            "-=",
            "/=",
            "*=",
            "%=",
            "&=",
            "|=",
            "^=",
            "<<=",
            ">>=",
            "??=",
            
            
            "}",
            "]",
            ")",
            "::",
            ":",
            
            ":=",
            
            ",",
            "$",
            "..",
            ".",
            "\"",
           
            "=>",
            "#",
            "$\"",
            "@$\"",
            "--",
            "->",
            "{",
            "[",
            "(",
            "++",
            
            "?",
            ";",
            "\'",
            "_",
            
            "fn", //Original Not CSharp
            
            "bool",
            "byte",
            "char",
            "decimal",
            "double",
            "float",
            "int",
            "long",
            "object",
            "sbyte",
            "short",
            "string",
            "uint",
            "ulong",
            "ushort",
            "void",
            "nint",
            "nuint",
            
            "false",
            "null",
            "true",
            "default",
            "break",
            "continue",
            
            
            
            
            
            "abstract",
            "as",
            "base",
            "case",
            "catch",
            "checked",
            "class",
            "const",
            "delegate",
            "do",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "finally",
            "fixed",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "interface",
            "internal",
            "is",
            "lock",
            "namespace",
            "new",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sealed",
            "sizeof",
            "stackalloc",
            "static",
            "struct",
            "switch",
            "this",
            "throw",
            "try",
            "typeof",
            "unchecked",
            "unsafe",
            "using",
            "virtual",
            "volatile",
            "while",
            "add",
            "and",
            "alias",
            "ascending",
            "args",
            "async",
            "await",
            "by",
            "descending",
            "dynamic",
            "equals",
            "from",
            "get",
            "global",
            "group",
            "init",
            "into",
            "join",
            "let",
            "managed",
            "nameof",
            "not",
            "notnull",
            "on",
            "or",
            "orderby",
            "partial",
            "record",
            "remove",
            "required",
            "select",
            "set",
            "unmanaged",
            "value",
            "var",
            "when",
            "where",
            "with",
            "yield",
            "EOF",
            
            
            //The following requires additional data for number
            "EOL",
            "identifier",
            "CharLiteral",
            "StringLiteral",
            "UIntLiteral",
            "IntLiteral",
            "ULongLiteral",
            "LongLiteral",
            "SingleLiteral",
            "DoubleLiteral",
            "DecimalLiteral",
            
        };
        [StructLayout(LayoutKind.Explicit)]
        private struct Union {
            [FieldOffset(0)]
            public char CharValue;
            [FieldOffset(0)]
            public int IntValue;
            [FieldOffset(0)]
            public uint UintValue;
            [FieldOffset(0)]
            public long LongValue;
            [FieldOffset(0)]
            public ulong UlongValue;
            [FieldOffset(0)]
            public float FloatValue;
            [FieldOffset(0)]
            public double DoubleValue;
            [FieldOffset(0)]
            public TokenInfo TokenInfo;
            [FieldOffset(8)]
            public TokenType Type;
        }
        [FieldOffset(0)]
        public  readonly char CharValue;
        [FieldOffset(0)]
        public  readonly int IntValue;
        [FieldOffset(0)]
        public  readonly uint UintValue;
        [FieldOffset(0)]
        public  readonly long LongValue;
        [FieldOffset(0)]
        public  readonly ulong UlongValue;
        [FieldOffset(0)]
        public  readonly float FloatValue;
        [FieldOffset(0)]
        public  readonly double DoubleValue;
        [FieldOffset(8)]
        public readonly TokenType Type;

        public static TokenInfo New(TokenType tokenType) {
            Union union = default;
            union.Type = tokenType;
            return union.TokenInfo;
        }
        public static TokenInfo New(char charValue) {
            Union union = default;
            union.Type = TokenType.CharLiteral;
            union.CharValue = charValue;
            return union.TokenInfo;
        }
        public static TokenInfo New(int intValue) {
            Union union = default;
            union.Type = TokenType.IntLiteral;
            union.IntValue = intValue;
            return union.TokenInfo;
        }public static TokenInfo New(TokenType tokenType,int intValue) {
            Union union = default;
            union.Type = tokenType;
            union.IntValue = intValue;
            return union.TokenInfo;
        }
        public static TokenInfo New(long longValue) {
            Union union = default;
            union.Type = TokenType.LongLiteral;
            union.LongValue = longValue;
            return union.TokenInfo;
        }
        public static TokenInfo New(uint uintValue) {
            Union union = default;
            union.Type = TokenType.UIntLiteral;
            union.UintValue = uintValue;
            return union.TokenInfo;
        }
        public static TokenInfo New(ulong ulongValue) {
            Union union = default;
            union.Type = TokenType.ULongLiteral;
            union.UlongValue = ulongValue;
            return union.TokenInfo;
        }
        public static TokenInfo New(double doubleValue) {
            Union union = default;
            union.Type = TokenType.DoubleLiteral;
            union.DoubleValue = doubleValue;
            return union.TokenInfo;
        }
         public static TokenInfo New(float floatValue) {
            Union union = default;
            union.Type = TokenType.SingleLiteral;
            union.FloatValue = floatValue;
            return union.TokenInfo;
        }
        public static TokenInfo New(decimal decimalValue) {
            Union union = default;
            union.Type = TokenType.DecimalLiteral;
            union.DoubleValue =(double) decimalValue;
            return union.TokenInfo;
        }

        public static string ToString(TokenType type) {
            return  TokenStrings[(int) type];
        }

        public override string ToString() {
            return ToString(Type);
        }
       
        
    }

          
}