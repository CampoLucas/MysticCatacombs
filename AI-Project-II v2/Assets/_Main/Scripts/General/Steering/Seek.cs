using Game.Data;
using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class Seek : ISteering
    {
        public Vector3 CatchDirection { get; protected set; }
        
        protected Transform Origin;
        protected readonly float Strength;
        

        public Seek(Transform origin, float strength)
        {
            Origin = origin;
            Strength = strength;
        }

        public Seek(Transform origin, SteeringData data) : this(origin, data.Strength)
        {
            
        }
        
        public Vector3 GetDir(Transform target)
        {
            CatchDirection = CalculateDir(target);
            return CatchDirection;
        }

        public Vector3 GetDir(Vector3 position)
        {
            CatchDirection = CalculateDir(position);
            return CatchDirection;
        }

        protected virtual Vector3 CalculateDir(Transform target)
        {
            var targetPos = target.position;
            var originPos = Origin.position;
            targetPos.y = originPos.y;
            
            return (targetPos - originPos).normalized * Strength;
        }
        
        protected virtual Vector3 CalculateDir(Vector3 position)
        {
            var targetPos = position;
            var originPos = Origin.position;
            targetPos.y = originPos.y;
            
            return (targetPos - originPos).normalized * Strength;
        }
        
        public virtual void Dispose()
        {
            Origin = null;
        }

        public virtual void Draw()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(Origin.position, CatchDirection);
#endif
        }
    }
}