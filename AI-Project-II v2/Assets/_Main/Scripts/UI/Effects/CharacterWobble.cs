using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI.Effects
{
    public class CharacterWobble : MonoBehaviour
    {
        [SerializeField] private Vector2 speed;
        [SerializeField] private AnimationCurve curveSpeed;
        private TMP_Text textMesh;
        private Mesh mesh;
        private Vector3[] vertices;

        private void Start()
        {
            textMesh = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            textMesh.ForceMeshUpdate();
            mesh = textMesh.mesh;
            vertices = mesh.vertices;

            for (var i = 0; i < textMesh.textInfo.characterCount; i++)
            {
                var c = textMesh.textInfo.characterInfo[i];
                var index = c.vertexIndex;

                Vector3 offset = Wobble(Time.unscaledTime + i);
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }

            mesh.vertices = vertices;
            textMesh.canvasRenderer.SetMesh(mesh);
        }

        private Vector2 Wobble(float time) {
            return new Vector2(Mathf.Sin(time * speed.x), Mathf.Cos(time * speed.y));
        }
    }
}
