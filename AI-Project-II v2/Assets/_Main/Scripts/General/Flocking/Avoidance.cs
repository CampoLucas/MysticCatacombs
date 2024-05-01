using System.Collections.Generic;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Flocking
{
    public class Avoidance : IFlocking
    {
        private readonly float _personalRange;
        private readonly float _multiplier;

        public Avoidance(float multiplier, float personalRange)
        {
            _multiplier = multiplier;
            _personalRange = personalRange;
        }

        public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            Vector3 dir = Vector3.zero;
            for (int i = 0; i < boids.Count; i++)
            {
                Vector3 diff = self.Position - boids[i].Position;
                float distance = diff.magnitude;
                if (distance > _personalRange) continue;
                dir += diff.normalized * (_multiplier - distance);
            }

            return dir.normalized * _multiplier;
        }

        public void Dispose()
        {
            
        }
    }
}