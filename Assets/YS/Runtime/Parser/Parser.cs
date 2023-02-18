using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using YS.AST;
using YS.AST.Expressions;
using YS.AST.Statements;
using YS.Collections;
using YS.Lexer;
using YS.Modules;

using YS.Text;
namespace YS.Parser {
    public unsafe struct Parser {
        TokenInfo CurrentTokenInfo => tokenBuffer[0];
        TokenType CurrentTokenType => tokenBuffer->Type;
        int CurrentIntValue => tokenBuffer->IntValue;
        TokenInfo NextTokenInfo=> tokenBuffer[1];
        TokenType NextTokenType => (tokenBuffer + 1)->Type;

        int CurrentLine {
            get {
                var ptr = tokenBuffer;
                while (ptr->Type is not TokenType.EndOfLineToken or TokenType.Invalid) {
                    ptr++;
                    
                }
                return ptr->IntValue;
            }
        }

        TokenInfo* tokenBuffer;
        TokenInfo* last;
        readonly StringSegmentList Names;
        readonly SimpleList<ushort> CurrentLines;

        Precedence CurrentPrecedence => GetPrecedence(CurrentTokenType);

        Precedence NextPrecedence => GetPrecedence(NextTokenType);

        void AddLine() => CurrentLines.Add((ushort)CurrentLine);

        Parser(TokenInfo* tokens,int length, StringSegmentList names,SimpleList<ushort> currentLines) {
            tokenBuffer = tokens;
            last = tokenBuffer + length-1;
            Names = names;
            currentLines.Clear();
            CurrentLines = currentLines;
        }

        void Advance() {
            if (IsEof) {
                throw new Exception("end");
            }
            ++tokenBuffer;
            AdvanceIfEOL();
        }

        void AdvanceIfEOL() {

            while (CurrentTokenType == TokenType.EndOfLineToken && !IsEof) {
                ++tokenBuffer;
            }
        }
        public  static  void ParseProgram(SimpleList<TokenInfo> tokens,StringSegmentList names,Root root) {
            fixed (TokenInfo* t = tokens.GetArray()) {
                var parser = new Parser(t,tokens.Count, names,root.StatementToLine);
                try {
                    parser.ParseProgramInternal(root);
                }
                catch (Exception) {
                    var builder = new StringBuilder();
                    foreach (var token in tokens.AsSpan()) {
                        builder.Append(TokenInfo.ToString(token.Type));
                        builder.Append(' ');
                    }
                    Debug.Log(builder.ToString());
                    Debug.LogError("line : " + parser.CurrentLine);
                    throw;
                }

            }
        }

        bool IsEof => last<=tokenBuffer;
         void ParseProgramInternal(Root root) {
            root.Statements.Clear();
            AdvanceIfEOL();
            while (!IsEof) {
                var statement = ParseStatement();
                if (statement != null) {
                    root.Statements.Add(statement);
                }
            }
        }

        public IStatement ParseStatement() {
            AddLine();
            
            switch (CurrentTokenType) {
                case TokenType.EndOfLineToken: {
                    AdvanceIfEOL();
                    return null;
                }
                case TokenType.VarKeyword:
                    return ParseVarStatement();
                case TokenType.ConstKeyword:
                    return ParseConstStatement();
                case TokenType.ReturnKeyword:
                    return ParseReturnStatement();
                case TokenType.ContinueKeyword:
                    Advance();
                    return ContinueStatement.Instance;
                case TokenType.BreakKeyword:
                    Advance();
                    return BreakStatement.Instance;
                case TokenType.ForKeyword:
                    return ParseForEachStatement();
                case TokenType.UsingKeyword:
                    return  ParseUsingStatement();
                case TokenType.AsyncKeyword:
                    return ParseAsyncStatement();
                case TokenType.IfKeyword:
                    return ParseIfStatement();
                case TokenType.WhileKeyword:
                    return ParseWhileStatement();
                
                case TokenType.SemicolonToken:
                    Advance();
                    return null;
                case TokenType.Identifier: {
                    return NextTokenType switch {
                        TokenType.ColonEqualsToken => ParseDefineStatement(),
                        TokenType.ColonToken => ParseDefineStatementWithType(),
                        _ => ParseExpressionStatement()
                    };
                }
                default:
                    return ParseExpressionStatement();
            }
        }


