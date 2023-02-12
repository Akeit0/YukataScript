using System.Text;
using YS.VM;

namespace YS.AST.Expressions {
    public class OutVarNameExpression :IExpression {
        public string Name;

        public OutVarNameExpression(string name) {
            Name = name;
        }
        public void ToCode(StringBuilder builder) {
            throw new System.NotImplementedException();
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context){
            throw new System.NotImplementedException();
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            throw new System.NotImplementedException();
        }
    }
}