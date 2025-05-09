﻿using System;
using UnityEngine;
using Game.Entities;
using Game.Entities.FieldOfView;
using Game.Interfaces;
using Game.Player;
using Game.SO;
using Game.Pathfinding;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Enemies
{
    public class EnemyModel : EntityModel, IBoid
    {
        private bool _hasVisionCone;
        private EnemySO _data;
        private FieldOfView[] _fieldOfViews;
        private InRange _range;
        private Vector3 _direction = Vector3.zero;
        private bool _isFollowing;
        
        protected override void Awake()
        {
            base.Awake();
            
            _data = GetData<EnemySO>();
            _fieldOfViews = _data.GetFieldOfViews(transform);
            _range = new InRange(transform);
        }


        public float GetMoveAmount() => Mathf.Clamp01(Mathf.Abs(_direction.x) + Mathf.Abs(_direction.z));

        public override void Move(Vector3 dir)
        {
            _direction = dir;
            base.Move(_direction);
        }

        public override void Move(Vector3 dir, float speed)
        {
            _direction = dir;
            base.Move(dir, speed);
        }

        private bool CheckFOV(Transform target, int index)
        {
            return _fieldOfViews != null && 
                   _fieldOfViews.Length != 0 && 
                   index >= 0 && 
                   index < _fieldOfViews.Length &&
                   _fieldOfViews[index].Evaluate(target);
        }
        
        public bool IsTargetInSight(Transform target, int index) => CheckFOV(target, index);
        public bool IsFollowing() => _isFollowing;
        public void SetFollowing(bool isFollowing) => _isFollowing = isFollowing;
        public bool TargetInRange(Transform target) => _range.GetBool(target, CurrentWeapon().Stats.Range);
        public bool IsTargetAlive(IModel target) => target != null && target.IsAlive();

        
        public override void Dispose()
        {
            base.Dispose();
            if (_fieldOfViews != null)
            {
                for (var i = 0; i < _fieldOfViews.Length; i++)
                {
                    _fieldOfViews[i].Dispose();
                    _fieldOfViews[i] = null;
                }
            }
            _fieldOfViews = null;
            _data = null;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            #region AttackRange

            if (CurrentWeapon())
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, CurrentWeapon().Stats.Range);

            }

            #endregion

            #region AlertRange

            var stats = GetData<EnemySO>();
            if (stats && stats.AlertNearby)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(transform.position, stats.AlertRange);
            }

            #endregion
        }

        #if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            var transform1 = transform;
            var forward = transform1.forward;
            var position = transform1.position;
            
            #region FOV

            GetData<EnemySO>().DrawFOVs(transform1);

            #endregion

            #region ObsAvoidance

            Gizmos.color = Color.blue;
            var halfObs = GetData<EnemySO>().ObstacleAvoidance.Angle / 2f;
            var obsLeftRayRotation = Quaternion.AngleAxis(-halfObs, Vector3.up);
            var obsRightRayRotation = Quaternion.AngleAxis(halfObs, Vector3.up);

            var obsLeftRayDirection = obsLeftRayRotation * forward;
            var obsRightRayDirection = obsRightRayRotation * forward;

            
            Gizmos.DrawRay(position, obsLeftRayDirection * GetData<EnemySO>().ObstacleAvoidance.Range);
            Gizmos.DrawRay(position, obsRightRayDirection * GetData<EnemySO>().ObstacleAvoidance.Range);

            Handles.color = new Color(0f, 0f, 1f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.up, obsLeftRayDirection, GetData<EnemySO>().ObstacleAvoidance.Angle, GetData<EnemySO>().ObstacleAvoidance.Range);
            Handles.color = Color.blue;
            Handles.DrawWireArc(position, Vector3.up, obsLeftRayDirection, GetData<EnemySO>().ObstacleAvoidance.Angle, GetData<EnemySO>().ObstacleAvoidance.Range);

            #endregion
        }
        #endif


        public Vector3 Position => Transform.position;
        public Vector3 Front => Transform.forward;
        public Vector3 Velocity => GetVelocity();
    }
}