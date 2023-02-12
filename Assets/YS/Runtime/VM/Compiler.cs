using System.Collections.Generic;
using System.Text;
using UnityEngine;
using YS.AST;
using YS.Collections;
using YS.Lexer;
using YS.Text;
namespace YS.VM {
    public class Compiler {
        public string SourceCode;
        readonly SimpleList<TokenInfo> _tokens = new (100);
        readonly StringSegmentList _identifiers = new (100);
        readonly StringBuilder builder = new (100);
        readonly Root _root = new () {Statements = new SimpleList<IStatement>(100)};
        readonly  CompilingContext _compilingContext = new ();
        public  void Compile(VirtualMachine engine,bool checkPerformance=false) {
            if (engine.IsRunning) {
                Debug.LogError("Engine is running. Try to cancel");
                return;
            }
            _tokens.Clear();
            _identifiers.Clear();
            if(checkPerformance) {
                using (ElapsedTimeLogger.StartNew("lex")) {
                    Lexer.Lexer.Tokenize(SourceCode, _tokens, _identifiers);
                }
                using (ElapsedTimeLogger.StartNew("parse")) {
                    Parser.Parser.ParseProgram(_tokens, _identifiers, _root);
                }
                using (ElapsedTimeLogger.StartNew("compile")) {
                    _compilingContext.SetEngine(engine);
                    _root.Compile(_compilingContext);
                }
            }
            else {
                Lexer.Lexer.Tokenize(SourceCode, _tokens, _identifiers);
                Parser.Parser.ParseProgram(_tokens, _identifiers, _root);
                _compilingContext.SetEngine(engine);
                _root.Compile(_compilingContext);
            }
        }
        public string ToCode() {
            return _root.ToCode(builder);
        }
    }
}