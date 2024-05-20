using System;
using Game.Enemies;
using UnityEngine;

namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class IsPlayerAlive : Conditional
    {
        private EnemyController _controller;
        private EnemyModel _model;
        private bool _started;

        protected override void OnStart()
        {
            if (_started) return;
            _started = true;

            _controller = Owner.GetComponent<EnemyController>();
            _model = _controller.GetModel<EnemyModel>();
        }

        protected override NodeState OnUpdate()
        {
            if (!_controller)
            {
#if UNITY_EDITOR
                Debug.LogWarning("The EnemyController in the InAttackingRange node is null", Owner);
#endif
                return NodeState.Failure;
            }
            
            if (!_model)
            {
#if UNITY_EDITOR
                Debug.LogWarning("The EnemyModel in the InAttackingRange node is null", Owner);
#endif
                return NodeState.Failure;
            }
            
            if (_controller.Target == null) Debug.LogWarning("Target is null", Owner);

            return _controller.Target != null && _controller.Target.IsAlive()
                ? NodeState.Success
                : NodeState.Failure;
        }

        private void OnDestroy()
        {
            _controller = null;
            _model = null;
        }
    }
}