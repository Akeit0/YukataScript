using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using YS.Collections;
using YS.Modules;
using YS.VM;

namespace YS.AST.Expressions {
    public class ListedExpression :IExpression ,IEnumerable<(PassType,IExpression)> {
        private (PassType,IExpression)[] _array;
        public int Count => _count;
        private int _count;

        public ListedExpression() {
            _array = new (PassType,IExpression)[4];
            _count = 0;
        }

        public void Add((PassType,IExpression) param) {
            CollectionsUtility.Add(ref _array,ref _count, param);
        }
        

        public void Clear() {
            _array.AsSpan().Clear();
            _count = 0;
        }
        
        public void ToCode(StringBuilder builder) {
            for (var index = 0; index < Count; index++) {
                var args = _array[index];
                switch (args.Item1) {
                    case PassType.Ref: builder.Append("ref ");
                        break;
                    case PassType.Out: builder.Append("out ");
                        break;
                    case PassType.In: builder.Append("in ");
                        break;
                }
                args.Item2.ToCode(builder);
                if (index != Count - 1) {
                    builder.Append(',');
                }
            }
        }

        public void Compile((ushort id,Variable variable) target, CompilingContext context) {
            throw new NotImplementedException();
        }

        public Span<(PassType,IExpression)>.Enumerator GetEnumerator() {
            return _array.AsSpan(0, _count).GetEnumerator();
        }
        IEnumerator<(PassType,IExpression)> IEnumerable<(PassType,IExpression)>.GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }

        public (ushort id,object obj) Compile( CompilingContext context) {
            throw new NotImplementedException();
        }
    }
}