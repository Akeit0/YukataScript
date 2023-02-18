using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YS.Fields;
using Object = UnityEngine.Object;

namespace YS.UI {
    public delegate bool Parser<T>(string text, out T value);

    public static class FieldEx {
        public static void Bind(this TextField field, Action<float> setter, Func<float> getter) {
            field.RegisterCallback<FocusOutEvent>(_ => {
                var currentValue = getter();
                field.SetValueWithoutNotify(currentValue == 0 ? "0" : currentValue.ToString(CultureInfo.InvariantCulture));
            });
            field.RegisterValueChangedCallback(e => {
                var newValue = e.newValue;
                if (string.IsNullOrWhiteSpace(newValue)) {
                    setter(0);
                }
                else if (float.TryParse(newValue, out var v)) {
                    setter(v);
                }
            });
        }
        public static void Bind(this TextField field, Action<int> setter, Func<int> getter) {
            field.RegisterCallback<FocusOutEvent>(_ => {
                var currentValue = getter();
                field.SetValueWithoutNotify(currentValue == 0 ? "0" : currentValue.ToString(CultureInfo.InvariantCulture));
            });
            field.RegisterValueChangedCallback(e => {
                var newValue = e.newValue;
                if (string.IsNullOrWhiteSpace(newValue)) {
                    setter(0);
                }
                else if (int.TryParse(newValue, out var v)) {
                    setter(v);
                }
            });
        } 
        public static void Bind(this TextField field, Action<string> setter, Func<string> getter) {
            field.RegisterCallback<FocusOutEvent>(_ => {
                field.SetValueWithoutNotify(getter());
            });
            field.RegisterValueChangedCallback(e => {
                setter(e.newValue);
            });
        }
        public static void Bind<T>(this TextField field, Action<T> setter, Func<T> getter,Parser<T>parser,Func<T,string>  formatter=null) {
            field.RegisterCallback<FocusOutEvent>(_ => {
                field.SetValueWithoutNotify(formatter!=null ?formatter.Invoke(getter()):getter().ToString());
            });
            field.RegisterValueChangedCallback(e => {
                var newValue = e.newValue;
                if (string.IsNullOrWhiteSpace(newValue)) {
                    setter(default);
                }
                else if (parser(newValue, out var v)) {
                    setter(v);
                }
            });
        }
        
    public static VisualElement AddPopup(this VisualElement rootElementForPopup,
        VisualElement popupContent)
        {
            var popupElement = new VisualElement {
                style = {
                    position = Position.Absolute,
                    height = new Length(100, LengthUnit.Percent),
                    width = new Length(100, LengthUnit.Percent)
                }
            };
            var background = new VisualElement {
                style = {
                    position = Position.Absolute,
                    height =new Length(100, LengthUnit.Percent),
                    width =new Length(100, LengthUnit.Percent),
                    opacity = 0.5f,
                    backgroundColor = Color.black
                }
            };
            background.RegisterCallback<PointerDownEvent>(e => {
                e.StopImmediatePropagation();
                rootElementForPopup.Remove(popupElement);
            });
            popupElement.Add(background);
     
            
     
            //Set content size
            popupContent.style.width = new Length(100, LengthUnit.Percent);
            popupContent.style.height =new Length(100, LengthUnit.Percent);
            
            popupContent.style.position =Position.Absolute;

            popupElement.Add(popupContent);
     
            rootElementForPopup.Add(popupElement);
            return popupElement;
        }
    }

    public  class SelectableField : VisualElement {
        public NamedVariable NamedVariable;
        public readonly TextField Name=new ("name");
        public readonly Button Selector=new ();
        public VariableField Field;

        public void SetVariable(Variable variable) {
            if (Field != null) {
                Remove(Field);
            }
            switch (variable) {
                case Variable<int> intVariable: 
                    Field = new IntegerField(intVariable);
                    break;
                case Variable<float> floatVariable: 
                    Field = new FloatField(floatVariable);
                    break;
                case Variable<Color> colorVariable: 
                    Field = new ColorField(colorVariable);
                    break;
                case Variable<Transform> transformVariable: 
                    Field = new ObjectReferenceField<Transform>(transformVariable);
                    break;
                default: throw new NotSupportedException(variable.type.ToString()+" is not supported.");
            }
            Selector.text = Field.Variable.type.ToString();
            Add(Field);
        }

