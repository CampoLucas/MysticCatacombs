using System;
using System.Collections.Generic;
using Game.SO;
using Game.WeaponSystem.Modules;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.WeaponSystem
{
    public class BaseWeapon : Weapon
    {
        public WeaponSO Stats => stats;
        public GameObject Owner => owner;

        [SerializeField] private GameObject owner;
        [SerializeField] private WeaponSO stats;
        [SerializeField] private List<WeaponModule> modules = new();

        private void Awake()
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
            owner = null;
            stats = null;
        }
    }
}