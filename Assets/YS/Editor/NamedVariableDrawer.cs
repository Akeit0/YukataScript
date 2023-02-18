
#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using YS;
using YS.Editor;
using YS.Fields;
using YS.Modules;


namespace YS.Editor {
    [CustomPropertyDrawer(typeof(NameVariablePair), true)]
    public class NameVariablePairDrawer : PropertyDrawer {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) =>
            NamedVariableDrawer.OnGUIShared(rect, property, label);
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => NamedVariableDrawer.GetPropertyHeightShared(property);
    }

    [CustomPropertyDrawer(typeof(NamedVariable), true)]
    public class NamedVariableDrawer : PropertyDrawer {
        internal static PropertyData _data;

        public static TreeDropDown<Type>  DropDown;
        
        internal static void Init()
        {
            _data ??= new PropertyData();
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) =>
            OnGUIShared(rect, property, label);
        public static void OnGUIShared(Rect rect, SerializedProperty property, GUIContent label) {
            Init();
            EditorGUI.BeginProperty(rect,label,property);
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, property.serializedObject.FindProperty(property.propertyPath).FindPropertyRelative("Name"), GUIContent.none);
            var selectedIndex = -1;
            var variableProperty = property.serializedObject.FindProperty(property.propertyPath)
                .FindPropertyRelative("Variable");
            var variable = variableProperty.managedReferenceValue as Variable;
            if (variable != null) {
                var variableType = variable.GetType();
                selectedIndex = Array.IndexOf(_data.DerivedTypes, variableType);
            }
          

            rect.y += EditorGUIUtility.singleLineHeight;
            string currentName = selectedIndex == -1 ? "Select" : _data.Types[selectedIndex].Item1;
            if (EditorGUI.DropdownButton(rect,new GUIContent(currentName),FocusType.Keyboard)) {
                //DropDown ??= new TypesDropDown(new AdvancedDropdownState(), _data.RefTypes);
                DropDown ??= new TreeDropDown<Type> (new AdvancedDropdownState(), new NameSplitTree<Type>(_data.Types)) {
                    NodeIcon = (Resources.Load("namespaceIcon") as Texture2D),
                };
                
                DropDown.onItemSelected = null;
                DropDown.onItemSelected = (n,item) => {
                    if(item is null) {
                        variableProperty.managedReferenceValue = null;
                        variableProperty.serializedObject.ApplyModifiedProperties();
                        variableProperty.serializedObject.Update();
                    }
                    else {
                       
                        var selectedType = _data.Dictionary[item];
                        variableProperty.managedReferenceValue = Activator.CreateInstance(selectedType);
                        variableProperty.serializedObject.ApplyModifiedProperties();
                        variableProperty.serializedObject.Update();
                        
                    }
                    
                };
                DropDown.Show(rect);
            }
            if (variable != null) {
                rect.y += EditorGUIUtility.singleLineHeight;
                if (variable.type.IsPrimitive) {
                    EditorGUI.PropertyField(rect, variableProperty,  new GUIContent("Value"));
                }
                else EditorGUI.PropertyField(rect, variableProperty,  GUIContent.none);
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => GetPropertyHeightShared(property);
        
        public static float GetPropertyHeightShared(SerializedProperty property) {
            Init();
            var variableProperty = property.serializedObject.FindProperty(property.propertyPath)
                .FindPropertyRelative("Variable");
            var variable = variableProperty.managedReferenceValue  as  Variable ;
            if (variable == null) {
                return EditorGUIUtility.singleLineHeight * 2;
            }
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUI.GetPropertyHeight(variableProperty, GUIContent.none);
        }
        
        
        internal class PropertyData
        {
            public PropertyData()
            {
                DerivedTypes = TypeCache.GetTypesDerivedFrom(typeof(Variable<>)).Where(x =>x!=typeof(TypedObjectVariable)&& !x.IsAbstract)
                    .ToArray();
                Types = new ( string,Type)[DerivedTypes.Length];
                for (var i = 0; i < DerivedTypes.Length; i++)
                {
                    var type = DerivedTypes[i].BaseType.GetGenericArguments()[0];
                    Dictionary.Add(type,DerivedTypes[i]);
                    Types[i] = (type.Build(),type);
                }

                foreach (var t in DerivedTypes) {
                    ModuleLibrary.RegisterNonGeneric((Variable)Activator.CreateInstance(t));
                }
            }
            public Type[] DerivedTypes { get; }

            public ( string,Type)[] Types { get; }

            public Dictionary<Type, Type> Dictionary=new Dictionary<Type, Type>();

        }
    }
}

#endif