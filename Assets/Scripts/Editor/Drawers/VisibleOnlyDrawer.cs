using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Drawer
{
    [CustomPropertyDrawer(typeof(VisibleOnly))]
    public class VisibleOnlyDrawer : PropertyDrawer
    {
        Queue<SerializedProperty> queue = new Queue<SerializedProperty>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;
            queue.Clear();
            queue.Enqueue(property.Copy());
            while (queue.Count > 0)
            {
                SerializedProperty currentProperty = queue.Dequeue();
                if (currentProperty.isExpanded)
                {
                    SerializedProperty endProperty = currentProperty.GetEndProperty();
                    while (currentProperty.NextVisible(true) &&
                           !SerializedProperty.EqualContents(currentProperty, endProperty))
                    {
                        height += EditorGUI.GetPropertyHeight(currentProperty, true);
                    }
                }
            }

            return height;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool visible = false;
            if (Application.isPlaying)
            {
                if (((VisibleOnly)attribute).EditableIn == EditableIn.PlayMode)
                {
                    visible = true;
                }
            }
            else
            {
                if (((VisibleOnly)attribute).EditableIn == EditableIn.EditMode)
                {
                    visible = true;
                }
            }
            if (visible)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}