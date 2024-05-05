using UnityEngine;
using UnityEngine.Serialization;

namespace Game.WeaponSystem.Modules
{
    [System.Serializable]
    public class RangedModuleData
    {
        public Vector3 SpawnPoint => spawnPoint;
        public int FireAmount => fireAmount;
        public float FireDelay => fireDelay;
        
        [SerializeField] private Vector3 spawnPoint;
        [SerializeField] private int fireAmount;
        [SerializeField] private float fireDelay;
    }
}