        public SelectableField(bool createNamedVariable=true) {
            Selector.clicked += () => VariableField.InstantiateDropdownField(this);
            if (createNamedVariable) NamedVariable = new NamedVariable();
            Name.Bind(text=>NamedVariable.Name=text,()=>NamedVariable.Name);
            Add(Name);
            Add(Selector);
            style.backgroundColor = Color.gray;

            style.borderBottomColor = Color.black;
            style.borderBottomWidth = 2;
        }

        public static SelectableField Create<T>(string name ,T value) {
            var selectable = new SelectableField(false);
            VariableField variableField;
            if (typeof(T).IsEnum) variableField=new  EnumField<T>(new Variable<T>(value));
            else if (typeof(T).IsSubclassOf(typeof(Object))) variableField=new  ObjectReferenceField<T>(new Variable<T>(value));
            else {
                throw new Exception();
            }
            selectable.Name.value = name;
            selectable.Add(variableField);
            selectable.Selector.text = typeof(T).Name;
            selectable.Field = variableField;
            selectable.NamedVariable = new NamedVariable(name, variableField.Variable);
            return selectable;
        }
        public static SelectableField Create(string name ,float value) {
            var selectable = new SelectableField(false);
            VariableField variableField=new FloatField(new Variable<float>(value));
            selectable.Name.value = name;
            selectable.Add(variableField);
            selectable.Selector.text = "float";
            selectable.Field = variableField;
            selectable.NamedVariable = new NamedVariable(name, variableField.Variable);
            return selectable;
        }
        public static SelectableField Create(string name ,Vector3 value) {
            var selectable = new SelectableField(false);
            VariableField variableField=new Vector3Field(new Variable<Vector3>(value));
            selectable.Name.value = name;
            selectable.Add(variableField);
            selectable.Selector.text = "Vector3";
            selectable.Field = variableField;
            selectable.NamedVariable = new NamedVariable(name, variableField.Variable);
            return selectable;
        }
         public static SelectableField Create(string name ,Color value) {
            var selectable = new SelectableField(false);
            VariableField variableField=new ColorField(new Variable<Color>(value));
            selectable.Name.value = name;
            selectable.Add(variableField);
            selectable.Selector.text = "Color";
            selectable.Field = variableField;
            variableField.style.backgroundColor = value;
            selectable.NamedVariable = new NamedVariable(name, variableField.Variable);
            return selectable;
        }
        public static SelectableField Create(string name ,int value) {
            var selectable = new SelectableField(false);
            VariableField variableField=new IntegerField(new Variable<int>(value));
            selectable.Name.value = name;
            selectable.Add(variableField);
            selectable.Field = variableField;
            selectable.NamedVariable = new NamedVariable(name, variableField.Variable);
            return selectable;
        }
        

        public  SelectableField (NamedVariable namedVariable) {
           
            Selector.clicked += () => VariableField.InstantiateDropdownField(this);
            NamedVariable = namedVariable;
            Name.Bind(text=>NamedVariable.Name=text,()=>NamedVariable.Name);
            Add(Name);
            Add(Selector);
            style.backgroundColor = Color.gray;

            style.borderBottomColor = Color.black;
            style.borderBottomWidth = 2;
            var variable = namedVariable.Variable;
            var typeName = namedVariable.Variable.type.Name;
            Selector.text = typeName;
            VariableField newField;
            switch (variable) {
                case Variable<int> intVariable: 
                    newField = new IntegerField(intVariable);
                    break;
                case Variable<float> floatVariable: 
                    newField = new FloatField(floatVariable);
                    break;
                case Variable<Color> colorVariable: 
                    newField = new ColorField(colorVariable);
                    break;
                case Variable<Transform> transformVariable: 
                    newField = new ObjectReferenceField<Transform>(transformVariable);
                    break;
                case Variable<EasingType> easingType:
                    newField = new EnumField<EasingType>(easingType);
                    break;
                case Variable<Vector3> vector3Variable:
                    newField = new Vector3Field(vector3Variable);
                    break;
                default: throw new NotSupportedException(variable.type.ToString()+" is not supported.");
            }
            
            Add(newField);
            
        }
    }

    public abstract class VariableField :VisualElement {
        static VariableField() {
            RegisterField("int",()=>Create(new Variable<int>()));
            RegisterField("float",()=>Create(new Variable<float>()));
            RegisterField("Transform",()=>Create(new Variable<Transform>()));
            RegisterField("Color",()=>Create(new Variable<Color>()));
            RegisterField("Vector3",()=>Create(new Variable<Vector3>()));
            RegisterField("EasingType",()=>Create(new Variable<YS.EasingType>()));
        }
        public Variable Variable;
        public static readonly List<string> Names=new ();
        public static readonly List<Func<VariableField>> Creator=new ();

