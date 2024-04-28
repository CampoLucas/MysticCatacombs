using UnityEngine;

namespace Game.Entities.Steering.Testing
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