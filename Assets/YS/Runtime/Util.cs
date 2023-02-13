using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using YS.Lexer;
using YS.Instructions;
using YS.Modules;
using YS.Text;
using YS.VM;

namespace YS {
    public static class Util {
        
       
        public static readonly Type[][] TypeArrays = {
            Array.Empty<Type>(),
            new Type[1],
            new Type[2],
            new Type[3],
            new Type[4],
            new Type[5],
            new Type[6],
            new Type[7],
        };
        public static Type[] Types() {
            return Array.Empty<Type>();
        }
        public static Type[] Types(Type arg1) {
            var typeArray = TypeArrays[1];
            typeArray[0] = arg1;
            return typeArray;
        }
        public static Type[] Types<T>() {
            var typeArray = TypeArrays[1];
            typeArray[0] = typeof(T);
            return typeArray;
        }
        public static Type[] Types<T1,T2>() {
            var typeArray = TypeArrays[2];
            typeArray[0] = typeof(T1);
            typeArray[1] = typeof(T2);
            return typeArray;
        }
        public static Type[] Types<T1,T2,T3>() {
            var typeArray = TypeArrays[3];
            typeArray[0] = typeof(T1);
            typeArray[1] = typeof(T2);
            typeArray[2] = typeof(T3);
            return typeArray;
        }
        
        
        public static Type[] Types(Type arg1,Type arg2) {
            var typeArray = TypeArrays[2];
            typeArray[0] = arg1;
            typeArray[1] = arg2;
            return typeArray;
        }
        public static Type[] Types(Type arg1,Type arg2,Type arg3) {
            var typeArray = TypeArrays[3];
            typeArray[0] = arg1;
            typeArray[1] = arg2;
            typeArray[2] = arg3;
            return typeArray;
        }
        public static Type[] Types(Type arg1,Type arg2,Type arg3,Type arg4) {
            var typeArray = TypeArrays[4];
            typeArray[0] = arg1;
            typeArray[1] = arg2;
            typeArray[2] = arg3;
            typeArray[3] = arg4;
            return typeArray;
        }
         public static Type[] Types(Type arg1,Type arg2,Type arg3,Type arg4,Type arg5) {
            var typeArray = TypeArrays[5];
            typeArray[0] = arg1;
            typeArray[1] = arg2;
            typeArray[2] = arg3;
            typeArray[3] = arg4;
            typeArray[4] = arg5;
            return typeArray;
        }
        public static Type[] Types(Type arg1,Type arg2,Type arg3,Type arg4,Type arg5,Type arg6) {
            var typeArray = TypeArrays[5];
            typeArray[0] = arg1;
            typeArray[1] = arg2;
            typeArray[2] = arg3;
            typeArray[3] = arg4;
            typeArray[4] = arg5;
            typeArray[5] = arg6;
            return typeArray;
        }
        
        
        public static bool TryGetVariable(this TokenType tokenType,out Variable variable) {
            if (TokenInfo.IsType(tokenType)) {
                switch (tokenType) {
                    case TokenType.IntKeyword:
                        variable = new Variable<int>();
                        return true;
                    case TokenType.FloatKeyword:
                        variable = new Variable<float>();
                        return true;
                    case TokenType.DoubleKeyword:
                        variable = new Variable<double>();
                        return true;
                    case TokenType.ObjectKeyword:
                        variable = new Variable<object>();
                        return true;
                    case TokenType.StringKeyword:
                        variable = new Variable<string>();
                        return true;
                    case TokenType.DecimalKeyword:
                        variable = new Variable<decimal>();
                        return true;
                    
                }
            }
            variable = null;
            return false;
        }public static bool TryGetType(this TokenType tokenType,out Type type) {
            if (TokenInfo.IsType(tokenType)) {
                switch (tokenType) {
                    case TokenType.IntKeyword:
                        type = typeof(int);
                        return true;
                    case TokenType.FloatKeyword:
                        type = typeof(float);
                        return true;
                    case TokenType.DoubleKeyword:
                        type = typeof(double);
                        return true;
                    case TokenType.ObjectKeyword:
                        type = typeof(object);
                        return true;
                    case TokenType.StringKeyword:
                        type = typeof(string);
                        return true;
                    case TokenType.DecimalKeyword:
                        type = typeof(decimal);
                        return true;
                    
                }
            }
            type = null;
            return false;
        }
        public static string ToUnaryOperatorName(this TokenType type) {
            switch (type) {
                case TokenType.PlusToken: return "op_UnaryPlus";
                case TokenType.MinusToken: return "op_UnaryNegation";
                case TokenType.ExclamationToken: return "op_LogicalNot";
                case TokenType.PlusPlusToken: return "op_Increment";
                case TokenType.MinusMinusToken: return "op_Decrement";
            }
            return null;
        }
        
