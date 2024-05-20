using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class Flee : Seek
    {
        public Flee(Transform origin, float strength) : base(origin, strength)
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
