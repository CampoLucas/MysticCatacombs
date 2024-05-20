using System;
using UnityEngine;
using Game.Interfaces;
using Game.SO;

namespace Game.Entities
{
    public class Rotation : IRotation
    {
        private StatSO _data;
        private Transform _transform;

        public Rotation(Transform transform, StatSO data)
        {
            _data = data;
            _transform = transform;
        }

        private Quaternion _targetRotation = Quaternion.identity;
        private Vector3 _lastTargetDir;

        public void Rotate(Vector3 dir)
        {
            Rotate(dir, _data.RotSpeed);
        }

        public void Rotate(Vector3 dir, float speed, float delta = -1)
        {
            if (delta < 0) delta = Time.deltaTime;
            
            _lastTargetDir = dir.normalized;

            if (_lastTargetDir == Vector3.zero)
            {
                _lastTargetDir = _transform.forward;
            }

            var tr = Quaternion.LookRotation(_lastTargetDir); // tr == target rotation
            _targetRotation = Quaternion.Slerp(_transform.rotation, tr, speed * delta);

            _transform.rotation = _targetRotation;
        }

        public void Dispose()
        {
            _data = null;
            _transform = null;
        }
    }
}