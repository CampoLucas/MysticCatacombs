﻿using UnityEditor;
using UnityEngine;

namespace Game.UpdateManager.Editor
{
    [CustomPropertyDrawer(typeof(EnumDataContainer<,>))]
    public class EnumDataContainerDrawer : PropertyDrawer
    {
        private const float FOLDOUT_HEIGHT = 16f;
    
        private SerializedProperty _content;
        private SerializedProperty _enumType;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            _content ??= property.FindPropertyRelative("content");
        
            _enumType ??= property.FindPropertyRelative("enumType");

            var height = FOLDOUT_HEIGHT;
            if (!property.isExpanded) return height;
            if (_content.arraySize != _enumType.enumNames.Length)
            {
                _content.arraySize = _enumType.enumNames.Length;
            }

            for (var i = 0; i < _content.arraySize; i++)
            {
                height += EditorGUI.GetPropertyHeight(_content.GetArrayElementAtIndex(i));
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
        
            var foldoutRect = new Rect(position.x, position.y, position.width, FOLDOUT_HEIGHT);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                var addY = FOLDOUT_HEIGHT;
                for (var i = 0; i < _content.arraySize; i++)
                {
                    var rect = new Rect(position.x, position.y + addY, position.width,
                        EditorGUI.GetPropertyHeight(_content.GetArrayElementAtIndex(i)));
                    addY += rect.height;
                    EditorGUI.PropertyField(rect, _content.GetArrayElementAtIndex(i),
                        new GUIContent(_enumType.enumNames[i]), true);
                }

                EditorGUI.indentLevel--;
            }
        
            EditorGUI.EndProperty();
        }
    }
}