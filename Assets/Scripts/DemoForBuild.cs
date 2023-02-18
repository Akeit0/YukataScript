
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YS;
using YS.Fields;
using YS.VM;
using YS.UI;
using IngameDebugConsole;
namespace DefaultNamespace {
    [RequireComponent(typeof(UIDocument))]
    public   class DemoForBuild : MonoBehaviour {
        VisualElement _root;

        public Transform square;
        public SpriteRenderer spriteRenderer;
        VirtualMachine _engine=new (32,32,32);

        Compiler _compiler = new Compiler();


        List<SelectableField> _selectableFields;
        [TextArea(3,30)]
        public string Example1;
        [TextArea(3,30)]
        public string Example2;
        [TextArea(3,30)]
        public string Example3;
        [TextArea(3,30)]
        public string Example4;
        
        
        public void Start() {
            Application.targetFrameRate = 60;
            _selectableFields = new(5) {
                SelectableField.Create("duration",4f),
                SelectableField.Create("v3",new Vector3(2,2,1)),
                SelectableField.Create("transform",square),
                SelectableField.Create("color",Color.green),
                SelectableField.Create("spriteRenderer",spriteRenderer),
                SelectableField.Create("easingType",EasingType.InBounce),
            } ;
           
            _root = GetComponent<UIDocument>().rootVisualElement;
            var inputRoot=_root.Q<ScrollView>();
            var inputField=inputRoot.Q<TextField>();
            inputField.value = Example1;

            var examplesElement = new VisualElement() {style = {flexDirection = FlexDirection.Row}};
            var example1 = Example1;
            var example2 = Example2;
            var example3 = Example3;
            var example4 = Example4;
            _ = new I(examplesElement) {
                new Button(()=>inputField.value=example1){text = "example1"},
                new Button(()=>inputField.value=example2){text = "example2"},
                new Button(()=>inputField.value=example3){text = "example3"},
                new Button(()=>inputField.value=example4){text = "example4"},
                new Button(()=>inputField.value=""){text = "Clear"},
            };
            inputRoot.Insert(1,examplesElement);
            var compileButton=new Button(()=>Compile(inputField.value)){text = "Compile"};
            inputRoot.Insert(2,compileButton);
            var runButton=new Button(Run){text = "Run"};
            inputRoot.Insert(3,runButton);
            
            var holder = inputRoot.Q("Variables");
            VariableField.VariableView = holder;
           
            foreach (var field in _selectableFields) {
                holder.Add( new SelfRemoveSelectableField(field,_selectableFields));
            }
            
            var addButton = new Button(() => {
                var newSelectable = new SelectableField();
                _selectableFields.Add(newSelectable);
                holder.Insert(holder.childCount-1, new SelfRemoveSelectableField(newSelectable,_selectableFields));
            }) {
                text = "+",
                style = {
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };
            holder.Add(addButton);

        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.F12)||Input.GetKeyDown(KeyCode.Escape)) {
                _root.visible = !_root.visible;
            }

            foreach (var field in _selectableFields) {
                field.Field.Validate();
            }
        }
        public void Compile(string code) {
            YS.Generated.Wrapper.Init();
            try {
                if(_engine.TryClear()) {
                    _compiler.SourceCode =code;
                    foreach (var selectable in _selectableFields) {
                        var namedVariable = selectable.NamedVariable;
                        if (namedVariable is {IsValid: true}) _engine.AddVariable(namedVariable.Name, namedVariable.Variable);
                    }
                    _compiler.Compile(_engine, true);
                }
            }
            catch (Exception e) {
                _engine.TryClear();
                Debug.LogException(e);
            }
        }
      
        public void Run() {
            using (ElapsedTimeLogger.StartNew("run")) {
                _engine.Process();
            } 
        }
        
    }
    public class SelfRemoveSelectableField : VisualElement {
        public SelfRemoveSelectableField(SelectableField element,List<SelectableField> list) {
            style.flexDirection = FlexDirection.Row;
            element.style.flexGrow = 1;
            Add(element);
            Add(new Button(()=> { 
                    parent.Remove(this);
                    list.Remove(element);
                })
                {style={unityTextAlign = TextAnchor.UpperCenter,width=20,height = new Length(100,LengthUnit.Percent)},text = "-"}
            );
        }
       
    }
}