         static DropdownField _dropdownField;
        static VisualElement _popupElement;
        static SelectableField _target;
        public static VisualElement VariableView;
        
        public static void InstantiateDropdownField(SelectableField selectable) {
            var grandParent = VariableView;
            if(_dropdownField==null) {
                _dropdownField = new DropdownField();
                _dropdownField.style.maxHeight = 20;
                _dropdownField.style.position = Position.Absolute;
                _dropdownField.choices = Names;
                _dropdownField.value = "select";
                _dropdownField.RegisterValueChangedCallback(_ => {
                    _target.Selector.text = Names[_dropdownField.index];
                    VariableView.Remove(_popupElement);
                    if (_target.Field != null) {
                        _target.Remove(_target.Field );
                    }
                    var newField = Creator[_dropdownField.index]();
                    _target.Field = newField;
                    _target.NamedVariable.Variable = newField.Variable;
                    _target.Add(newField);
                    _dropdownField.index = -1;
                });
            }

            
            _target = selectable;
           var targetPos = selectable.worldTransform.GetPosition() - grandParent.worldTransform.GetPosition();
           _dropdownField.transform.position =targetPos;
            _dropdownField.value = "select";
            if (_popupElement != null) {
                grandParent.Add( _popupElement);
            }
            else {
                _popupElement= grandParent.AddPopup(_dropdownField);
            }
           
        }
        public static void RegisterField(string name,Func<VariableField> func) {
            Names.Add(name);
            Creator.Add(func);
        }
        public static VariableField Create<T>(Variable<T> variable) {
            if (typeof(T).IsEnum) return new EnumField<T>(variable);
            if (typeof(T).IsSubclassOf(typeof(Object))) return new ObjectReferenceField<T>(variable);
            return null;
        }
        
        public static FloatField Create(Variable<float> variable) {
            return new FloatField(variable);
        }public static IntegerField Create(Variable<int> variable) {
            return new IntegerField(variable);
        }
        public static Vector3Field Create(Variable<Vector3> variable) {
            return new Vector3Field(variable);
        }
        public static ColorField Create(Variable<Color> variable) {
            return new ColorField(variable);
        }
         public static StringField Create(Variable<string> variable) {
             return new StringField(variable);
        }
        
        public abstract bool Validate();
    }
    
    public abstract class VariableField<T>:VariableField {
        public T CurrentValue;
        
    }
    public  class StringField:VariableField<string> {
       
        public TextField TextField=new TextField();
        public override bool Validate() {
            if (Variable == null) return false;
            if (Variable.As<string>() != CurrentValue) {
                CurrentValue = Variable.As<string>();
                TextField.SetValueWithoutNotify(CurrentValue);
            }
            
            return true;
        }

        public StringField():this (new Variable<string>()){}
        public StringField(Variable<string> variable) {
            Variable = variable;
            CurrentValue = Variable.As<string>();
            TextField.value = CurrentValue.ToString(CultureInfo.InvariantCulture);
            TextField.Bind(f => Variable.As<string>() = CurrentValue = f,
                () => CurrentValue = Variable.As<string>());
            Add(TextField);
        }
    }
    public  class IntegerField:VariableField<int> {
       
        public TextField TextField=new TextField();
        public override bool Validate() {
            if (Variable == null) return false;
            if (Variable.As<int>() != CurrentValue) {
                CurrentValue = Variable.As<int>();
                TextField.SetValueWithoutNotify( CurrentValue.ToString(CultureInfo.InvariantCulture));
            }
            
            return true;
        }

        public IntegerField():this (new Variable<int>()){}
        public IntegerField(Variable<int> variable) {
            Variable = variable;
            CurrentValue = Variable.As<int>();
            TextField.value = CurrentValue.ToString(CultureInfo.InvariantCulture);
            TextField.Bind(f => Variable.As<int>() = CurrentValue = f,
                () => CurrentValue = Variable.As<int>());
            Add(TextField);
        }
    }
    public  class FloatField:VariableField<float> {
        public TextField TextField=new TextField();
        public override bool Validate() {
            if (Variable == null) return false;
            if (Variable.As<float>() != CurrentValue) {
                CurrentValue = Variable.As<float>();
                TextField.value = CurrentValue.ToString(CultureInfo.InvariantCulture);
            }
            return true;
        }
        public FloatField(Variable<float> variable) {
            Variable = variable;
            CurrentValue = Variable.As<float>();
            TextField.value = CurrentValue.ToString(CultureInfo.InvariantCulture);
            TextField.Bind(f => Variable.As<float>() = CurrentValue = f,
                () => CurrentValue = Variable.As<float>());
            Add(TextField);
        }
        public FloatField() {
            TextField.value = CurrentValue.ToString(CultureInfo.InvariantCulture);
            TextField.Bind(f => Variable.As<float>() = CurrentValue = f,
                () => CurrentValue = Variable.As<float>());
            Add(TextField);
        }
    }

