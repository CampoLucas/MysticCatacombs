using System.Collections.Generic;
using Game.Interfaces;
using UnityEngine;

namespace Game.Managers
{
    public class FlockingHandler : ISteering
    {
        public Vector3 CatchDirection { get; private set; }
        
        protected IBoid Self;
        
        private List<IFlocking> _flockings;
        private List<IBoid> _boids = new();
        private int _boidsCount;
        private int _maxBoids;
        private Collider[] _colliders;
        private LayerMask _boidMask;
        private float _strength;
        private GameObject _owner;
        private float _detectionRange;

        public FlockingHandler(GameObject owner, List<IFlocking> flockings, IBoid self, LayerMask boidMask, float detectionDetectionRange, float strength = 1, int maxBoids = 5)
        {
            Self = self;
            _flockings = flockings;
            _boidMask = boidMask;
            _maxBoids = maxBoids;
            _colliders = new Collider[_maxBoids];
            _strength = strength;
            _owner = owner;
            _detectionRange = detectionDetectionRange;
        }
        
        public Vector3 GetDir(Transform target)
        {
            CatchDirection = GetDirection(target);
            return CatchDirection;
        }

        public Vector3 GetDir(Vector3 position)
        {
            CatchDirection = GetDirection(position);
            return CatchDirection;
        }

        protected virtual Vector3 GetDirection(Transform target)
        {
            return RunFlocking();
        }

        protected virtual Vector3 GetDirection(Vector3 pos)
        {
            return RunFlocking();
        }

        private Vector3 RunFlocking()
        {
            GetNeighbours();

            var dir = Vector3.zero;
            if (_boidsCount > 0)
            {
                for (var i = 0; i < _flockings.Count; i++)
                {
                    var curr = _flockings[i];
                    dir += curr.GetDir(_boids, Self);
                }
            }

            return dir.normalized  * _strength;
        }

        private void GetNeighbours()
        {
            _boids.Clear();
            
            _boidsCount = Physics.OverlapSphereNonAlloc(Self.Position, _detectionRange, _colliders, _boidMask);

            for (var i = 0; i < _boidsCount; i++)
            {
                var curr = _colliders[i];
                if (curr.gameObject == _owner)
                {
                    _boidsCount--;
                }
                
                var boid = curr.GetComponent<IBoid>();
                
                if (boid == null) continue;
                _boids.Add(boid);
            }
        }

        public virtual void Dispose()
        {
            Self = null;

            if (_flockings != null)
            {
                for (var i = 0; i < _flockings.Count; i++)
                {
                    _flockings[i].Dispose();
                }
                _flockings = null;
            }
            
            _boids = null;
            _colliders = null;
            _owner = null;
        }

        public virtual void Draw()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(Self.Position, CatchDirection);
#endif
        }
    }
}