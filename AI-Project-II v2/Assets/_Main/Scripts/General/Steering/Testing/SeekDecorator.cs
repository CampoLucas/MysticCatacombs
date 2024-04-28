using UnityEngine;

namespace Game.Entities.Steering.Testing
{
    public class SeekDecorator : Seek, ISteeringDecorator
    {
        public ISteering Child { get; private set; }
        
        public SeekDecorator(Transform origin, float strength) : base(origin, strength)
        {
        }
        
        public SeekDecorator(ISteering child, Transform origin, float strength) : base(origin, strength)
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