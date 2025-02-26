using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(CardActionContainer))]
public class CardActionContainerDrawer : PropertyDrawer
{
    private ReorderableList _list;
    private List<bool> _foldouts;
    private List<int> _lines;


    private void Init(SerializedProperty property)
    {

        var actionNodesProperty = property.FindPropertyRelative("actionNodes");
        
        if (_foldouts == null)
        {
            _foldouts = new List<bool>();
        }
        
        if (_lines == null)
        {
            _lines = new List<int>();
        }
        
        if (_list == null)
        {
            // ReorderableList 초기화
            _list = new ReorderableList(
                property.serializedObject,
                actionNodesProperty,
                draggable: true,  
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true
            );
            for (int i = 0; i < _list.count; i++)
            {
                _foldouts.Add(false);
                _lines.Add(2);
            }
            // 헤더
            _list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Action Nodes");
            };

            // 노드 그리기
            _list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = actionNodesProperty.GetArrayElementAtIndex(index);
                
                var blueprintProp = element.FindPropertyRelative("actionBlueprint");
                var actionProp    = element.FindPropertyRelative("action");

                float lineHeight  = EditorGUIUtility.singleLineHeight;
                float spacing     = EditorGUIUtility.standardVerticalSpacing;
                
                // 첫 줄 (blueprint)
                Rect blueprintRect = new Rect(
                    rect.x, 
                    rect.y, 
                    rect.width, 
                    lineHeight
                );
                
                // 두 번째 줄 (action)
                Rect actionRect = new Rect(
                    rect.x, 
                    rect.y + lineHeight + spacing, 
                    rect.width, 
                    lineHeight
                );

                EditorGUI.PropertyField(blueprintRect, blueprintProp, new GUIContent("Blueprint"));
                EditorGUI.PropertyField(actionRect, actionProp, new GUIContent("Action"), true);
                while(_foldouts.Count <= index)
                {
                    _foldouts.Add(false);
                }
                while(_lines.Count <= index)
                {
                    _lines.Add(2);
                }

                _foldouts[index] = actionProp.isExpanded;
                _lines[index] = 2 + actionProp.CountInProperty();
            };
            
            _list.elementHeightCallback = (int index) =>
            {
                float lineHeight = EditorGUIUtility.singleLineHeight;
                float spacing = EditorGUIUtility.standardVerticalSpacing;
                while(_foldouts.Count <= index)
                {
                    _foldouts.Add(false);
                }
                while(_lines.Count <= index)
                {
                    _lines.Add(2);
                }
                
                int numLines = _lines[index];
                if (!_foldouts[index])
                {
                    numLines = 2;
                }
                return (lineHeight * numLines) + spacing + 5f; 
            };
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        Init(property);
        
        var labelRect = new Rect(
            position.x, position.y, 
            position.width, EditorGUIUtility.singleLineHeight
        );
        EditorGUI.LabelField(labelRect, label);
        
        float listStartY = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        var listRect = new Rect(
            position.x,
            listStartY,
            position.width,
            _list.GetHeight()
        );
        
        _list.DoList(listRect);

        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Init(property);

        // 라벨 한 줄 + 리스트 높이
        float height = EditorGUIUtility.singleLineHeight 
                       + EditorGUIUtility.standardVerticalSpacing
                       + _list.GetHeight();

        return height;
    }
}
