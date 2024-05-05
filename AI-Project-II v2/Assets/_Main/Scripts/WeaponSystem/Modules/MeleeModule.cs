using System;
using System.Collections.Generic;
using System.Linq;
using Game.CustomCollider;
using Game.Items.Weapons;
using UnityEngine;

namespace Game.WeaponSystem.Modules
{
    public class MeleeModule : WeaponModule
    {
        [SerializeField] private List<Trigger> triggers;
        private Damager _damageBox;

        private void Awake()
        {
            _damageBox = GetComponentInChildren<Damager>();
            
        }

        private void Start()
        {
            _damageBox.SetOwner(MainWeapon.Owner);
            SubscribeDamager();
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
            triggers.Clear();
            triggers = null;
            _damageBox = null;
        }

        private void SubscribeDamager()
        {
            Debug.Log(triggers.Count);
            for (var i = 0; i < triggers.Count; i++)
            {
                if (i > triggers.Count - 1)
                {
                    Debug.Log("Is bigger");
                    break;
                }
                triggers[i].OnEnter += _damageBox.OnEnter;
                Debug.Log("Subscribed");
            }
        }

        private void UnsubscribeDamager()
        {
            for (var i = 0; i < triggers.Count; i++)
            {
                if (i > triggers.Count - 1)
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
            for (var i = 0; i < triggers.Count; i++)
            {
                triggers[i].EnableCollider();
            }
        }
        
        private void DisableTriggers()
        {
            for (var i = 0; i < triggers.Count(); i++)
            {
                triggers[i].DisableCollider();
            }
        }
    }
}