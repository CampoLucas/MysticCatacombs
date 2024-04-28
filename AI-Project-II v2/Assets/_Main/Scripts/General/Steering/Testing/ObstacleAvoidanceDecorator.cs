using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Steering.Testing
{
    public class ObstacleAvoidanceDecorator : ObstacleAvoidance, ISteeringDecorator
    {
        public ISteering Child { get; private set; }
        
        public ObstacleAvoidanceDecorator(Transform origin, float angle, float radius, int maxObs, float strength, LayerMask mask) : base(origin, angle, radius, maxObs, strength, mask)
        {
        }
        
        public ObstacleAvoidanceDecorator(ISteering child, Transform origin, float angle, float radius, int maxObs, float strength, LayerMask mask) : base(origin, angle, radius, maxObs, strength, mask)
        {
            Child = child;
        }
        
        public override Vector3 GetDir(Transform target)
        {
            return base.GetDir(target) + Child.GetDir(target);
        }

        public override Vector3 GetDir(Vector3 position)
        {
            return base.GetDir(position) + Child.GetDir(position);
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
    }
}
