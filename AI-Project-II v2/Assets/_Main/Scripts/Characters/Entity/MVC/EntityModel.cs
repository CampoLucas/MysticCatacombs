﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;
using Game.Sheared;
using Game.SO;

namespace Game.Entities
{
    public class EntityModel : MonoBehaviour, IModel
    {
        public IDamageable Damageable { get; private set; }
        public Transform Transform { get; private set; }

        [SerializeField] private bool spawnable;
        [SerializeField] private StatSO stats;
        [SerializeField] private Weapon weapon;

        protected Rigidbody Rigidbody { get; private set; }
        private IMovement _walkMovement;
        private IMovement _runMovement;
        private IMovement _movement;
        private IRotation _rotate;
        private IAttack _lightAttack;
        private IAttack _heavyAttack;
        private WaitTimer _waitTimer;
        private Levelable _level;

        protected virtual void Awake()
        {
            Transform = transform;
            Rigidbody = GetComponent<Rigidbody>();

            _runMovement = new Movement(stats.MoveSpeed, stats.MoveLerpSpeed, Rigidbody);
            _walkMovement = new Movement(stats.WalkSpeed, stats.MoveLerpSpeed, Rigidbody);
            SetMovement(_runMovement);
            SetRotation(new Rotation(transform, stats));
            _lightAttack = GetComponent<LightAttack>();
            _heavyAttack = GetComponent<HeavyAttack>();
            Damageable = GetComponent<Damageable>();
            _waitTimer = new WaitTimer();
            _level = new Levelable(stats.Level, stats.MaxLevel);
        }

        public virtual void Move(Vector3 dir) => _movement?.Move(dir);

        public virtual void Move(Vector3 dir, float speed)
        {
            
        }
        public void Rotate(Vector3 dir) => _rotate?.Rotate(dir);
        public StatSO GetData() => stats;
        public T GetData<T>() where T : StatSO => (T)stats;
        public Vector3 GetVelocity() => Rigidbody.velocity;
        public Vector3 GetForward() => transform.forward;
        public bool IsAlive() => Damageable.IsAlive();
        public bool IsInvulnerable() => Damageable.IsInvulnerable();
        public bool HasTakenDamage() => Damageable.HasTakenDamage();
        public Weapon CurrentWeapon() => weapon;
        public void LightAttack() => _lightAttack.Attack();
        public void CancelLightAttack() => _lightAttack.CancelAttack();
        public void HeavyAttack() => _heavyAttack.Attack();
        public void CancelHeavyAttack() => _heavyAttack.CancelAttack();
        public void IncreaseLevel() => _level.IncreaseLevel();
        public int GetCurrentLevel() => _level.CurrentLevel;

        public bool HasReachedMaxLevel() => _level.HasReachedMaxLevel();
        public Levelable GetLevelable() => _level;

        #region Timer Methods

        public bool GetTimerComplete() => _waitTimer?.TimerComplete() ?? default;
        public float GetRandomTime(float maxTime) => _waitTimer?.GetRandomTime(maxTime) ?? default;
        public void RunTimer() => _waitTimer?.RunTimer();
        public void SetTimer(float time) => _waitTimer?.SetTimer(time);

        #endregion

        #region Strategy
        
        public IMovement GetWalkingMovement()
        {
            if (_walkMovement == null) return default;
            return _walkMovement;
        }

        public IMovement GetRunningMovement()
        {
            if (_runMovement == null) return default;
            return _runMovement;
        }

        public void SetMovement(IMovement movement)
        {
            if (_movement == movement) return;
            if (_movement != null && !(movement == _walkMovement || movement == _runMovement))
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

        public void SetLightAttack(IAttack attack)
        {
            _lightAttack = attack;
        }

        public void SetHeavyAttack(IAttack attack)
        {
            _heavyAttack = attack;
        }
        
        #endregion

        private void OnDestroy()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            if (Damageable != null)
            {
                Damageable.Dispose();
                Damageable = null;
            }

            stats = null;
            weapon = null;
            Rigidbody = null;

            if (_walkMovement != null)
            {
                _walkMovement.Dispose();
                _walkMovement = null;
            }

            if (_runMovement != null)
            {
                _runMovement.Dispose();
                _runMovement = null;
            }

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

            if (_lightAttack != null)
            {
                _lightAttack.Dispose();
                _lightAttack = null;
            }

            if (_heavyAttack != null)
            {
                _heavyAttack.Dispose();
                _heavyAttack = null;
            }

            _waitTimer = null;
            _level = null;
        }
    }

    public enum MovementType
    {
        Walk, Run
    }
}