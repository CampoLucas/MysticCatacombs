using UnityEngine;

namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class Wait : Action
    {
        public float duration = 1f;
        public bool randomDuration;
        public Vector2 randomTime;

        private float _startTime;
        private float _duration;

        protected override void OnStart()
        {
            _startTime = Time.time;
            _duration = randomDuration ? Random.Range(randomTime.x, randomTime.y) : duration;
        }

        protected override NodeState OnUpdate()
        {
            return Time.time - _startTime >= _duration ? NodeState.Success : NodeState.Running;
        }
    }
}