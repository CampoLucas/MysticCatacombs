using UnityEngine;

namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class Wait : Action
    {
        public float duration = 1f;

        private float _startTime;

        protected override void OnStart()
        {
            _startTime = Time.time;
        }

        protected override NodeState OnUpdate()
        {
            return Time.time - _startTime >= duration ? NodeState.Success : NodeState.Running;
        }
    }
}