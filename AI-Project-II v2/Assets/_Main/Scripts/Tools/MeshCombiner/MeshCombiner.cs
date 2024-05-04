using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.MeshCombiner
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

    public class MeshCombiner : MonoBehaviour
    {
        public string savePath = "Assets/CombinedMeshes/";
        private MeshFilter[] _meshFilters;
        private CombineInstance[] _combine;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        public void CombineMeshes()
        {
            _meshFilters = GetComponentsInChildren<MeshFilter>();
            _combine = new CombineInstance[_meshFilters.Length];

            var i = 0;
            while (i < _meshFilters.Length)
            {
                _combine[i].mesh = _meshFilters[i].sharedMesh;
                _combine[i].transform = _meshFilters[i].transform.localToWorldMatrix;
                _meshFilters[i].gameObject.SetActive(false);

                i++;
            }

            _meshFilter = transform.GetComponent<MeshFilter>();
            _meshRenderer = transform.GetComponent<MeshRenderer>();

            var mesh = new Mesh();
            _meshFilter.mesh = mesh;
            _meshFilter.sharedMesh = mesh;
            _meshFilter.sharedMesh.CombineMeshes(_combine);
            transform.gameObject.SetActive(true);

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            if (_meshFilters.Length > 0)
            {
                _meshRenderer.sharedMaterial = _meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
            }
        }

        public void SaveMesh()
        {
            var fileName = gameObject.name + "_CombinedMesh";
            var filePath = savePath + fileName + ".asset";
            AssetDatabase.CreateAsset(_meshFilter.sharedMesh, filePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Combined mesh saved at: " + filePath);
        }

        public bool HasAMesh() => _meshFilter != null && _meshFilter.sharedMesh != null;
    }
}