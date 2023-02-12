using System;
using System.Collections.Generic;
using UnityEngine;
using YS.Collections;
using YS.Modules;

namespace YS.Fields {
   [Serializable]
    public struct NameVariablePair {
        public string Name;
        public bool IsValid => !string.IsNullOrEmpty(Name);
        [SerializeReference] public Variable Variable;
        public T GetValue<T>() => ((Variable<T>) Variable).value;
        public T GetObject<T>() => (T)Variable.As<object>();
     
        public NameVariablePair(string name, Variable variable) {
            Name = name;
            Variable = variable;
        }
       
        
    }

    [Serializable]
    public  class NamedVariable {
        public NamedVariable() {
            
        }
        public string Name;
        public bool IsValid => !string.IsNullOrEmpty(Name);
        [SerializeReference]
        public Variable Variable;
        public T GetValue<T>() => ((Variable<T>) Variable).value;
        public T GetObject<T>() => (T)Variable.As<object>();

       

        public NamedVariable(string name, Variable variable) {
            Name = name;
            Variable = variable;

        }
     
       
    }
    
  
}