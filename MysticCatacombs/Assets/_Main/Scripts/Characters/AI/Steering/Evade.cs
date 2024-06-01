using UnityEngine;

namespace Game.Entities.Steering
{
    public class Evade : Pursuit
    {
        public Evade(Transform origin, float strength, float time) : base(origin, strength, time)
        {
        }

        protected override Vector3 CalculateDir(Transform target)
        {
            return -base.CalculateDir(target);
        }

        protected override Vector3 CalculateDir(Vector3 position)
        {
            return -base.CalculateDir(position);
        }
    }
}
