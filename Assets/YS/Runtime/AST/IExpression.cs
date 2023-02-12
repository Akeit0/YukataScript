using System.Text;
using YS.VM;

namespace YS.AST {
    public interface IExpression {
        public void ToCode(StringBuilder builder);
        public void Compile((ushort id,Variable variable) target, CompilingContext context);
        public (ushort id,object obj) Compile( CompilingContext context);
    }
}