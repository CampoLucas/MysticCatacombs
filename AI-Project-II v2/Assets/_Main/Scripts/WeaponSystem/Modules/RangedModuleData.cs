using UnityEngine;
using UnityEngine.Serialization;

namespace Game.WeaponSystem.Modules
{
    [System.Serializable]
    public class RangedModuleData
    {
        public bool FromOwner => fromOwner;
        public Vector3 SpawnPoint => spawnPoint;
        public int FireAmount => fireAmount;
        public float FireDelay => fireDelay;

        [SerializeField] private bool fromOwner;
        [SerializeField] private Vector3 spawnPoint;
        [SerializeField] private int fireAmount;
        [SerializeField] private float fireDelay;
    }
}