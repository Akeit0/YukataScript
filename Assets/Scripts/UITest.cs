using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;
using YS;
using YS.Fields;
using YS.UI;

namespace DefaultNamespace {
    [RequireComponent(typeof(UIDocument))]
      public   class UITest : MonoBehaviour {
        VisualElement root;
        public Variable<float> FloatVariable;
        public Variable<Vector3>Vector3Variable;
        public Variable<Color>ColorVariable;
        public Variable<Transform>TransformVariable;
        
        VariableField _floatField;
        VariableField _vector3Field;
        VariableField _colorField;
        VariableField _transformField;
        public void Start() {
            Application.targetFrameRate = 60;
            
           
            root = GetComponent<UIDocument>().rootVisualElement;
            var inputRoot=root.Q<ScrollView>();
          
            VariableField.VariableView = inputRoot;
            var holder = inputRoot.Q("Variables");
            var _ = new I(holder) {
                (_floatField = VariableField.Create(FloatVariable)),
                (_vector3Field = VariableField.Create(Vector3Variable)),
                (_colorField = VariableField.Create(ColorVariable)),
                (_transformField = VariableField.Create(TransformVariable)),
            };
            var button = new Button(() => holder.Insert(holder.childCount-1,new SelfRemoveElement(new SelectableField()))) {
                text = "+",
                style = {
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };
            holder.Add(button);

        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.F12)||Input.GetKeyDown(KeyCode.Escape)) {
                root.visible = !root.visible;
            }
            _floatField.Validate();
            _vector3Field.Validate();
            _colorField.Validate();
            _transformField.Validate();
        
        }
    }

    public class SelfRemoveElement : VisualElement {
        public SelfRemoveElement(VisualElement element) {
            style.flexDirection = FlexDirection.Row;
            element.style.flexGrow = 1;
            Add(element);
            Add(new Button(()=>parent.Remove(this)){style={unityTextAlign = TextAnchor.UpperCenter,width=20,height = new Length(100,LengthUnit.Percent)},text = "-"});
        }
       
    }
}