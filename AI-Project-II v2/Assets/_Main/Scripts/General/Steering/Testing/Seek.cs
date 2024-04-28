using UnityEngine;

namespace Game.Entities.Steering.Testing
{
    public class Seek : ISteering
    {
        protected Transform Origin;
        protected readonly float Strength;

        public Seek(Transform origin, float strength)
        {
            Origin = origin;
            Strength = strength;
        }
        
        public virtual Vector3 GetDir(Transform target)
        {
            return (target.position - Origin.position).normalized * Strength;
        }

        public virtual Vector3 GetDir(Vector3 position)
        {
            return (position - Origin.position).normalized * Strength;
        }
        
        public virtual void Dispose()
        {
            Origin = null;
        }
    }
}