
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using YS.Collections;
using YS.VM;

namespace YS.AST{
    public sealed class Root 
    {
        public SimpleList<IStatement> Statements { get; set; }
        public SimpleList<ushort> StatementToLine { get; set; }

        public Root(int length = 10) {
            Statements = new SimpleList<IStatement>(length);
            StatementToLine = new SimpleList<ushort>(length);
        }
        public string ToCode()
        {
            var builder = new StringBuilder(32);
            for (var index = 0; index < this.Statements.Count; index++) {
                var ast = this.Statements[index];
                ast.ToCode(builder, 0);
            }

            return builder.ToString();
        }


        public void Compile(CompilingContext context) {
            context.Init(StatementToLine);
            try {
                foreach (var statement in Statements.AsSpan()) {
                    statement.Compile(context);
                }
            }
            catch (Exception) {
               Debug.LogError("line : "+context.GetCurrentLine() );
               throw;
            }
            
        }
        public string ToCode(StringBuilder builder) {
            builder.Clear();
            foreach (var statement in Statements.AsSpan())
            {
                statement.ToCode(builder,0);
            }
            return builder.ToString();
        }
    }
}