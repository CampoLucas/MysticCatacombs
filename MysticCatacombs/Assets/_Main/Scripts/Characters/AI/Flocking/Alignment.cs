using System.Collections.Generic;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Flocking
{
    public class Alignment : IFlocking
    {
        private readonly float _multiplier;
        
        public Alignment(float multiplier)
        {
            _multiplier = multiplier;
        }

        public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            var front = Vector3.zero;
            for (int i = 0; i < boids.Count; i++)
            {
                var boid = boids[i];
                
                if (boid.Velocity.magnitude > 0.01f)
                    front += boid.Front;
            }
            return front.normalized * _multiplier;
        }

        public void Dispose()
        {
            
        }
    }
}
