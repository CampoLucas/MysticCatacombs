using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Sheared;
using Game.SO;
using Game.WeaponSystem;
using Weapon = Game.Items.Weapons.Weapon;

namespace Game.Entities
{
    public class EntityModel : MonoBehaviour, IModel
    {
        public IDamageable Damageable { get; private set; }
        public Transform Transform { get; private set; }

        [SerializeField] private bool spawnable;
        [SerializeField] private StatSO stats;
        [SerializeField] private BaseWeapon weapon;

        protected CharacterController Controller { get; private set; }
        private IMovement _movement;
        private IRotation _rotate;
        private WaitTimer _waitTimer;

        protected virtual void Awake()
        {
            Transform = transform;
            Controller = GetComponent<CharacterController>();

            SetMovement(new Movement(Controller));
            SetRotation(new Rotation(transform, stats));
            Damageable = GetComponent<Damageable>();
            _waitTimer = new WaitTimer();
            
        }

        protected virtual void Start()
        {
            weapon.Equip(gameObject);
            Damageable.OnDie += OnDieHandler;
        }

        public virtual void Move(Vector3 dir) => _movement?.Move(dir);

        public virtual void Move(Vector3 dir, float speed) => _movement?.Move(dir, speed);
        public void Rotate(Vector3 dir) => _rotate?.Rotate(dir);
        public void Rotate(Vector3 dir, float speed) => _rotate?.Rotate(dir, speed);
        public StatSO GetData() => stats;
        public T GetData<T>() where T : StatSO => (T)stats;
        public Vector3 GetVelocity() => Controller.velocity;
        public Vector3 GetForward() => transform.forward;
        public bool IsAlive() => Damageable != null && Damageable.IsAlive();
        public bool IsInvulnerable() => Damageable != null && Damageable.IsInvulnerable();
        public bool HasTakenDamage() => Damageable != null && Damageable.HasTakenDamage();
        public BaseWeapon CurrentWeapon() => weapon;

        #region Timer Methods

        public bool GetTimerComplete() => _waitTimer?.TimerComplete() ?? default;
        public float GetRandomTime(float maxTime) => _waitTimer?.GetRandomTime(maxTime) ?? default;
        public void RunTimer() => _waitTimer?.RunTimer();
        public void SetTimer(float time) => _waitTimer?.SetTimer(time);

        #endregion

        #region Strategy
        
        public void SetMovement(IMovement movement, bool dispose = false)
        {
            if (_movement == movement) return;
            if (dispose && _movement != null)
                _movement.Dispose();
            _movement = movement;
        }

        public void SetRotation(IRotation rotation)
        {
            if (_rotate == rotation) return;
            if (_rotate != null)
                _rotate.Dispose();
            _rotate = rotation;
        }
        
        #endregion

        private void OnDestroy()
        {
            Dispose();
        }

        private void OnDieHandler()
        {
            Controller.enabled = false;
        }

        public virtual void Dispose()
        {
            if (Damageable != null)
            {
                Damageable.OnDie -= OnDieHandler;
                Damageable.Dispose();
                Damageable = null;
            }

            stats = null;
            weapon = null;
            Controller = null;

            if (_movement != null)
            {
                _movement.Dispose();
                _movement = null;
            }

            if (_rotate != null)
            {
                _rotate.Dispose();
                _rotate = null;
            }

            _waitTimer = null;
        }
    }

    public enum MovementType
    {
        Walk, Run
    }
}