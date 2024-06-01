using UnityEngine;

namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class DebugLog : Action
    {
        public string message;

        protected override NodeState OnUpdate()
        {
            Debug.Log(message);
            return NodeState.Success;
        }
    }
}