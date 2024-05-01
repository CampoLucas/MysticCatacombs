using Game.Data;
using Game.Enemies;
using Game.Managers;
using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "SO/Entities/EnemyStats", order = 2)]
    public class EnemySO : StatSO
    {
        public float AttackRange => attackRange;
        public FieldOfViewData FOV => fieldOfView; 
        public SeekData Seek => seek;
        public PursuitData Pursuit => pursuit;
        public ObstacleAvoidanceData ObstacleAvoidance => obstacleAvoidance;
        public FlockingHandlerData Flocking => flocking;

        [Header("Attack")]
        [SerializeField] private float attackRange = 0.5f;
        
        [Header("Cone Vision")]
        [SerializeField] private FieldOfViewData fieldOfView;
        
        [Header("Steering")] 
        [SerializeField] private SeekData seek;
        [SerializeField] private PursuitData pursuit;
        [SerializeField] private ObstacleAvoidanceData obstacleAvoidance;
        [SerializeField] private FlockingHandlerData flocking;

    }
}