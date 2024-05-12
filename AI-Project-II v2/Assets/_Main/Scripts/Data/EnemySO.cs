using System.Collections.Generic;
using Game.Data;
using Game.Entities.FieldOfView;
using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "SO/Entities/EnemyStats", order = 2)]
    public class EnemySO : StatSO
    {
        public float AttackRange => attackRange;
        public FieldOfViewData[] FOVs => fieldOfViews; 
        public SeekData Seek => seek;
        public PursuitData Pursuit => pursuit;
        public ObstacleAvoidanceData ObstacleAvoidance => obstacleAvoidance;
        public FlockingHandlerData Flocking => flocking;

        [Header("Attack")]
        [SerializeField] private float attackRange = 0.5f;
        
        [Header("Cone Vision")]
        [SerializeField] private FieldOfViewData[] fieldOfViews;
        
        [Header("Steering")] 
        [SerializeField] private SeekData seek;
        [SerializeField] private PursuitData pursuit;
        [SerializeField] private ObstacleAvoidanceData obstacleAvoidance;
        [SerializeField] private FlockingHandlerData flocking;

        public FieldOfView[] GetFieldOfViews(Transform transform)
        {
            if (fieldOfViews.Length == 0) return null;
            var fovs = new FieldOfView[fieldOfViews.Length];

            for (var i = 0; i < fieldOfViews.Length; i++)
            {
                fovs[i] = fieldOfViews[i].GetFieldOfView(transform);
            }

            return fovs;
        }

        public void DrawFOVs(Transform transform)
        {
            if (fieldOfViews == null) return;
            
            for (var i = 0; i < fieldOfViews.Length; i++)
            {
                fieldOfViews[i].DebugGizmos(transform, new Color(0.2f * (i + 1), 0, 0));
            }
        }

    }
}