using System;
using System.Collections;
using Game.DesignPatterns.Factory;
using Game.DesignPatterns.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.WeaponSystem.Modules
{
    public class RangedModule : WeaponModule, IFactory<Projectile, ProjectileData>
    {
        public Projectile Product => prefab;

        [Header("Module Settings")] 
        [SerializeField] private RangedModuleData moduleData;
        
        [Header("Projectile Settings")]
        [SerializeField] private Projectile prefab;
        [SerializeField] private ProjectileData projectileData;

        private Pool<Projectile> _pool;
        private bool _started;

        private void Awake()
        {
            _pool = new Pool<Projectile>(PoolCreate);
        }

        protected override void OnBegin()
        {
            base.OnBegin();
            if (!_started)
            {
                _started = true;
                Create(moduleData.FireAmount, moduleData.FireDelay);
            }
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            _started = false;
        }

        public Projectile Create()
        {
            var p = _pool.Get();
            p.Reset();
            p.gameObject.SetActive(false);
            return p;
        }

        public Projectile[] Create(int quantity, float delay = 0)
        {
            if (quantity < 0) quantity = 1;
            var projectiles = new Projectile[quantity];
            var spawnTime = 0f;
            for (var i = 0; i < quantity; i++)
            {
                projectiles[i] = Create();
                StartCoroutine(BeginProjectile(projectiles[i], spawnTime));
                spawnTime += delay;
            }

            return projectiles;
        }

        private IEnumerator BeginProjectile(Projectile projectile, float time)
        {
            yield return new WaitForSeconds(time);
            projectile.Begin();
            SetProjectilePosition(projectile);
        }

        private void SetProjectilePosition(Projectile projectile)
        {
            var t = projectile.transform;
            t.position = SpawnPoint();
            t.forward = MainWeapon.Owner.transform.forward;
        }

        private Projectile PoolCreate()
        {
            var t = Instantiate(prefab);
            t.Init(projectileData, _pool, MainWeapon.Owner);
            return t;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(SpawnPoint(), 0.05f);
        }

        private Vector3 SpawnPoint()
        {
            var tr = moduleData.FromOwner && MainWeapon != null && MainWeapon.Owner != null
                ? MainWeapon.Owner.transform
                : transform;

            var x = tr.right * moduleData.SpawnPoint.x;
            var y = tr.up * moduleData.SpawnPoint.y;
            var z = tr.forward * moduleData.SpawnPoint.z;

            return tr.position + x + y + z;
        }
    }
}