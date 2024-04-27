using System;
using Game.Enemies.States;
using Game.FSM;
using Game.Interfaces;
using UnityEngine;

namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class SetState : Action
    {
        [SerializeField] private EnemyStatesEnum to;

        private IController<EnemyStatesEnum> _controller;

        protected override void OnAwake()
        {
            _controller = Owner.GetComponent<IController<EnemyStatesEnum>>();
        }

        protected override NodeState OnUpdate()
        {
            if (_controller == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Controller is null in SetState node", Owner);
#endif

                return NodeState.Failure;
            }

            _controller.StateMachine.SetState(to);
            return NodeState.Success;
        }

        private void OnDestroy()
        {
            _controller = null;
        }
    }
}