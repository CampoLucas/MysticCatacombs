using UnityEngine;

namespace Game.WeaponSystem
{
    [System.Serializable]
    public class ProjectileData
    {
        public float Speed => speed;
        public float Damage => damage;
        public float LifeTime => lifeTime;
        
        [SerializeField] private float speed;
        [SerializeField] private float damage;
        [SerializeField] private float lifeTime = 10;
    }
}