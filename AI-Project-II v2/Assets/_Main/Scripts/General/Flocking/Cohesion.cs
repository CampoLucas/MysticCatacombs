using System.Collections.Generic;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Flocking
{
    public class Cohesion : IFlocking
    {
        private readonly float _multiplier;

        public Cohesion(float multiplier)
        {
            _multiplier = multiplier;
        }
        
        public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            var center = Vector3.zero;
            var dir = Vector3.zero;
            var movingBoids = 0;
            
            for (int i = 0; i < boids.Count; i++)
            {
                var boid = boids[i];
                if (boid.Velocity.magnitude > 0.01f)
                {
                    center += boid.Position;
                    movingBoids++;
                }
            }
            
            if (movingBoids > 0)
            {
                center /= movingBoids;
                dir = center - self.Position;
            }
            return dir.normalized * _multiplier;
        }

        public void Dispose()
        {
            
        }
    }
}
