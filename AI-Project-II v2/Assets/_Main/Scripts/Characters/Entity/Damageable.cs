using System;
using System.Collections;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities
{
    public class Damageable : MonoBehaviour, IDamageable
    {
        public float CurrentLife => _currentLife;
        public Action OnTakeDamage { get; set; }
        public Action OnDie { get; set; }
        private StatSO _data;
        private float _currentLife;
        private bool _isInvulnerable;
        private bool _hasTakenDamage;
        private Collider _collider;

        private void InitStats()
        {
            _currentLife = _data.MaxHealth;
        }
        private void Awake()
        {
            _data = GetComponent<EntityModel>().GetData();
            _collider = GetComponent<Collider>();
            InitStats();
        }

        // private void LateUpdate()
        // {
        //     _hasTakenDamage = false;
        // }

        public bool IsAlive() => _currentLife > 0;
        public bool IsInvulnerable() => _isInvulnerable;

        public bool HasTakenDamage()
        {
            if (_hasTakenDamage)
            {
                _hasTakenDamage = false;
                return true;
            }

            return false;
        }
        
        public void TakeDamage(float damage)
        {
            if (_isInvulnerable) return;
            _currentLife -= damage;

            if (!IsAlive())
            {
                OnDie?.Invoke();
                Die();
            }
            else
            {
                _hasTakenDamage = true;
                OnTakeDamage?.Invoke();
                TurnInvulnerable();
            }
        }

        private void TurnInvulnerable()
        {
            _isInvulnerable = true;
            StartCoroutine(InvulnerableCooldown());
        }

        private IEnumerator InvulnerableCooldown()
        {
            yield return new WaitForSeconds(_data.InvulnerableCooldown);
            _isInvulnerable = false;
        }
        
        public void Die()
        {
            _collider.enabled = false;
            Destroy(gameObject, 3f);
        }
        
        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            OnTakeDamage -= OnTakeDamage;
            OnDie -= OnDie;
            _data = null;
            _collider = null;
        }
    }
}