using System;
using System.Collections.Generic;
using Game.Interfaces;
using Game.SO;
using Game.WeaponSystem.Modules;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.WeaponSystem
{
    public class BaseWeapon : Weapon
    {
        public WeaponSO Stats => stats;
        public GameObject Owner { get; private set; }
        public IView View { get; private set; }
        
        [SerializeField] private WeaponSO stats;
        [SerializeField] private List<WeaponModule> modules = new();

        private void Awake()
        {
            InitModules();
        }

        public void Equip(GameObject owner)
        {
            Owner = owner;
            if (Owner)
            {
                View = Owner.GetComponent<IView>();
                View.SetFloat("WeaponIndex", stats.WeaponIndex);
            }

            for (var i = 0; i < modules.Count; i++)
            {
                modules[i].SetOwner();
            }
        }

        public void InitModules()
        {
            for (var i = 0; i < modules.Count; i++)
            {
                modules[i].Init(this);
            }
        }

        protected override void OnBegin()
        {
            for (var i = 0; i < modules.Count; i++)
            {
                modules[i].Begin();
            }
        }

        protected override void OnEnd()
        {
            for (var i = 0; i < modules.Count; i++)
            {
                modules[i].End();
            }
        }

        protected override void OnDispose()
        {
            modules = null;
            Owner = null;
            View = null;
            stats = null;
        }
    }
}