using System;
using System.Collections;
using Game.CustomCollider;
using Game.DesignPatterns.Factory;
using Game.DesignPatterns.Pool;
using UnityEngine;

namespace Game.WeaponSystem
{
    public class Projectile : MonoBehaviour, IProduct<ProjectileData>
    {
        public ProjectileData Data { get; private set; }

        private Trigger _damageTrigger;
        private Transform _transform;
        private Damager _damager;
        private Coroutine _coroutine;
        private Pool<Projectile> _pool;
        private bool _begin;

        private void Awake()
        {
            _transform = transform;
            _damager = GetComponent<Damager>();
            _damageTrigger = GetComponent<Trigger>();
            _damageTrigger.OnEnter += OnDamageHandler;
        }

        private void OnEnable()
        {
            _begin = false;
        }

        public void Init(ProjectileData data, Pool<Projectile> pool, GameObject owner)
        {
            Data = data;
            _pool = pool;
            _damager.SetOwner(owner);
            _damager.SetDamage(data.Damage);
            
        }

        public void Begin()
        {
            gameObject.SetActive(true);
            _begin = true;
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(DisableAfterTime(Data.LifeTime));
            _damageTrigger.EnableCollider();
        }

        public void Reset()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }

        private void Update()
        {
            if (!_begin) return;
            _transform.position += _transform.forward * (Data.Speed * Time.deltaTime);
        }

        private void OnDamageHandler(Collider other)
        {
            if (other.gameObject.layer == 3 || other.gameObject.layer == 8)
            {
                PoolDestroy();
                return;
            }
            
            _damager.OnEnter(other);
            PoolDestroy();
        }

        private IEnumerator DisableAfterTime(float t)
        {
            yield return new WaitForSeconds(t);
            PoolDestroy();   
        }

        private void PoolDestroy()
        {
            _pool.Recicle(this);
            _damager.Reset();
            _damageTrigger.DisableCollider();
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }

        private void OnDestroy()
        {
            if (_damageTrigger)
            {
                _damageTrigger.OnEnter -= OnDamageHandler;
                _damageTrigger = null;
            }

            Data = null;
            _transform = null;
            _damager = null;
            _coroutine = null;

            if (_pool != null)
            {
                _pool.Dispose();
                _pool = null;
            }
        }
    }
}