        public IExpression ParseExpression(Precedence precedence) {
            AdvanceIfEOL();
            var leftExpression = GetPrefix();
            while (!IsEndOfExpression(NextTokenType) && precedence < NextPrecedence) {
                Advance();
                leftExpression = GetInfixOrPostfix(CurrentTokenType, leftExpression);
            }

            while (NextTokenType == TokenType.EndOfLineToken) {
                ++tokenBuffer;
            }

            return leftExpression;
        }



         static bool IsEndOfExpression(TokenType tokenType) {
            return tokenType is TokenType.CloseParenToken or TokenType.SemicolonToken;
        }

        public IExpression GetPrefix() {
            var tokenType = CurrentTokenType;
            if (TokenInfo.IsType(tokenType)) {

                return new KeywordIdentifier(tokenType);
            }
            switch (tokenType) {
                case TokenType.Identifier: {
                    return new IdentifierLiteral(Names[CurrentIntValue]);
                }
                case TokenType.TypeOfKeyword: {
                    Advance();
                    Advance();
                    var type = ParseType();
                    return new TypeOfExpression(type);
                }
                case TokenType.CharLiteral:
                case TokenType.IntLiteral:
                case TokenType.UIntLiteral:
                case TokenType.LongLiteral:
                case TokenType.ULongLiteral:
                case TokenType.SingleLiteral:
                case TokenType.DoubleLiteral:
                case TokenType.DecimalLiteral: return new NumericLiteral(CurrentTokenInfo);
                case TokenType.StringLiteral: return  new StringLiteral(Names[CurrentIntValue].ToString());
                case TokenType.MinusToken: return ParseMinusExpression();
                case TokenType.PlusToken:
                case TokenType.ExclamationToken: 
                    return ParseUnaryExpression(tokenType);
                case TokenType.PlusPlusToken: 
                case TokenType.MinusMinusToken: 
                    return ParsePreIncrementDecrementExpression(tokenType);
                case TokenType.TrueKeyword:return BooleanLiteral.True;
                case TokenType.FalseKeyword:return BooleanLiteral.False;
                case TokenType.NullKeyword:return NullExpression.Instance;
                case TokenType.DefaultKeyword:return DefaultExpression.Instance;
                case TokenType.OpenParenToken: return ParseGroupedExpression();
                case TokenType.AwaitKeyword: return ParseAwaitExpression();
                case TokenType.DotDotToken: return ParseIntRangeExpression();
                case TokenType.NewKeyword: {
                    Advance();
                    return new NewExpression(ParseType(), ParseCallArguments());
                }
                default:
                    if (TokenInfo.IsKeyword(tokenType)) {
                        return new KeywordIdentifier(tokenType);
                    }
                    throw new ParseException(tokenType.ToString() + " is not predicted as prefix.");
            }
        }
        IExpression GetInfixOrPostfix(TokenType type, IExpression expression) {
            if (type is TokenType.PlusPlusToken) {
                if (expression is IdentifierLiteral identifierLiteral) {
                    Advance();
                    return new PostIncrementDecrementExpression(true, identifierLiteral.Name);
                }
                throw new Exception();
            }
            if (type is TokenType.MinusMinusToken) {
                if (expression is IdentifierLiteral identifierLiteral) {
                    Advance();
                    return new PostIncrementDecrementExpression(false, identifierLiteral.Name);
                }
                throw new Exception();
            }
            
            if (type == TokenType.DotDotToken) {
                return ParseIntRangeExpression(expression);
            }
            if (TokenInfo.IsBinary(type)) {
                return ParseBinaryExpression(expression,type);
            }
            if (TokenInfo.IsAssign(type)) {
                return ParseAssignExpression(expression,type);
            }
            switch (type) {
                case TokenType.OpenBracketToken: return ParseIndexerExpression(expression);
                case TokenType.OpenParenToken: return ParseCallExpression(expression);
                case TokenType.DotToken: return ParseMemberExpression(expression);
                case TokenType.QuestionToken: return ParseConditionalExpression(expression);
                default: {
                    throw new ParseException(type.ToString() + " is not predicted as infix.");
                }
            }
        }

