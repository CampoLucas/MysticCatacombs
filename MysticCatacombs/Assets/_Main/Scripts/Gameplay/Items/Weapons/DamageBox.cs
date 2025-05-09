using System.Collections.Generic;
using Game.CustomCollider;
using UnityEngine;
using Game.SO;
using Game.Sheared;
using Game.Entities;

namespace Game.Items.Weapons
{
    public class DamageBox : MonoBehaviour
    {
        [SerializeField] private bool destroyOnEnter;
        private Damageable _damageable;
        private Trigger _trigger;
        private WeaponSO _data;
        private Dictionary<GameObject, Damageable> _damageables = new();
        

        public void InitData(WeaponSO data)
        {
            _data = data;
        }

        private void Awake()
        {
            _trigger = GetComponent<BoxCastTrigger>();
            _damageable = GetComponentInParent<Damageable>();
        }

        private void Start()
        {
            if (_trigger)
                _trigger.OnEnter += CastEnter;
        }

        private void Damage(Damageable damageable)
        {
            damageable.TakeDamage(_data.Damage);
        }

        private void CastEnter(Collider other)
        {
            var otherGameObject = other.gameObject;
            if (_damageables.TryGetValue(otherGameObject, out var damageable))
            {
                Damage(damageable);
            }
            else
            {
                damageable = otherGameObject.GetComponent<Damageable>();
                if (damageable)
                {
                    _damageables[otherGameObject] = damageable;
                    Damage(damageable);
                }
            }
            if (destroyOnEnter) Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _trigger.OnEnter -= CastEnter;
            _trigger = null;
            _data = null;
            _damageables = null;
            
        }
    }
}
