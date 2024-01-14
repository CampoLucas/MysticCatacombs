using System;
using _Main.Scripts.General;
using UnityEngine;

namespace Game.StateMachine.States
{
    public class MoveState : State
    {
        private GameObject _target;
        private float _speed;
        private Func<Vector3> _moveDir;
        private NullChecker<Func<Vector3>> _moveDirChecker;
        private Func<float> _delta;
        private NullChecker<Func<float>> _deltaChecker;

        private GameObject _cachedGameObject;
        private Transform _transform;

        public MoveState(float speed, Func<Vector3> moveDirection, Func<float> delta = null) : this(null, speed,
            moveDirection, delta = null)
        {
            
        }
        
        public MoveState(GameObject targetGameObject, float speed, Func<Vector3> moveDirection, Func<float> delta = null)
        {
            _target = targetGameObject;
            _speed = speed;
            _moveDir = moveDirection;
            _moveDirChecker.Set(_moveDir);
            _delta = delta;
            _deltaChecker.Set(_delta);
        }

        protected override void OnStart()
        {
            var currentGameObject = GetDefault(_target);
            if (currentGameObject != _cachedGameObject)
            {
                _cachedGameObject = currentGameObject;
                _transform = _cachedGameObject.transform;
            }
        }

        protected override void OnUpdate()
        {
            _transform.position += MoveDir() * (_speed * Delta());
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
        
        private float Delta() => _deltaChecker ? _delta() : Time.deltaTime;
        private Vector3 MoveDir() => _moveDirChecker ? _moveDir() : Vector3.zero;

        protected override void OnDestroy()
        {
            _target = null;
            _moveDir = null;
            _delta = null;

            _cachedGameObject = null;
            _transform = null;
        }
    }
}