        IExpression ParseIntRangeExpression() {
            Advance();
            var end = ParseExpression(Precedence.Range);
            return new IntRangeExpression(end);
        }
        IExpression ParseIntRangeExpression(IExpression start) {
            Advance();
            var end = ParseExpression(Precedence.Range);
            return new IntRangeExpression(start,end);
        }


        public IExpression ParseConditionalExpression(IExpression condition) {
            var expression = new ConditionalExpression {
                Condition = condition
            };
            Advance();
            expression.Consequent = ParseExpression(Precedence.Lowest);
            Advance();
            Advance();
            expression.Alternative = ParseExpression(Precedence.Lowest);
            return expression;
        }

        public IExpression ParseUnaryExpression(TokenType type) {
            Advance();
            var expression = new UnaryExpression {
                Type = type,
                Right = ParseExpression(Precedence.Unary)
            };

            return expression;
        }

        IExpression ParseMinusExpression() {
            switch (NextTokenType) {
                case TokenType.IntLiteral:
                case TokenType.UIntLiteral:
                case TokenType.LongLiteral:
                case TokenType.ULongLiteral:
                case TokenType.SingleLiteral:
                case TokenType.DoubleLiteral: {
                    Advance();
                    return new NumericLiteral(CurrentTokenInfo, true);
                }
            }

            return ParseUnaryExpression(TokenType.MinusToken);
        }
        public IExpression ParsePreIncrementDecrementExpression(TokenType type) {
          
            Advance();
            var i= (IdentifierLiteral)ParseExpression(Precedence.Unary);
            var expression = new PreIncrementDecrementExpression(type==TokenType.PlusPlusToken,i.Name);
            return expression;
        }

        public IExpression ParseBinaryExpression(IExpression left,TokenType type) {
            var expression = new BinaryExpression() {
                Type = type,
                Left = left,
            };

            var precedence = CurrentPrecedence;
            Advance();
            expression.Right = ParseExpression(precedence);

            return expression;
        }

        public IExpression ParseAwaitExpression() {
            Advance();
            var expression = new AwaitExpression() {
                Right = ParseExpression(Precedence.Lowest)
            };

            return expression;
        }



        public IExpression ParseGroupedExpression() {
            Advance();
            var expression = ParseExpression(Precedence.Lowest);

            if (!ExpectPeek(TokenType.CloseParenToken)) return null;
            
            if (CurrentTokenType==TokenType.Identifier&&expression is MemberExpression or IIdentifier) {
                return new CastExpression(expression, ParseExpression(Precedence.Unary));
            }

            return expression;
        }

        public IStatement ParseForEachStatement() {
            var statement = new ForEachStatement();
            Advance();
            var hasOpenParen=false;
            if (CurrentTokenType == TokenType.OpenParenToken) {
                hasOpenParen = true;
                Advance();
            }
            statement.Current = Names[CurrentIntValue];
            ExpectPeek(TokenType.InKeyword);
            Advance();
            statement.Enumerable = ParseExpression(Precedence.Lowest);
            if(hasOpenParen){
                Advance();
            }
            statement.Consequence = ParseBlockStatement();
            return statement;
        }

        public IStatement ParseIfStatement() {
            var statement = new IfStatement();

            Advance();
            var hasOpenParen=false;
            if (CurrentTokenType == TokenType.OpenParenToken) {
                hasOpenParen = true;
                Advance();
            }
            if (CurrentTokenType == TokenType.NotKeyword) {
                statement.IsNot = true;
                Advance();
            }
            statement.Condition = ParseExpression(Precedence.Lowest);
            if(hasOpenParen){
                Advance();
            }
            
            statement.IfBlock = ParseBlockStatement();
           
            
            if (NextTokenType == TokenType.EndOfLineToken) Advance();

            if (CurrentTokenType==TokenType.ElseKeyword) {
                if (NextTokenType == TokenType.EndOfLineToken) {
                    Advance();
                }
                statement.ElseBlock = ParseBlockStatement();
            }

            return statement;
        }

        public IStatement ParseWhileStatement() {
            var statement = new WhileStatement();

            Advance();
            var hasOpenParen=false;
            if (CurrentTokenType == TokenType.OpenParenToken) {
                hasOpenParen = true;
                Advance();
            }
            if (CurrentTokenType == TokenType.NotKeyword) {
                statement.IsNot = true;
                Advance();
            }
            statement.Condition = ParseExpression(Precedence.Lowest);
            if(hasOpenParen){
                Advance();
            }
            
            statement.Block = ParseBlockStatement();
           
            return statement;
        }