        public static TokenType ToBinaryOperator(this TokenType type) {
            switch (type) {
                case TokenType.PlusEqualsToken: return TokenType.PlusToken;
                case TokenType.MinusEqualsToken: return TokenType.MinusToken;
                case TokenType.AsteriskEqualsToken: return TokenType.AsteriskToken;
                case TokenType.SlashEqualsToken: return TokenType.SlashToken;
                case TokenType.AmpersandEqualsToken: return TokenType.AmpersandToken;
                case TokenType.BarEqualsToken: return TokenType.BarToken;
                case TokenType.PercentEqualsToken: return TokenType.PercentToken;
            }
            return 0;
        }
        public static BinaryOpType ToBinaryOpType(this TokenType type) {
            switch (type) {
                case TokenType.PlusToken: return BinaryOpType.Addition;
                case TokenType.MinusToken: return BinaryOpType.Subtraction;
                case TokenType.AsteriskToken: return BinaryOpType.Multiply;
                case TokenType.SlashToken: return BinaryOpType.Division;
                case TokenType.AmpersandToken: return BinaryOpType.BitwiseAnd;
                case TokenType.BarToken: return BinaryOpType.BitwiseOr;
                case TokenType.PercentToken: return BinaryOpType.Modulus;
                case TokenType.EqualsEqualsToken: return BinaryOpType.Equal;
                case TokenType.ExclamationEqualsToken: return BinaryOpType.Inequal;
                case TokenType.LessThanToken: return BinaryOpType.LessThan;
                case TokenType.GreaterThanToken: return BinaryOpType.GreaterThan;
                case TokenType.LessThanEqualsToken: return BinaryOpType.LessThanOrEqual;
                case TokenType.GreaterThanEqualsToken: return BinaryOpType.GreaterThanOrEqual;
                case TokenType.LessThanLessThanToken: return BinaryOpType.LeftShift;
                case TokenType.GreaterThanGreaterThanToken: return BinaryOpType.RightShift;
                case TokenType.CaretToken: return BinaryOpType.ExclusiveOr;
            }
            return (BinaryOpType)99;
        }
        
        public static (int, string)[] BinaryOperatorHashAndName = {
            ("op_Addition".GetExHashCode(),"op_Addition"),
            ("op_Subtraction".GetExHashCode(),"op_Subtraction"),
            ("op_Multiply".GetExHashCode(),"op_Multiply"),
            ("op_Division".GetExHashCode(),"op_Division"),
            ("op_Modulus".GetExHashCode(),"op_Modulus"),
            ("op_Equal".GetExHashCode(),"op_Equal"),
            ("op_Inequal".GetExHashCode(),"op_Inequal"),
            ("op_LessThan".GetExHashCode(),"op_LessThan"),
            ("op_GreaterThan".GetExHashCode(),"op_GreaterThan"),
            ("op_LessThanOrEqual".GetExHashCode(),"op_LessThanOrEqual"),
            ("op_GreaterThanOrEqual".GetExHashCode(),"op_GreaterThanOrEqual"),
            ("op_BitwiseAnd".GetExHashCode(),"op_BitwiseAnd"),
            ("op_BitwiseOr".GetExHashCode(),"op_BitwiseOr"),
            ("op_ExclusiveOr".GetExHashCode(),"op_ExclusiveOr"),
            ("op_LeftShift".GetExHashCode(),"op_LeftShift"),
            ("op_RightShift".GetExHashCode(),"op_RightShift"),
        };
       
      
    }
}