using System;
using Game.Scripts.VisionCone;
using UnityEngine;
using Game.Entities;
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
        [SerializeField] private float radius;
        
        private VisionCone visionCone;
        private bool _hasVisionCone;
        private EnemySO _data;
        private FieldOfView _fieldOfView;
        private PathToFollow _path;
        private InRange _range;
        private Vector3 _direction = Vector3.zero;
        private bool _isFollowing;

        
        protected override void Awake()
        {
            base.Awake();
            
            _data = GetData<EnemySO>();
            _fieldOfView = new FieldOfView(_data.FOV, transform);
            _path = GetComponent<PathToFollow>();
            _range = new InRange(transform);
            visionCone = GetComponentInChildren<VisionCone>();

            if (visionCone != null)
            {
                _hasVisionCone = true;
                visionCone.SetMesh(_data.FOV);
            }
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

        private bool CheckRange(Transform target) => _fieldOfView.CheckRange(target);
        private bool CheckAngle(Transform target) => _fieldOfView.CheckAngle(target);
        private bool CheckView(Transform target) => _fieldOfView.CheckView(target);

        public void Spawn()
        {
            if (_path)
            {
                transform.position = _path.GetCurrentPoint();
                transform.rotation = Quaternion.LookRotation(_path.GetWaypointDirection());
            }
            
        }

        public Vector3 GetWaypointDirection() => _path.GetWaypointDirection();
        public Vector3 GetNextWaypoint() => _path.GetNextWaypoint();
        public bool HasARoute() => _path.Path;
        public bool ReachedWaypoint() => _path.ReachedWaypoint();
        public void ChangeWaypoint() => _path.ChangeWaypoint();

        public bool IsTargetInSight(Transform target) => CheckRange(target) && CheckAngle(target) && CheckView(target);
        public bool IsFollowing() => _isFollowing;
        public void SetFollowing(bool isFollowing) => _isFollowing = isFollowing;
        public bool TargetInRange(Transform target) => _range.GetBool(target, CurrentWeapon().Stats.Range);
        public bool IsTargetAlive(IModel target) => target != null && target.IsAlive();

        public void SetVisionConeColor(VisionConeEnum input)
        {
            if (_hasVisionCone) 
                visionCone.SetMaterial(input);
        }

        
        public override void Dispose()
        {
            base.Dispose();
            if (_fieldOfView != null)
                _fieldOfView.Dispose();
            _fieldOfView = null;
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
        }

        #if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            var transform1 = transform;
            var forward = transform1.forward;
            var position = transform1.position;
            
            #region FOV

            GetData<EnemySO>().FOV.DebugGizmos(transform, Color.red);

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
        public float Radius => radius;
    }
}