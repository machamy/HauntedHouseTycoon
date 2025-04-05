using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace Drawer
{
    [CustomPropertyDrawer(typeof(RangedFloatVariableSO))]
    public class RangedFloatVariableSODrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var editorWidth = position.width;
            var objRect = new Rect(position.x, position.y, editorWidth/4, position.height);
            
            EditorGUI.ObjectField(objRect, property, GUIContent.none);
            
            if(property.objectReferenceValue != null)
            {
                
                var so = property.objectReferenceValue as ScriptableObject;
                if (so != null)
                {
                    var serializedObject = new SerializedObject(so);
                    var value = serializedObject.FindProperty("value");
                    serializedObject.Update();
                    var valueRect = new Rect(position.x + editorWidth/4, position.y, editorWidth*3/4, EditorGUIUtility.singleLineHeight);
                    var minProp = serializedObject.FindProperty("minValue");
                    var maxProp = serializedObject.FindProperty("maxValue");
                    EditorGUI.Slider(valueRect, value, minProp.floatValue, maxProp.floatValue, GUIContent.none);
                    EditorUtility.SetDirty(so);
                    serializedObject.ApplyModifiedProperties();
                }
            }
            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty();
        }
    }
}