    public class EnumField<T> : VariableField<T> {
        public DropdownField DropdownField=new DropdownField();
        public static T[] All;
        public override bool Validate() {
            if (Variable == null) return false;
            
            if (!EqualityComparer<T>.Default.Equals(Variable.As<T>(), CurrentValue)) {
                CurrentValue = Variable.As<T>();
                DropdownField.SetValueWithoutNotify(CurrentValue.ToString());
            }
            return true;
        }
        public EnumField(Variable<T> variable) {
            Variable = variable;
            CurrentValue = Variable.As<T>();
            All??= (T[])Enum.GetValues(typeof(T));
            DropdownField.choices = All.Select(t => t.ToString()).ToList();
            DropdownField.RegisterValueChangedCallback(_ => {
                    CurrentValue = Variable.As<T>() = All[DropdownField.index];
                }
            );
            DropdownField.value =  CurrentValue.ToString();
            Add(DropdownField);
        }
    }
  

    public class ObjectReferenceField<T> : VariableField<T>{
        public DropdownField DropdownField=new DropdownField();
        static string _nullName = "None (" + typeof(T).Name+")";
        public List<Object> _refList;
        public override bool Validate() {
            if (Variable == null) return false;
            if (Variable.As<object>() != (object)CurrentValue) {
                CurrentValue = Variable.As<T>();
                DropdownField.SetValueWithoutNotify((Object) (object) CurrentValue == null ? _nullName : CurrentValue.ToString());
            }
            return true;
        }
        public void UpdateList() {
            DropdownField.choices =ToList (Object.FindObjectsOfType(typeof(T)),typeof(T));
        }
        public ObjectReferenceField(Variable<T> variable) {
            if (!typeof(T).IsSubclassOf(typeof(Object))) throw new Exception();
            Variable = variable;
            CurrentValue = Variable.As<T>();
            DropdownField.choices =ToList ( Object.FindObjectsOfType(typeof(T)),typeof(T));
            DropdownField.value = ((Object) (object) CurrentValue == null) ? _nullName : CurrentValue.ToString();
            DropdownField.RegisterValueChangedCallback(_ => {
                    var index = DropdownField.index;
                    if(0<=index)
                        CurrentValue = Variable.As<T>() = (T)(object)_refList[index];
                    else {
                        CurrentValue = Variable.As<T>() = default;
                    }
                }
            );
            Add(DropdownField);
        }
         List<string> ToList(Object[] array,Type type) {
            var list = new List<string>(array.Length + 1) {_nullName};
            _refList = new List<Object>(array.Length + 1) {null};
            if (type.IsSubclassOf(typeof(Component))) {
                foreach (var o in array) {
                    if(!IsDontDestroyOnLoad((Component)o)){
                        _refList.Add(o);
                        list.Add(o.ToString());
                    }
                }
            }
            else {
                foreach (var o in array) {
                    _refList.Add(o);
                    list.Add(o.ToString());
                }
            }
            

            return list;
        }
        public static bool IsDontDestroyOnLoad( GameObject gameObject) {
            return gameObject.scene.name == "DontDestroyOnLoad";
        }
        public static bool IsDontDestroyOnLoad( Component component) {
            return component.gameObject.scene.name == "DontDestroyOnLoad";
        }
    }
    
    public  class Vector3Field:VariableField<Vector3> {
        public TextField X=new TextField();
        public TextField Y=new TextField();
        public TextField Z=new TextField();

        public override bool Validate() {
            if (Variable == null) return false;
            var newValue = Variable.As<Vector3>();
            if (newValue.x != CurrentValue.x) {
                X.SetValueWithoutNotify(newValue.x.ToString(CultureInfo.InvariantCulture));
            }
            if (newValue.y != CurrentValue.y) {
                Y.SetValueWithoutNotify(newValue.y.ToString(CultureInfo.InvariantCulture));
            }
            if (newValue.z != CurrentValue.z) {
                Z.SetValueWithoutNotify(newValue.z.ToString(CultureInfo.InvariantCulture));
            }
            CurrentValue = newValue;
            
            return true;
        }
        
