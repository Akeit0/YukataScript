using System;
using System.Text;
using UnityEngine;
using YS.AST.Expressions;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.Instructions;
using YS.VM;
using YS.Text;

namespace YS.AST.Expressions {
    public class LinkedIdentifierExpression:IExpression,IPoolNode<LinkedIdentifierExpression> {
        
        static Pool<LinkedIdentifierExpression> _pool;
         public ref LinkedIdentifierExpression NextNode => ref _next;
        private LinkedIdentifierExpression _next;

        public StringSegment Segment;
        public static LinkedIdentifierExpression Create() {
            if (!_pool.TryPop(out var item)) return new LinkedIdentifierExpression();
            return item;
        }

        public int Length {
            get {
                if (_next == null) return 1;
                return _next.Length + 1;
            }
        }
        public void Add(StringSegment segment) {
            if (Segment.Source == null) Segment = segment;
            else if (_next==null) {
                _next = Create();
                _next.Segment = segment;
            }
            else {
                _next.Add(segment);
            }
        }

        public void Dispose() {
            _next?.Dispose();
            Segment = null;
            _next = null;
            _pool.TryPush(this);
        }
        
        public void ToCode(StringBuilder builder) {
            builder.Append(Segment.AsSpan());
            if (_next != null) {
                builder.Append('.');
                _next.ToCode(builder);
            }
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context){
            // if(_next==null) {
            //     var src = context.Get(Segment);
            //     Validator.ValidateAssignment(variableAddress, src, context);
            //     return;
            // }
            //
            // if (context.TryGet(Segment, out var id)) {
            //     var current = _next;
            //     var currentId = id;
            //     while (current.NextNode != null) {
            //         var parentVariable = context.GetVariable(currentId);
            //         var getter= (MethodData)ModuleLibrary.FindFunctions(parentVariable.type,"get_"+current.Segment.ToString());
            //         if (getter.MethodData.MethodType != MethodType.Instance) throw new Exception();
            //         currentId = context.AddVariable(getter.MethodData.ReturnType);
            //         context.EmitData(currentId,id);
            //         context.EmitData(getter.Index);
            //         current = current.NextNode;
            //     }
            //     {
            //         var parentVariable = context.GetVariable(currentId);
            //         var getter= (MethodData)ModuleLibrary.FindFunctions(parentVariable.type,"get_"+current.Segment.ToString());
            //         if (getter.MethodData.MethodType != MethodType.Instance) throw new Exception();
            //         Validator.ValidateReturnAssignment(variableAddress,getter,new ArgumentData(id, ArgumentType.Instance),context);
            //     }
            //     return;
            // }
            //
            // {
            //     Type type = FindType(context,out var current);
            //     var getter= (MethodData)ModuleLibrary.FindFunctions(type,"get_"+current.Segment);
            //     var remain=current.Length;
            //     
            //     context.EmitData(getter.Index);
            //     context.EmitData(variableAddress);
            //     context.Emit(getter.InstructionId);
            //     return;
            //     
           // }
        }

        // private Type FindType(CompilingContext context,out LinkedIdentifierExpression current ) {
        //      current = this;
        //     var list = new ValueList<char>(stackalloc char[32]);
        //     list.AddRange(Segment);
        //     Type type;
        //     while (!context.TryGetType(list.AsSpan(), out type)) {
        //         list.AddRange(current.Segment);
        //         current = current.NextNode;
        //         if (current == null) throw new Exception();
        //     }
        //     return type;
        // }
        // private MethodData FindStaticGetter(Type type,CompilingContext context ) {
        //      var getter = (MethodData) ModuleLibrary.FindFunctions(type, "get_" + _next.Segment.ToString());
        //      if (getter.MethodData.MethodType != MethodType.Static) throw new Exception();
        //      var currentId = context.AddVariable(getter.MethodData.ReturnType);
        //      context.EmitData(currentId);
        //      context.EmitData(getter.Index);
        //      return getter;
        //      
        // }

        public (ushort id,object obj) Compile( CompilingContext context) {
            throw new System.NotImplementedException();
        }

    }
}