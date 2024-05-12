using Game.DesignPatterns.Predicate;
using UnityEngine;

namespace Game.Entities.FieldOfView
{
    public class CheckViewPredicate : IFOVPredicate
    {
        private CheckViewData _data;
        private Transform _origin;
        
        public CheckViewPredicate(Transform origin, CheckViewData data)
        {
            _origin = origin;
            _data = data;
        }
        
        public bool Evaluate(Transform target)
        {
            var position = _origin.position;
            var diff = target.position - position;
            var dirToTarget = diff.normalized;
            var distanceToTarget = diff.magnitude;

            return !Physics.Raycast(position, dirToTarget, out var hit, distanceToTarget, _data.Mask);
        }

        public void Dispose()
        {
            _data = null;
            _origin = null;
        }
    }
    
    [System.Serializable]
    public class CheckViewData
    {
        public bool Enabled => enabled;
        public LayerMask Mask => mask;

        [SerializeField] private bool enabled;
        [SerializeField] private LayerMask mask;

        public bool TryGetPredicate(Transform origin, out IFOVPredicate predicate)
        {
            predicate = enabled ?  new CheckViewPredicate(origin,this) : null;
            return enabled;
        }
    }
}