        public IStatement ParseAsyncStatement() {
            var statement = new AsyncStatement {
                Consequence = ParseBlockStatement()
            };


            return statement;
        }


        public IStatement ParseFunctionStatement(object type) {
            var fn = new FunctionStatement {
                ReturnType = type,
                Name = CurrentTokenInfo.GetName(Names).ToString()
            };

            if (!ExpectPeek(TokenType.OpenParenToken)) return null;

            fn.Parameters = ParseParameters();

            fn.Body = ParseBlockStatement();

            return fn;
        }


        public IndexerExpression ParseIndexerExpression(IExpression array) {
            var expression = new IndexerExpression() {
                Expression = array,
                Arguments = ParseIndexArguments()
            };
            return expression;
        }

        public CallExpression ParseCallExpression(IExpression fn) {
            var expression = new CallExpression() {
                Function = fn,
                Arguments = ParseCallArguments(),
            };
            return expression;
        }

        public IExpression ParseMemberExpression(IExpression parent) {
            
            var expression = new MemberExpression() {
                Parent = parent
            };
            Advance();
            expression.Member = Names[CurrentIntValue];
            return expression;
        }



        public AssignExpression ParseAssignExpression(IExpression left,TokenType tokenType) {
            var expression = new AssignExpression() {
                Type = tokenType,
                Left = left,
            };
            Advance();
            expression.Right = ParseExpression(Precedence.Lowest);
            return expression;
        }

        public IExpression ParseCallArguments() {

            Advance();
            if (CurrentTokenType == TokenType.CloseParenToken) return null;
            var firstArg = ParseParameter();
            ListedExpression args = null;
            if (firstArg.Item1!=0||NextTokenType == TokenType.CommaToken) {
                args = new ListedExpression {firstArg};
                while (NextTokenType == TokenType.CommaToken) {
                    Advance();
                    Advance();
                    args.Add(ParseParameter());
                }
            }

            if (!ExpectPeek(TokenType.CloseParenToken)) return null;
            return args ?? firstArg.Item2;
        }

        (PassType, IExpression) ParseParameter() {
            var currentType = CurrentTokenType;
            switch (currentType) {
                case TokenType.RefKeyword or TokenType.InKeyword: {
                    Advance();
                    var expr=ParseExpression(Precedence.Lowest);
                    if (expr is not IdentifierLiteral) throw new Exception("'ref' or 'in' param arguments must be readable variable ");
                    return (currentType == TokenType.RefKeyword ? PassType.Ref : PassType.In,expr);
                }
                case TokenType.OutKeyword: {
                    Advance();
                    if (CurrentTokenType == TokenType.VarKeyword) {
                        Advance();
                        var currentInfo = CurrentTokenInfo;
                        if(currentInfo.Type!=TokenType.Identifier)throw new Exception("out var -> next must be name ");
                        var param = (PassType.Out,new OutVarNameExpression(Names[currentInfo.IntValue].ToString()) );
                        return param;
                    }
                    {
                        var expr=ParseExpression(Precedence.Lowest);
                        if (expr is not IdentifierLiteral) throw new Exception("'out'  param arguments must be new or readable variable ");
                        return (PassType.Out,expr);
                    }
                }
                default:
                    return (0, ParseExpression(Precedence.Lowest));
            }
        }
        public IExpression ParseIndexArguments() {

            Advance();
            if (CurrentTokenType == TokenType.CloseBracketToken) return null;
            var firstArg = ParseParameter();
            ListedExpression args = null;
            if (firstArg.Item1!=0||NextTokenType == TokenType.CommaToken) {
                args = new ListedExpression {firstArg};
                while (NextTokenType == TokenType.CommaToken) {
                    Advance();
                    Advance();
                    args.Add(ParseParameter());
                }
            }
            if (!ExpectPeek(TokenType.CloseBracketToken)) return null;
            return args ?? firstArg.Item2;
        }

        TypeNamePair GetTypeNamePair() {
            object type;
            if (TokenInfo.IsType(CurrentTokenType)) {
                type = TokenInfo.ToString(CurrentTokenType);
            }
            else {
                type = ParseExpression(Precedence.Lowest);
            }

            Advance();
            var name = CurrentTokenInfo.GetName(Names);


            return new TypeNamePair(type, name);
        }

