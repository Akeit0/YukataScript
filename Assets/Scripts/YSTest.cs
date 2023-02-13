using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Scripting;
using YS;
using YS.Fields;
using YS.VM;
using YS.Generated;

namespace DefaultNamespace {
    
    [Preserve]
    public class YSTest : MonoBehaviour {
        
        [ReflectionCall]
        public static void HelloWorld() {
           Debug.Log("HelloWorld");
        }
        [ReflectionCall]
        public static void ThisIsTest() {
           Debug.Log("ThisIsTest");
        }

        [ReflectionCall]
        public static void Test(int a,ref int b) {
            b+=a;
        }
       [ReflectionCall]
        public static int Add(int a, int b) {
            return a + b;
        }
        
        
        [TextAreaAttribute(3,20)]
        public string code;

        public TextAsset TextFile;

        public bool DoPerformanceCheck = false;

        VirtualMachine _engine=new (32,32,32);

        Compiler _compiler = new Compiler();

        public List<NameVariablePair> Fields;
       
        [Button]
        public void Compile() {
            Wrapper.Init();
            try {
                if(_engine.TryClear()) {
                    _compiler.SourceCode = TextFile != null ? TextFile.text : code;
                    foreach (var field in Fields) {
                        if (field is {IsValid: true}) _engine.AddVariable(field.Name, field.Variable);
                    }
                    _compiler.Compile(_engine, DoPerformanceCheck);
                }
            }
            catch (Exception e) {
               Debug.LogException(e);
            }
         
        }
        [Button]
        public void Run() {
            if (DoAsync) {
                RunAsync();
                return;
            }
            if (DoPerformanceCheck) {
                using (ElapsedTimeLogger.StartNew("run")) {
                    _engine.Process();
                }
            }
            else {
                _engine.Process();
            }
        }
        
        public bool DoAsync = false;
        async void RunAsync() {
            try {
                await   _engine.RunAsync();
            }
            catch (Exception) {
               Debug.Log("catch");
                throw;
            }
            
            Debug.Log("End");
        }
        [Button]
        public void Cancel() {
             _engine.Cancel();
        }
        [Button]
        public void Stop() {
             _engine.Stop();
        }
        
        [Button]
        public void Restart() {
             _engine.Restart();
        }
        
 
        StringBuilder _builder = new StringBuilder(100);
        [Button]
        public void Show() {
            Debug.Log( _compiler.ToCode());
            for (int i = 0; i < _engine.OpCount; i++) {
                Debug.Log(YS.Instructions.IInstruction.Instructions[_engine.Codes[i]]);
            }
            for (int i = 0; i < _engine.DataCount; i++) {
                Debug.Log(_engine.UnmanagedData[i]);
            }
        }
        [Button]
        public void ShowReadable() {
            _builder.Clear();
          _engine.ToCode(_builder);
          Debug.Log(_builder.ToString());
          
          
        }
       
    }
}