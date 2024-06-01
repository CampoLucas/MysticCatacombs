using Game.Data;
using Game.DesignPatterns.Predicate;
using Game.Interfaces;
using Game.SO;
using UnityEngine;


namespace Game.Entities.FieldOfView
{
    /// <summary>
    /// This class is used to detect if a target is within the enemy's field of view.
    /// </summary>
    public class FieldOfView : IFOVPredicate
    {
        private IFOVPredicate[] _predicates;

        public FieldOfView(IFOVPredicate[] predicates)
        {
            _predicates = predicates;
        }

        public bool Evaluate(Transform target)
        {
            if (_predicates == null || _predicates.Length == 0) return false;
            
            var result = true;

            for (var i = 0; i < _predicates.Length; i++)
            {
                if (_predicates[i].Evaluate(target)) continue;
                result = false;
                break;
            }

            return result;
        }
        
        // public bool CheckRange(Transform target)
        // {
        //     var distance = Vector3.Distance(_origin.position, target.position);
        //     return distance < _data.Range;
        // }
        //
        // public bool CheckAngle(Transform target)
        // {
        //     var forward = _origin.forward;
        //     var dirToTarget = target.position - _origin.position;
        //     var angleToTarget = Vector3.Angle(forward, dirToTarget);
        //     return _data.Angle / 2 > angleToTarget;
        // }
        //
        // public bool CheckView(Transform target)
        // {
        //     var position = _origin.position;
        //     var diff = target.position - position;
        //     var dirToTarget = diff.normalized;
        //     var distanceToTarget = diff.magnitude;
        //
        //     return !Physics.Raycast(position, dirToTarget, out var hit, distanceToTarget, _data.Mask);
        // }

        /// <summary>
        /// Method used to nullify the references.
        /// </summary>
        public void Dispose()
        {
            if (_predicates != null)
            {
                for (var i = 0; i < _predicates.Length; i++)
                {
                    _predicates[i].Dispose();
                    _predicates[i] = null;
                }

                _predicates = null;
            }
        }
    }
}