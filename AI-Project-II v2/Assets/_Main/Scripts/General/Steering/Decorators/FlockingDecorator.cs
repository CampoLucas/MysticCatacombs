using System.Collections.Generic;
using Game.Interfaces;
using Game.Managers;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class FlockingDecorator : FlockingHandler, ISteeringDecorator
    {
        public ISteering Child { get; private set; }
        
        public FlockingDecorator(List<IFlocking> flockings, IBoid self, LayerMask boidMask, int maxBoids = 5) : base(flockings, self, boidMask, maxBoids)
        {
        }

        public void SetChild(ISteering child)
        {
            Child = child;
        }

        protected override Vector3 GetDirection(Transform target)
        {
            return Child.GetDir(target) + base.GetDirection(target);
        }
        
        protected override Vector3 GetDirection(Vector3 pos)
        {
            return Child.GetDir(pos) + base.GetDirection(pos);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (Child != null) Child.Dispose();
            Child = null;
        }

        public override void Draw()
        {
            base.Draw();
#if UNITY_EDITOR
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(Self.Position, CatchDirection);
#endif
        }
    }
}