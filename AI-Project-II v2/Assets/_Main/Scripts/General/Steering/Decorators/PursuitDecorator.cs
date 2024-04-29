using UnityEngine;
using Game.Interfaces;

namespace Game.Entities.Steering
{
    public class PursuitDecorator : Pursuit, ISteeringDecorator
    {
        public ISteering Child { get; private set; }
        
        public PursuitDecorator(Transform origin, float strength, float time) : base(origin, strength, time)
        {
        }
        
        public PursuitDecorator(ISteering child, Transform origin, float strength, float time) : base(origin, strength, time)
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
            Gizmos.DrawRay(Origin.position, Child.CatchDirection);
#endif
        }
    }
}