using System;
using Project.Utils.Mask;
using Project.Utils.Math;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Locomotion
{
    [RequireComponent(typeof(Rigidbody))]
    public class LocomotionController : MonoBehaviour
    {
        [SerializeField] private float suspensionDistance = 1;
        [SerializeField] private float offset = 1;
        [SerializeField] private LayerMask ignoreLayer;


        [SerializeField] private Vector3 downDir = -Vector3.up;
        [SerializeField] private float springStrength;
        [SerializeField] private float springDamper;


        private Rigidbody _rigidbody;
        private int _groundLayer;
        private RaycastHit _rayHit;
        private bool _rayDidHit;
        [SerializeField] private Quaternion _uprightJointTargetRot;
        [SerializeField] private float _uprightJointSpringStrength;
        [SerializeField] private float _uprightJointSpringDamper;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _groundLayer = UtilsMask.Invert(ignoreLayer);
        }


        private void Update()
        {
            _rayDidHit = Physics.Raycast(RayOrigin(), Vector3.down, out _rayHit, suspensionDistance + 0.1f,
                _groundLayer);


            if (_rayDidHit)
            {
                var velocity = _rigidbody.velocity;
                var rayDirection = transform.TransformDirection(downDir);


                var otherVelocity = Vector3.zero;
                var hitRigidbody = _rayHit.rigidbody;
                if (hitRigidbody != null) otherVelocity = hitRigidbody.velocity;


                var rayDirectionVelocity = Vector3.Dot(rayDirection, velocity);
                var otherDirectionVelocity = Vector3.Dot(rayDirection, otherVelocity);


                var relativeVelocity = rayDirectionVelocity - otherDirectionVelocity;


                var x = _rayHit.distance - suspensionDistance;


                var springForce = x * springStrength - relativeVelocity * springDamper;

                rayDirection.x = 0;
                rayDirection.z = 0;
                _rigidbody.AddForce(rayDirection * springForce);


                if (hitRigidbody != null) hitRigidbody.AddForceAtPosition(rayDirection * -springForce, _rayHit.point);
            }
            
            UpdateUprightForce(0);
        }

        public void Move(Vector3 dir)
        {
            _rigidbody.velocity += dir * Time.deltaTime;
        }

        private void UpdateUprightForce(float elapsed)
        {
            var current = transform.rotation;
            var target = UtilsMath.ShortestRotation(_uprightJointTargetRot, current);

            var rotAxis = Vector3.zero;
            var rotDegrees = 0f;
            
            target.ToAngleAxis(out rotDegrees, out rotAxis);
            rotAxis.Normalize();

            var rotRadians = rotDegrees * Mathf.Deg2Rad;
            
            _rigidbody.AddTorque((rotAxis * (rotRadians * _uprightJointSpringStrength)) - (_rigidbody.angularVelocity * _uprightJointSpringDamper));
        }

        private Vector3 RayOrigin()
        {
            var pos = transform.position;
            pos.y += offset;
            return pos;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(RayOrigin(), Vector3.down * (suspensionDistance + 0.1f));
        }
    }
}