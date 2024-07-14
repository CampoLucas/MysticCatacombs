using System;
using UnityEditor;
using UnityEngine;

namespace Game.UpdateManager
{
    [CustomEditor(typeof(UpdatableBehaviour), true)] [CanEditMultipleObjects]
    public class UpdatableBehaviourEditor : Editor
    {
        private SerializedProperty _layer;
        private SerializedProperty _doUpdate;
        private SerializedProperty _doFixedUpdate;
        private SerializedProperty _doLateUpdate;
        private bool _expanded;

        private void OnEnable()
        {
            _layer = serializedObject.FindProperty("layer");
            _doUpdate = serializedObject.FindProperty("doUpdate");
            _doFixedUpdate = serializedObject.FindProperty("doFixedUpdate");
            _doLateUpdate = serializedObject.FindProperty("doLateUpdate");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space(10);
            
            _expanded = EditorGUILayout.Foldout(_expanded, "UpdatableBehaviour");
            if (_expanded)
            {
                EditorGUILayout.LabelField("Layer", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_layer);
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_doUpdate);
                EditorGUILayout.PropertyField(_doFixedUpdate);
                EditorGUILayout.PropertyField(_doLateUpdate);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}