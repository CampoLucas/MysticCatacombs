using TMPro;
using UnityEngine;

namespace Game.UI.Effects
{
    public class CharacterWave : MonoBehaviour
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

            for (var i = 0; i < textMesh.textInfo.characterCount; i++)
            {
                var c = textMesh.textInfo.characterInfo[i];
                var index = c.vertexIndex;

                Vector3 offset = Wobble(Time.time + i);
                ApplyCharacterWave(offset, index);
            }
        }

        private Vector3 Wobble(float time)
        {
            float curveValue = curveSpeed.Evaluate(time);
            return new Vector3(Mathf.Sin(time * speed.x) * curveValue, Mathf.Cos(time * speed.y) * curveValue, 0f);
        }

        private void ApplyCharacterWave(Vector3 offset, int index)
        {
            TMP_TextInfo textInfo = textMesh.textInfo;
            Vector3[] vertices = textInfo.meshInfo[0].vertices;

            vertices[index] += offset;
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;

            textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }
}