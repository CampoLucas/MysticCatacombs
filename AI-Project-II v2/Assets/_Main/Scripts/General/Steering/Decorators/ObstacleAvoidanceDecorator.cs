using Game.Data;
using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class ObstacleAvoidanceDecorator : ObstacleAvoidance, ISteeringDecorator
    {
        public ISteering Child { get; private set; }
        
        public ObstacleAvoidanceDecorator(Transform origin, float angle, float radius, int maxObs, float strength, 
            LayerMask mask) : base(origin, angle, radius, maxObs, strength, mask)
        {
        }
        
        public ObstacleAvoidanceDecorator(Transform origin, ObstacleAvoidanceData data) : base(origin, data)
        {
        }
        
        
        
        protected override Vector3 CalculateDir(Transform target)
        {
            return Child.GetDir(target) + base.CalculateDir(target);
        }

        protected override Vector3 CalculateDir(Vector3 position)
        {
            return Child.GetDir(position) + base.CalculateDir(position);
        }
        
        public void SetChild(ISteering child)
        {
            Child = child;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (Child != null)
                Child.Dispose();
            Child = null;
        }
        
        public override void Draw()
        {
            base.Draw();
#if UNITY_EDITOR
            Gizmos.color = Color.magenta;
            Child.Draw();
#endif
        }
    }
}
