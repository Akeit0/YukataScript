

using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace YS.Editor {
    [CustomPropertyDrawer(typeof(Variable), true)]
    public class VariableDrawer : PropertyDrawer {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            property = property.serializedObject.FindProperty(property.propertyPath).FindPropertyRelative ("value");
            if(property!=null) 
                EditorGUI.PropertyField(rect, property, new GUIContent(label.text));
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            property = property.serializedObject.FindProperty(property.propertyPath).FindPropertyRelative ("value");
            if(property==null)return EditorGUIUtility.singleLineHeight;
            return EditorGUI.GetPropertyHeight(property, true);
        }
    }
}
#endif