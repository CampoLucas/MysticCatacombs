using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Steering
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