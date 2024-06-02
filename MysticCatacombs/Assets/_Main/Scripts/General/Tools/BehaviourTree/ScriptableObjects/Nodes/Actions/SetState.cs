using Game.Interfaces;
using UnityEngine;

namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class SetState : Action
    {
        [SerializeField] private string to;

        private IController _controller;

        protected override void OnAwake()
        {
            _controller = Owner.GetComponent<IController>();
        }

        protected override void OnStart()
        {
            _controller.StateMachine.SetState(to);
        }

        protected override NodeState OnUpdate()
        {
            if (_controller == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"SetState({to.ToString()}): Controller is null in SetState node", Owner);
#endif

                return NodeState.Failure;
            }
            
            return _controller.StateMachine.CanTransition() ? NodeState.Success : NodeState.Running;
        }

        private void OnDestroy()
        {
            _controller = null;
        }
    }
}