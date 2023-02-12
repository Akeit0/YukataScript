using System.Collections.Generic;
using UnityEngine;
using System.Text;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.VM;
using YS.Text;
namespace YS.AST.Expressions {
    public interface IIdentifier : IExpression {
        public (ushort id ,Variable variable) AddTypeVariable(CompilingContext context);
    }
    
    public class IdentifierLiteral : IIdentifier {
        public StringSegment Name { get; }

        public IdentifierLiteral(StringSegment value)
        {
            Name = value;
        }
        public  void ToCode(StringBuilder builder) {
            builder.Append(Name.AsSpan());
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context){
            var src = context.Get(Name);
            Validator.ValidateAssignment(target, src,context);
          
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            var nameSpan = Name.AsSpan();
            var hash = Name.GetHashCode();
            if (context.TryGetSomething(hash, nameSpan, out var something)) {
                return something;
            }
            // if (context.TryGet(hash,nameSpan, out var id)) {
            //     return (id, context.GetVariable(id));
            // }
            // if (context.TryGetFunctions(hash,nameSpan, out var functions)) {
            //     return (0, functions);
            // }
            // if (context.TryGetType(hash,nameSpan, out var type)) {
            //     return (0, type);
            // }
            throw new KeyNotFoundException(nameSpan.ToString());
        }

       
        public (ushort,Variable) AddTypeVariable(CompilingContext context) {
            return context.AddVariable(context.GetType(Name));
        }
    }
}