        public Vector3Field(Variable<Vector3> variable) {
           style.flexDirection = FlexDirection.Row;
            Variable = variable;
            CurrentValue = Variable.As<Vector3>();
            X.style.width =  Length.Percent(31);
            Y.style.width =  Length.Percent(31);
            Z.style.width =  Length.Percent(31);
            X.label = "X";
            Y.label = "Y";
            Z.label = "Z";
            X.value =CurrentValue.x ==0? "0" : CurrentValue.x.ToString(CultureInfo.InvariantCulture);
            Y.value =CurrentValue.y ==0? "0" : CurrentValue.y.ToString(CultureInfo.InvariantCulture);
            Z.value =CurrentValue.z ==0? "0" : CurrentValue.z.ToString(CultureInfo.InvariantCulture);

            X.Bind(f => Variable.As<Vector3>().x = CurrentValue.x = f,
                () => CurrentValue.x = Variable.As<Vector3>().x);
            Y.Bind(f => Variable.As<Vector3>().y = CurrentValue.y = f,
                () => CurrentValue.y = Variable.As<Vector3>().y);
            Z.Bind(f => Variable.As<Vector3>().z = CurrentValue.z = f,
                () => CurrentValue.z = Variable.As<Vector3>().z);
           
            _ = new I(this) {
                X,Y, Z
            };
        }
    }
    
    public  class ColorField:VariableField<Color> {
       
        public TextField R=new TextField();
        public TextField G=new TextField();
        public TextField B=new TextField();
        public TextField A=new TextField();

        public override bool Validate() {
            if (Variable == null) return false;
            var newValue = Variable.As<Color>();
            if (newValue.r != CurrentValue.r) {
                R.SetValueWithoutNotify(newValue.r.ToString(CultureInfo.InvariantCulture));
            }
            if (newValue.g != CurrentValue.g) {
                G.SetValueWithoutNotify( newValue.g.ToString(CultureInfo.InvariantCulture));
            }
            if (newValue.b != CurrentValue.b) {
                B.SetValueWithoutNotify( newValue.b.ToString(CultureInfo.InvariantCulture));
            }if (newValue.a != CurrentValue.a) {
                A.SetValueWithoutNotify( newValue.a.ToString(CultureInfo.InvariantCulture));
            }
            CurrentValue = newValue;
            if (style.backgroundColor.value != newValue) {
                style.backgroundColor= newValue;
            }
            return true;
        }
        
        public ColorField(Variable<Color> variable) {
            style.flexDirection = FlexDirection.Row;
           usageHints |= UsageHints.DynamicColor;
            Variable = variable;
            CurrentValue = Variable.As<Color>();
            R.style.width =  Length.Percent(23);
            G.style.width =  Length.Percent(23);
            B.style.width =  Length.Percent(23);
            A.style.width =  Length.Percent(23);
            R.label = "R";
            G.label = "G";
            B.label = "B";
            A.label = "A";
            R.value =CurrentValue.r ==0? "0" : CurrentValue.r.ToString(CultureInfo.InvariantCulture);
            G.value =CurrentValue.g ==0? "0" : CurrentValue.g.ToString(CultureInfo.InvariantCulture);
            B.value =CurrentValue.b ==0? "0" : CurrentValue.b.ToString(CultureInfo.InvariantCulture);
            A.value =CurrentValue.a ==0? "0" : CurrentValue.a.ToString(CultureInfo.InvariantCulture);
            R.Bind(f => {
                    Variable.As<Color>().r = CurrentValue.r = Mathf.Clamp01(f);
                    style.backgroundColor= CurrentValue;
                },
                () => CurrentValue.r = Variable.As<Color>().r);
            G.Bind(f =>  {
                    Variable.As<Color>().g = CurrentValue.g = Mathf.Clamp01(f);
                    style.backgroundColor= CurrentValue;
                },
                () => CurrentValue.g = Variable.As<Color>().g);
            B.Bind(f =>  {
                    Variable.As<Color>().b = CurrentValue.b = Mathf.Clamp01(f);
                    style.backgroundColor= CurrentValue;
                },
                () => CurrentValue.b = Variable.As<Color>().b);
            A.Bind(f =>  {
                    Variable.As<Color>().a = CurrentValue.a = Mathf.Clamp01(f);
                    style.backgroundColor= CurrentValue;
                },
                () => CurrentValue.a = Variable.As<Color>().a);
            
            _ = new I(this) {
                R, G, B, A
            };
        }

    }
    
    
}