        public List<TypeNamePair> ParseParameters() {
            if (NextTokenType == TokenType.CloseParenToken) {
                Advance();
                return null;
            }

            var parameters = new List<TypeNamePair>();
            Advance();
            parameters.Add(GetTypeNamePair());
            while (NextTokenType == TokenType.CommaToken) {
                Advance();
                Advance();
                parameters.Add(GetTypeNamePair());
            }

            ExpectPeek(TokenType.CloseParenToken);

            return parameters;
        }

        public UsingStatement ParseUsingStatement() {
            var statement = new UsingStatement();
           Advance();
           AssumeCurrentToken(TokenType.Identifier);
           var start = Names[CurrentIntValue];
           var end = start;
           while (NextTokenType==TokenType.DotToken) {
               Advance(); Advance();
               AssumeCurrentToken(TokenType.Identifier);
               end = Names[CurrentIntValue];
           }

           if (start.Range.Start.Value == end.Range.Start.Value) 
                statement.NameSpace = start.ToString();
           else statement.NameSpace = start.Source[new Range(start.Range.Start,end.Range.End)];
           Advance();
            return statement;
        }
        public BlockStatement ParseBlockStatement() {
            if (NextTokenType == TokenType.EndOfLineToken) {
                Advance();
                AssumeCurrentToken(TokenType.OpenBraceToken);
            }
            else {
                ExpectPeek(TokenType.OpenBraceToken);
            }


            var block = new BlockStatement() {
                Statements = new List<IStatement>(),
            };
            Advance();
            while (CurrentTokenType != TokenType.CloseBraceToken) {
                var statement = ParseStatement();
                if (statement != null) {
                    block.Statements.Add(statement);
                }
                if (IsEof)  return block;
            }
            Advance();
            return block;
        }

        public VarStatement ParseVarStatement() {
            var statement = new VarStatement();
            Advance();
            // if (!ExpectPeek(TokenType.Identifier)) return null;
            statement.Variable = CurrentTokenInfo.GetName(Names);
            var nextType = NextTokenType;
            if (nextType == TokenType.EndOfLineToken) throw new Exception();
            Advance();
            if(nextType!=TokenType.EqualsToken) {
                statement.Type =  ParseType();
                if(CurrentTokenType!=TokenType.EqualsToken)return statement;
            }
            Advance();
            statement.Value = ParseExpression(Precedence.Lowest);
            Advance();
            return statement;
        }
        public ConstStatement ParseConstStatement() {
            var statement = new ConstStatement();
            Advance();
            // if (!ExpectPeek(TokenType.Identifier)) return null;
            statement.Variable = CurrentTokenInfo.GetName(Names);
            if (!ExpectPeek(TokenType.EqualsToken)) return null;
            Advance();
            statement.Value = ParseExpression(Precedence.Lowest);
            Advance();
            return statement;
        }

        
    public VarStatement ParseDefineStatement() {
        var statement = new VarStatement {
            Variable = CurrentTokenInfo.GetName(Names)
        };
        Advance();  Advance();
        statement.Value = ParseExpression(Precedence.Lowest);
        return statement;
    }
    public VarStatement ParseDefineStatementWithType() {
        var statement = new VarStatement();
        statement.Variable = CurrentTokenInfo.GetName(Names);
        Advance();Advance();
        statement.Type= ParseType();
        if(CurrentTokenType!=TokenType.EqualsToken)return statement;
        ExpectPeek(TokenType.EqualsToken);
        Advance();
        switch (CurrentTokenType) {
            case TokenType.NewKeyword: 
                Advance();
                statement.Value = CurrentTokenType == TokenType.OpenParenToken 
                    ? new NewExpression(null, ParseCallArguments()) 
                    : new NewExpression(ParseType(), ParseCallArguments());
                break;
            case TokenType.DefaultKeyword:statement.Value =DefaultExpression.Instance;
                break;
            case TokenType.NullKeyword:statement.Value =NullExpression.Instance;
                break;
            default:
                Debug.Log(CurrentTokenInfo.ToString());
                statement.Value = ParseExpression(Precedence.Lowest);
                break;
        }
        return statement;
    }

