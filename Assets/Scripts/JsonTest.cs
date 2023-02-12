using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using NaughtyAttributes;
using UnityEngine;
using YS;
using YS.Fields;
using YS.Modules;

namespace DefaultNamespace {
    public class JsonTest :MonoBehaviour {
        public List<NameVariablePair> Fields;
        [TextArea(3,20)]
        public string Json;
         StringBuilder _builder = new StringBuilder();

    
        static Dictionary<string, NamedVariable> _dictionary = new Dictionary<string, NamedVariable>();
       
         [Button()]
        public void ToJson () {
           
            _builder.AppendLine("{");
            for (var i = 0; i < Fields.Count; i++) {
                var namedVariable = Fields[i];
                _builder.Append('"');
                _builder.Append(namedVariable.Name);
                _builder.Append("\":");
                ModuleLibrary.ToJson(namedVariable.Variable, _builder);
                if(i+1 <Fields.Count)_builder.AppendLine(",");
            }
            _builder.AppendLine();
            _builder.Append('}');
            Json = _builder.ToString();
            _builder.Clear();
        }
        
        [Button()]
        public void FromJson() {
            var p = new NameVariablePair(null,null);
            LoadJson(Json,Fields);
        }

      
        public static void LoadJson(ReadOnlySpan<char> json, List<NameVariablePair> list) {
            list.Clear();
            if (json.Length < 9) return;
            int start = 0;
            while (true) {
                while (json[start++] != '"') { }
                int end = start;
                while (json[++end] != '"') { }
                string name = json.Slice(start, end - start).ToString();
                start = end + 1;
                var variable = ModuleLibrary.FromJson(json[start..], out var count);
                list.Add(new NameVariablePair(name,variable));
                start  +=count;
                while (true) {
                    var c = json[start++];
                    if (c == ',') {
                        break;
                    }
                    if (c == '}') return;
                }
            }
            
            
        }
    } 
   
}