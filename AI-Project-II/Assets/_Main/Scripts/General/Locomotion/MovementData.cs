using UnityEngine;

namespace Project.Locomotion
{
    [System.Serializable]
    public class MovementData
    {
        [SerializeField] private float speed;
        [SerializeField] private AnimationCurve speedCurve;

        public float GetSpeed(float t) => speed * speedCurve.Evaluate(t);
    }
}