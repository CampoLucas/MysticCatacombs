using System;
using System.Collections.Generic;
using Game.Entities;
using UnityEngine;

namespace Game.WeaponSystem
{
    public class Damager : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private GameObject owner;
        [SerializeField] private bool ignoreOwner;
        
        private Dictionary<GameObject, Damageable> _damageables = new();
        private HashSet<GameObject> _damaged = new();

        public void SetOwner(GameObject newOwner)
        {
            owner = newOwner;
        }
        
        public void SetDamage(float newDamage)
        {
            damage = newDamage;
        }

        public void Reset()
        {
            _damaged.Clear();
        }

        public void OnEnter(Collider other)
        {
            var obj = other.gameObject;

            if ((ignoreOwner && obj == owner) || _damaged.Contains(obj))
            {
                return;
            }
            _damaged.Add(obj);

            var damageble = TryGetDamageable(obj);
            if (damageble != null)
            {
                damageble.TakeDamage(damage);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("The object that the damager is trying to damage is null", owner);
#endif
            }
        }

        private Damageable TryGetDamageable(GameObject target)
        {
            if (!_damageables.TryGetValue(target, out var damageable))
            {
                damageable = target.GetComponent<Damageable>();
                if (!damageable)
                {
                    damageable = target.GetComponentInChildren<Damageable>();
                }
                
                _damageables[target] = damageable;
            }

            return damageable;
        }

        private void OnDestroy()
        {
            owner = null;
            _damageables.Clear();
            _damageables = null;
            _damaged.Clear();
            _damaged = null;
        }
    }
}