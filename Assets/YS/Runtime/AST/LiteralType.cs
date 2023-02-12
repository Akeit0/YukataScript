

using System;
using System.Collections.Generic;
using UnityEngine;
using YS.AST;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.VM;
using YS.Text;
namespace YS.AST {
   
    public interface IType {
        public Type GetType(CompilingContext context);
        public (ushort id ,Variable variable) AddVariable(CompilingContext context) => context.AddVariable(GetType(context));
    }

    public class KeyWordType : IType {
        public KeyWordType(TokenType tokenType) {
            TokenType = tokenType;
        }

        public TokenType TokenType { get; }

        public Type GetType(CompilingContext context)
            => TokenType switch {
                TokenType.CharKeyword => typeof(char),
                TokenType.IntKeyword => typeof(int),
                TokenType.UIntKeyword => typeof(uint),
                TokenType.LongKeyword => typeof(long),
                TokenType.ULongKeyword => typeof(ulong),
                TokenType.FloatKeyword => typeof(float),
                TokenType.DoubleKeyword => typeof(double),
                TokenType.DecimalKeyword => typeof(decimal),
                TokenType.StringKeyword => typeof(string),
                TokenType.ObjectKeyword => typeof(object),
                TokenType.BoolKeyword => typeof(bool),
                TokenType.ByteKeyword => typeof(byte),
                TokenType.SbyteKeyword => typeof(sbyte),
                TokenType.ShortKeyword => typeof(short),
                TokenType.UshortKeyword => typeof(ushort),
                TokenType.VoidKeyword => typeof(void),
                TokenType.NintKeyword => typeof(nint),
                TokenType.NuintKeyword => typeof(nuint),
                _ => throw new ArgumentOutOfRangeException()

            };

        public override string ToString()
            => TokenType switch {
                TokenType.CharKeyword => "char",
                TokenType.IntKeyword => "int",
                TokenType.UIntKeyword => "uint",
                TokenType.LongKeyword => "long",
                TokenType.ULongKeyword => "ulong",
                TokenType.FloatKeyword => "float",
                TokenType.DoubleKeyword => "double",
                TokenType.DecimalKeyword => "decimal",
                TokenType.StringKeyword => "string",
                TokenType.ObjectKeyword => "object",
                TokenType.BoolKeyword => "bool",
                TokenType.ByteKeyword => "byte",
                TokenType.SbyteKeyword => "sbyte",
                TokenType.ShortKeyword => "short",
                TokenType.UshortKeyword => "ushort",
                TokenType.VoidKeyword => "void",
                TokenType.NintKeyword => "nint",
                TokenType.NuintKeyword => "nuint",
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    public class LiteralType:IType {
        public LiteralType(StringSegment name) {
            Name = name;
        }
        
        public StringSegment Name { get; }
    

        public Type GetType(CompilingContext context) {
            return context.GetType(Name);
        }

        public override string ToString() => Name.ToString();
    }
    public class LiteralTypeWithDot:IType {
        public LiteralTypeWithDot(List<StringSegment> list) {
            Name = list;
        }
        
        public List<StringSegment> Name { get; }
    

        public Type GetType(CompilingContext context) {
            
            if (context.TryGetSomething(Name[0].GetHashCode(), Name[0], out var something)) {
                var o = something.Item2;
                var depth = 0;
                while (depth++<Name.Count-1) {
                    if (o is NameSpace node) {
                        var name = Name[depth].AsSpan();
                        var hash = name.GetExHashCode();
                        if (node.TryGetTypeOrNameSpace(hash, name, out var type, out var node2)) {
                            if (type is not null) {
                                return type;
                            } 
                            if (node2 is not null) {
                                o = node2;
                            } 
                        }
                    }
                }
               
            }

            throw new KeyNotFoundException("Type cannot be found");
        }

        public override string ToString() => Name.ToString();
    }

      
}
    
