using UnityEngine;

namespace Game.Entities.Steering
{
    public class Pursuit : Seek
    {
        private readonly float _time;
        private Vector3 _prevPosition;
        
        public Pursuit(Transform origin, float strength, float time) : base(origin, strength)
        {
            _time = time;
        }

        protected override Vector3 CalculateDir(Transform target)
        {
            // do something to check if it is the first time calling the method.

            var targetPos = target.position;
            var originPos = Origin.position;
            targetPos.y = originPos.y;
            var targetVelocity = GetTargetVelocity(targetPos).magnitude;
            _prevPosition = targetPos;

            // if (targetVelocity > 0.01f)
            // {
            //     
            // }
            //
            // return (targetPos - originPos).normalized * Strength;
            var distance = Vector3.Distance(originPos, targetPos);
            var point = targetPos + target.forward *
                Mathf.Clamp(targetVelocity * _time, -distance, distance);
            
            var dir = (point - originPos).normalized;
            return dir * Strength;
        }

        private Vector3 GetTargetVelocity(Vector3 pos)
        {
            return pos - _prevPosition;
        }
    }
}