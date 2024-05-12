using System;
using Game.DesignPatterns.Predicate;
using UnityEngine;

namespace Game.Entities.FieldOfView
{
    public class CheckAnglePredicate : IFOVPredicate
    {
        private CheckAngleData _data;
        private Transform _origin;
        
        public CheckAnglePredicate(Transform origin, CheckAngleData data)
        {
            _origin = origin;
            _data = data;
        }
        
        public bool Evaluate(Transform target)
        {
            var forward = _origin.forward;
            var dirToTarget = target.position - _origin.position;
            var angleToTarget = Vector3.Angle(forward, dirToTarget);
            return _data.Angle / 2 > angleToTarget;
        }

        public void Dispose()
        {
            _data = null;
            _origin = null;
        }
    }

    [System.Serializable]
    public class CheckAngleData
    {
        public bool Enabled => enabled;
        public float Angle => angle;

        [SerializeField] private bool enabled;
        [SerializeField] private float angle;

        public bool TryGetPredicate(Transform origin, out IFOVPredicate predicate)
        {
            predicate = enabled ? new CheckAnglePredicate(origin,this) : null;
            return enabled;
        }
    }
}