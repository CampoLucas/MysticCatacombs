using UnityEngine;

namespace Game.Entities.Steering.Testing
{
    public class Pursuit : Seek, ISteering
    {
        private readonly float _time;
        private Vector3 _prevPosition;
        
        public Pursuit(Transform origin, float strength, float time) : base(origin, strength)
        {
            _time = time;
        }

        public override Vector3 GetDir(Transform target)
        {
            // do something to check if it is the first time calling the method.

            var targetPos = target.position;
            var originPos = Origin.position;
            var distance = Vector3.Distance(originPos, targetPos);
            var point = targetPos + target.forward *
                Mathf.Clamp(GetTargetVelocity(targetPos).magnitude * _time, -distance, distance);
            var dir = (point - originPos).normalized;
            return dir;
        }

        protected Vector3 GetTargetVelocity(Vector3 pos)
        {
            return pos - _prevPosition;
        }
    }
}