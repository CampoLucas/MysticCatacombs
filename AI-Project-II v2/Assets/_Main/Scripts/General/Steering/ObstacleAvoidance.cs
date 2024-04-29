using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class ObstacleAvoidance : ISteering
    {
        public Vector3 CatchDirection { get; private set; }
        protected Transform Origin;
        private readonly LayerMask _mask;
        private readonly float _radius;
        private readonly float _angle;
        private readonly float _strength;
        private Collider[] _obs;

        public ObstacleAvoidance(Transform origin, float angle, float radius, int maxObs, float strength, LayerMask mask)
        {
            Origin = origin;
            _angle = angle;
            _radius = radius;
            _mask = mask;
            _strength = strength;
            _obs = new Collider[maxObs];
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
            return GetDir() * _strength;
        }

        protected virtual Vector3 CalculateDir(Vector3 position)
        {
            return GetDir() * _strength;
        }

        /// <summary>
        /// A method that calculates a direction to evade an obstacle.
        /// </summary>
        /// <returns></returns>
        private Vector3 GetDir()
        {
            var obsCount = Physics.OverlapSphereNonAlloc(Origin.position, _radius, _obs, _mask);
            var dirToAvoid = Vector3.zero;
            var detectedObs = 0;
            for (var i = 0; i < obsCount; i++)
            {
                var curr = _obs[i];
                var position = Origin.position;
                var closestPoint = curr.ClosestPointOnBounds(position);
                closestPoint.y = position.y;
                var diffToPoint = closestPoint - position;
                var angleToObs = Vector3.Angle(Origin.forward, diffToPoint);
                if (angleToObs > _angle / 2) continue;
                var distance = diffToPoint.magnitude;
                detectedObs++;
                dirToAvoid += -(diffToPoint).normalized * (_radius - distance);
            }

            if (detectedObs != 0)
                dirToAvoid /= detectedObs;

            return dirToAvoid.normalized;
        }

        public virtual void Dispose()
        {
            Origin = null;
            _obs = null;
        }

        public virtual void Draw()
        {
#if UNITY_EDITOR
            Gizmos.DrawRay(Origin.position, CatchDirection);
#endif
        }
    }
}
