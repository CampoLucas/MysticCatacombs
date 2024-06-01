using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.MeshCombiner
{
    [CustomEditor(typeof(MeshCombiner))]

    public class MeshCombinerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            var tar = (MeshCombiner)target;

            if (GUILayout.Button("Combine Meshes"))
            {
                tar.CombineMeshes();
            }

            if (tar.HasAMesh())
            {
                if (GUILayout.Button("Save Mesh"))
                {
                    tar.SaveMesh();
                }
            }

        }
    }
}