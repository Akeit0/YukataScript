using System.Collections.Generic;
using System.Linq;
using System.Text;
using YS.AST.Expressions;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.VM;
using YS.Text;
namespace YS.AST.Statements {
    public readonly struct TypeNamePair {
        public readonly PassType PassType;
        public readonly object Type;
        public readonly StringSegment Name;

        public TypeNamePair(object type, StringSegment name) {
            PassType = PassType.Value;
            Type = type;
            Name = name;
        }
        public TypeNamePair(PassType passType,object type, StringSegment name) {
            PassType = passType;
            Type = type;
            Name = name;
        }

        public void ToCode(StringBuilder builder) {
            if (Type is MemberExpression memberExpression) {
                memberExpression.ToCode(builder);
            }
            else {
                builder.Append((string) Type);
            }
            builder.Append(' ');
            builder.Append(Name.AsSpan());
        }
    }
    public class FunctionStatement : IStatement {
        public object ReturnType { get; set; }
        public string Name { get; set; }
        public List<TypeNamePair> Parameters { get; set; }
        
        public BlockStatement Body { get; set; }
        
        public void ToCode(StringBuilder builder, int indentLevel) {
            IStatement.AppendIndent(builder,indentLevel);
            switch (ReturnType) {
                case Variable<TokenType> box:
                    builder.Append(TokenInfo.ToString(box.value));
                    break;
                case string str:
                    builder.Append(str);
                    break;
                case MemberExpression memberExpression:
                    memberExpression.ToCode(builder);
                    break;
            }
            builder.Append(' ');
            builder.Append(Name);
            builder.Append(" (");
            if(Parameters!=null) {
                for (var index = 0; index < Parameters.Count; index++) {
                    var parameter = Parameters[index];
                    parameter.ToCode(builder);
                    if (index != Parameters.Count - 1) {
                        builder.Append(',');
                        builder.Append(' ');
                    }
                }
            }
            builder.Append(')');
            Body.ToCode(builder,indentLevel);
        }

        public void Compile(CompilingContext context) {
            throw new System.NotImplementedException();
        }
    }
}