    IType ParseType() {
        var current = CurrentTokenType;
        if (current is >= TokenType.BoolKeyword and <= TokenType.NuintKeyword) {
            Advance();
            return new KeyWordType(current);
        }
        if (current != TokenType.Identifier) return null;
        var name = Names[CurrentIntValue];
        if (NextTokenType == TokenType.DotToken) {
            var list = new List<StringSegment> {name};
            Advance();Advance();
            list.Add(Names[CurrentIntValue]);
            while (NextTokenType == TokenType.DotToken) {
                Advance();Advance();
                list.Add(Names[CurrentIntValue]); 
            }
            Advance();
            return new LiteralTypeWithDot(list);
        }
        Advance();
        
        return new LiteralType(name);
        
    }


        public ReturnStatement ParseReturnStatement() {
            var statement = new ReturnStatement();
            if (IsEndOfExpression(NextTokenType)) {
                Advance();
                return statement;
            }

            Advance();


            if (CurrentTokenType != TokenType.SemicolonToken) {
                statement.ReturnValue = ParseExpression(Precedence.Lowest);
            }
            Advance();
            return statement;
        }

        public IStatement ParseExpressionStatement() {
            
            var expression = ParseExpression(Precedence.Lowest);
            
            if (expression is IIdentifier) {
                if (NextTokenType == TokenType.Identifier) {
                    Advance();
                    var next = NextTokenType;
                    if (next == TokenType.OpenParenToken) return ParseFunctionStatement(expression);
                }
            }
            
            var statement = new ExpressionStatement {
                Expression = expression
            };
            Advance();
            return statement;
        }
        bool ExpectPeek(TokenType type) {
            while (NextTokenType == TokenType.EndOfLineToken) {
                ++tokenBuffer;
            }

            if (NextTokenType == type) {
                Advance();
                return true;
            }

            throw ParseException.Expect(CurrentLine,type,NextTokenType);
        }

        bool AssumeCurrentToken(TokenType type) {
            if (CurrentTokenType == type) {
                return true;
            }
            throw ParseException.Expect(CurrentLine,type,CurrentTokenType);
        }
        public static Precedence GetPrecedence(TokenType type) {
            switch (type) {
                case TokenType.EqualsEqualsToken: return Precedence.Equals;
                case TokenType.AmpersandToken: return Precedence.And;
                case TokenType.BarToken: return Precedence.Or;
                case TokenType.AmpersandAmpersandToken: return Precedence.AndAnd;
                case TokenType.BarBarToken: return Precedence.OrOr;
                case TokenType.LessThanEqualsToken:
                case TokenType.GreaterThanEqualsToken:
                case TokenType.LessThanToken:
                case TokenType.GreaterThanToken:return Precedence.LessOrGreater;
                case TokenType.PlusToken:
                case TokenType.MinusToken:return Precedence.SUM;
                case TokenType.PlusPlusToken:
                case TokenType.MinusMinusToken:return Precedence.Unary;
                case TokenType.AsteriskToken:
                case TokenType.SlashToken:
                case TokenType.PercentToken:return Precedence.PRODUCT;
                case TokenType.DotToken: 
                case TokenType.OpenBracketToken:
                case TokenType.OpenParenToken:return Precedence.Primary;
                case TokenType.EqualsToken:
                case TokenType.PlusEqualsToken:
                case TokenType.MinusEqualsToken:
                case TokenType.PercentEqualsToken:
                case TokenType.AsteriskEqualsToken:
                case TokenType.SlashEqualsToken:
                case TokenType.AmpersandEqualsToken:
                case TokenType.BarEqualsToken:return Precedence.Assign;
                case TokenType.DotDotToken: return Precedence.Range;
                case TokenType.QuestionToken: return Precedence.Conditional;
                default: {
                    return Precedence.Lowest;
                }
            }
        }
       
        
    }
    public enum Precedence
    {
        Lowest = 0,
        Assign ,
        Conditional, //?:
        NullCoalescing,//??
        OrOr,
        AndAnd,
        Or,
        Xor,
        And,
        Equals,      // ==
       
        LessOrGreater, // >, <
        SUM,         // +
        PRODUCT,     // *
        Range,  // ..
        Unary,      // -x, !x
        Primary       // myFunction(x)
    }
}
