using System;
using System.Collections.Generic;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Flocking
{
    public class Predator : IFlocking
    {
        private readonly float _multiplier;
        private readonly float _predatorRange;
        private readonly LayerMask _whatIsPredator;
        private Collider[] _colliders;
        private int _predLevel;
        private Levelable _selfLevel;

        public Predator(float multiplier, float predatorRange, int maxPredators, LayerMask predatorMask, Levelable selfLevel)
        {
            _multiplier = multiplier;
            _predatorRange = predatorRange;
            _whatIsPredator = predatorMask;
            _selfLevel = selfLevel;
            _colliders = new Collider[maxPredators];
        }
        
        public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            int count = Physics.OverlapSphereNonAlloc(self.Position, _predatorRange, _colliders, _whatIsPredator);

            if (count < 1) return Vector3.zero;
            
            Vector3 dir = Vector3.zero;
            
            for (int i = 0; i < count; i++)
            {
                var diff = self.Position - _colliders[i].transform.position;
                dir += diff.normalized * (_predatorRange - diff.magnitude);
                
                if(!_colliders[i].TryGetComponent(out EntityModel predator)) continue;

                _predLevel = predator.GetCurrentLevel();
            }

            var lvlDiff = _predLevel - _selfLevel.CurrentLevel;
            var lvlMultiplier = 1;
            if (lvlDiff != 0)
                lvlMultiplier = Math.Sign(lvlDiff);

            return dir.normalized * (_multiplier * lvlMultiplier);
        }

        public void Dispose()
        {
            _colliders = null;
            _selfLevel = null;
        }
    }
}
