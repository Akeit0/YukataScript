using System;
using System.Text;
using YS.VM;

namespace YS.AST.Expressions {
    public class TypeOfExpression:IExpression {

        public IType Type;

        public TypeOfExpression(IType type) {
            Type = type;
        }
        public void ToCode(StringBuilder builder) {
            builder.Append("typeof(");
            builder.Append(Type.ToString());
            builder.Append(")");
        }

        public void Compile((ushort id, Variable variable) target, CompilingContext context) {
            if (target.variable is Variable<Type> or Variable<object>) {
               var src= context.AddVariable(new Variable<Type>(Type.GetType(context)));
               context.EmitCopyObject(target.id,src);
               return;
            }
            throw new Exception();

        }

        public (ushort id, object obj) Compile(CompilingContext context) {
            var typeVariable = new Variable<Type>(Type.GetType(context));
            var id= context.AddVariable(typeVariable);
            return (id, typeVariable);
        }
    }
}