using Game.DesignPatterns.Predicate;
using UnityEngine;

namespace Game.Entities.FieldOfView
{
    public class CheckRangePredicate : IFOVPredicate
    {
        private CheckRangeData _data;
        private Transform _origin;
        
        public CheckRangePredicate(Transform origin, CheckRangeData data)
        {
            _origin = origin;
            _data = data;
        }
        
        public bool Evaluate(Transform target)
        {
            var distance = Vector3.Distance(_origin.position, target.position);
            return distance < _data.Range;
        }

        public void Dispose()
        {
            _data = null;
            _origin = null;
        }
    }
    
    [System.Serializable]
    public class CheckRangeData
    {
        public bool Enabled => enabled;
        public float Range => range;

        [SerializeField] private bool enabled;
        [SerializeField] private float range;

        public bool TryGetPredicate(Transform origin, out IFOVPredicate predicate)
        {
            predicate = enabled ? new CheckRangePredicate(origin,this) : null;
            return enabled;
        }
    }
}