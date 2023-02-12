using System.Text;
using YS.Lexer;
using YS.Modules;
using YS.VM;

namespace YS.AST.Expressions {
    public class KeywordIdentifier:IIdentifier {
        public TokenType TokenType { get; set; }
        

        public KeywordIdentifier(TokenType type) {
            TokenType = type;
        }
        public void ToCode(StringBuilder builder) {
            builder.Append(TokenInfo.ToString(TokenType));
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context){
            throw new System.NotImplementedException();
        }

        public (ushort id,object obj) Compile( CompilingContext context){
            if (TokenType.TryGetType(out var type)) {
                return (0, type);
            }

            return default;
        }

        public (ushort,Variable) AddTypeVariable(CompilingContext context) {
            return context.AddVariable(context.GetType(TokenInfo.ToString(TokenType)));
        }
    }
}