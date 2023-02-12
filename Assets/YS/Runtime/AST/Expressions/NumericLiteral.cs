using System;
using YS.Lexer;
using System.Text;
using YS.VM;

namespace YS.AST.Expressions {
    public class NumericLiteral: IExpression {

        public TokenInfo Info { get; set; }
        private int  sign;
        

        public NumericLiteral(TokenInfo info,bool isMinus=false) {
            Info = info;
            sign = isMinus?-1:1;
        }
        public void ToCode(StringBuilder builder) {
            switch (Info.Type) {
                case TokenType.CharLiteral:
                    builder.Append('\'');
                    builder.Append(Info.CharValue);
                    builder.Append('\'');
                    break;
                case TokenType.IntLiteral:
                    builder.Append(sign*Info.IntValue);
                    break;
                case TokenType.UIntLiteral:
                    builder.Append(Info.UintValue);
                    builder.Append('u');
                    break;
                case TokenType.LongLiteral:
                    builder.Append(sign*Info.LongValue);
                    builder.Append('L');
                    break;
                case TokenType.ULongLiteral:
                    builder.Append(Info.UintValue);
                    builder.Append('u');
                    builder.Append('L');
                    break;
                case TokenType.SingleLiteral:
                    builder.Append(sign*Info.FloatValue);
                    builder.Append('f');
                    break;
                case TokenType.DoubleLiteral:
                    builder.Append(sign*Info.DoubleValue);
                    builder.Append('d');
                    break;
                case TokenType.DecimalLiteral:
                    builder.Append(sign*Info.DoubleValue);
                    builder.Append('m');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            var variable = target.variable;
            var id = target.id;
            switch (Info.Type) {
                case TokenType.CharLiteral:
                    if (variable is Variable<char>) {
                        context.EmitReadChar(id,Info.CharValue);
                    }
                    break;
                case TokenType.IntLiteral:
                    if (variable is Variable<int>) {
                        context.EmitReadInt(id,sign*Info.IntValue);
                    }else if (variable is Variable<float>) {
                        context.EmitReadFloat(id,sign* Info.IntValue);
                    }else if (variable is Variable<double>) {
                        context.EmitReadDouble(id,sign* Info.IntValue);
                    }
                    break;
                case TokenType.UIntLiteral:
                    break;
                case TokenType.LongLiteral:
                    break;
                case TokenType.ULongLiteral:
                    break;
                case TokenType.SingleLiteral:
                    if (variable is Variable<float>) {
                        context.EmitReadFloat(id,sign*Info.FloatValue);
                    }
                    break;
                case TokenType.DoubleLiteral:
                    if (variable is Variable<double>) {
                        context.EmitReadDouble(id,sign*Info.DoubleValue);
                    }
                    break;
                case TokenType.DecimalLiteral:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Variable GetNumber() {
            if (Info.DoubleValue == 0) {
                switch (Info.Type) {
                    case TokenType.IntLiteral:
                        return Constants.IntZero;
                    case TokenType.SingleLiteral:
                        return Constants.FloatZero;
                    case TokenType.DoubleLiteral:
                        return Constants.DoubleZero;
                }
            }
            return Info.Type switch {
                TokenType.CharLiteral => new Variable<char>(Info.CharValue),
                TokenType.IntLiteral => new Variable<int>(sign*Info.IntValue),
                TokenType.UIntLiteral => new Variable<uint>(Info.UintValue),
                TokenType.LongLiteral => new Variable<long>(sign*Info.LongValue),
                TokenType.ULongLiteral => new Variable<ulong>(Info.UintValue),
                TokenType.SingleLiteral => new Variable<float>(sign*Info.FloatValue),
                TokenType.DoubleLiteral => new Variable<double>(sign*Info.DoubleValue),
                TokenType.DecimalLiteral => new Variable<decimal>(sign*(decimal)Info.DoubleValue),
                _ => null
            };
        }
        public Variable GetVariable() {
            return Info.Type switch {
                TokenType.CharLiteral => new Variable<char>(),
                TokenType.IntLiteral => new Variable<int>(),
                TokenType.UIntLiteral => new Variable<uint>(),
                TokenType.LongLiteral => new Variable<long>(),
                TokenType.ULongLiteral => new Variable<ulong>(),
                TokenType.SingleLiteral => new Variable<float>(),
                TokenType.DoubleLiteral => new Variable<double>(),
                TokenType.DecimalLiteral => new Variable<decimal>(),
                _ => null
            };
        }
        
        public ushort   AddVariable(CompilingContext context) {
            ushort id;
            switch (Info.Type) {
                case TokenType.CharLiteral:
                    id = context.AddVariable<char>().id;
                    context.EmitReadChar(id,Info.CharValue);
                    return id;
                case TokenType.IntLiteral:
                    id = context.AddVariable<int>().id;
                    context.EmitReadInt(id,sign*Info.IntValue);
                    return id;
               
                case TokenType.SingleLiteral:
                    id = context.AddVariable<float>().id;
                    context.EmitReadFloat(id,sign*Info.FloatValue);
                    return id;
                case TokenType.DoubleLiteral:
                    id = context.AddVariable<double>().id;
                    context.EmitReadDouble(id,sign*Info.DoubleValue);
                    return id;
               
                default:
                    throw new NotSupportedException();
            }
        }

        public Type GetNumberType() {
            return Info.Type switch {
                TokenType.CharLiteral =>typeof(char),
                TokenType.IntLiteral =>typeof(int),
                TokenType.UIntLiteral => typeof(uint),
                TokenType.LongLiteral => typeof(long),
                TokenType.ULongLiteral =>typeof(ulong),
                TokenType.SingleLiteral => typeof(float),
                TokenType.DoubleLiteral => typeof(double),
                TokenType.DecimalLiteral => typeof(decimal),
                _ => null
            };
        }

       
        public (ushort id,object obj) Compile( CompilingContext context) {
            var number = GetNumber();
           return (context.AddVariable(number),number);
        }
        
    }
}