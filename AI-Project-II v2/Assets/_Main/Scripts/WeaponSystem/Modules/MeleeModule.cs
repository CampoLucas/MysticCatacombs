using System;
using System.Collections.Generic;
using System.Linq;
using Game.CustomCollider;
using Game.Items.Weapons;
using UnityEngine;

namespace Game.WeaponSystem.Modules
{
    [RequireComponent(typeof(Damager))]
    public class MeleeModule : WeaponModule
    {
        [SerializeField] private Trigger[] triggers;
        private Damager _damageBox;

        private void Awake()
        {
            _damageBox = GetComponent<Damager>();
        }

        private void Start()
        {
            SubscribeDamager();
        }

        protected override void OnSetOwner()
        {
            base.OnSetOwner();
            if (!_damageBox)
            {
                _damageBox = GetComponent<Damager>();
                SubscribeDamager();
            }
            _damageBox.SetOwner(MainWeapon.Owner);
        }

        protected override void OnBegin()
        {
            base.OnBegin();
            _damageBox.SetDamage(MainWeapon.Stats.Damage);
            EnableTriggers();
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            DisableTriggers();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            UnsubscribeDamager();
            triggers = null;
            _damageBox = null;
        }

        private void SubscribeDamager()
        {
            for (var i = 0; i < triggers.Length; i++)
            {
                if (_damageBox != null && triggers[i] != null && triggers[i].OnEnter != null)
                {
                    triggers[i].OnEnter += _damageBox.OnEnter;
                }
                else
                {
#if UNITY_EDITOR
                    if (_damageBox == null) Debug.LogWarning("Damage box is null.");
                    if (triggers[i] == null) Debug.LogWarning("The trigger is null.");
                    if (triggers[i].OnEnter == null) Debug.LogWarning("The OnEnter is null");
                    Debug.LogWarning("One of the triggers or _damageBox is null, skipping subscription.");              
#endif
                }
            }
        }

        private void UnsubscribeDamager()
        {
            for (var i = 0; i < triggers.Length; i++)
            {
                if (i > triggers.Length - 1)
                {
                    Debug.Log("Is bigger");
                    break;
                }
                triggers[i].OnEnter -= _damageBox.OnEnter;
            }
        }

        private void EnableTriggers()
        {
            _damageBox.Reset();
            for (var i = 0; i < triggers.Length; i++)
            {
                triggers[i].EnableCollider();
            }
        }
        
        private void DisableTriggers()
        {
            for (var i = 0; i < triggers.Length; i++)
            {
                triggers[i].DisableCollider();
            }
        }
    }
}