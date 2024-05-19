using Game.Enemies;
using Game.SO;
using UnityEngine;

namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class AlertNearbyEnemies : Action
    {
        [SerializeField] private LayerMask enemyMask;
        
        private EnemyModel _model;

        protected override void OnAwake()
        {
            _model = Owner.GetComponent<EnemyModel>();
        }
        

        protected override NodeState OnUpdate()
        {
            if (!_model) return NodeState.Success;

            if (_model.IsFollowing()) return NodeState.Success;
            
            var pos = _model.Position;
            var nearEnemies = Physics.OverlapSphere(pos, _model.GetData<EnemySO>().AlertRange, enemyMask);

            for (var i = 0; i < nearEnemies.Length; i++)
            {
                var other = nearEnemies[i];
                if (other == null || other.gameObject == _model.gameObject) continue;
                var enemy = nearEnemies[i].GetComponent<EnemyModel>();
                if (enemy == null) continue;
                enemy.SetFollowing(true);
            }
            return NodeState.Success;
        }
    }
}