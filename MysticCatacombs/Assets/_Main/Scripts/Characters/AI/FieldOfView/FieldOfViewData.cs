using System.Collections.Generic;
using Game.DesignPatterns.Predicate;
using Game.Enemies;
using UnityEngine;

namespace Game.Entities.FieldOfView
{
    [System.Serializable]
    public class FieldOfViewData
    {
        [SerializeField] private string name;
        [SerializeField] private CheckRangeData rangeData;
        [SerializeField] private CheckAngleData angleData;
        [SerializeField] private CheckViewData viewData;

        public FieldOfView GetFieldOfView(Transform origin)
        {
            var predicates = new List<IFOVPredicate>(1);
            
            if (rangeData.TryGetPredicate(origin, out var range))
            {
                predicates.Add(range);
            }
            if (angleData.TryGetPredicate(origin, out var angle))
            {
                predicates.Add(angle);
            }
            if (viewData.TryGetPredicate(origin, out var view))
            {
                predicates.Add(view);
            }

            return new FieldOfView(predicates.ToArray());
        }
        
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void DebugGizmos(Transform origin, Color color)
        {
            var forward = origin.forward;
            var position = origin.position;
            var angle = angleData.Enabled ? angleData.Angle : 360;
            var range = rangeData.Enabled ? rangeData.Range : 2;
            
            Gizmos.color = color;
            var halfFOV = angleData.Angle / 2f;
            var leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            var rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

            
            var leftRayDirection = leftRayRotation * forward;
            var rightRayDirection = rightRayRotation * forward;

            
            Gizmos.DrawRay(position, leftRayDirection * range);
            Gizmos.DrawRay(position, rightRayDirection * range);

#if UNITY_EDITOR
            UnityEditor.Handles.color = color - new Color(0, 0, 0, 0.9f);
            UnityEditor.Handles.DrawSolidArc(position, Vector3.up, leftRayDirection, angle, range);
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawWireArc(position, Vector3.up, leftRayDirection, angle, range);
#endif
        }
        
    }
}