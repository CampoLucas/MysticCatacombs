using System;
using System.Collections;
using System.Collections.Generic;
using Game.Enemies;
using UnityEngine;

namespace Game.Managers
{
    public class EnemyManager : MonoManager<EnemyManager>
    {
        public int TotalEnemies { get; private set; }
        public int CurrentEnemies { get; private set; }
        private HashSet<EnemyController> _enemies = new();

        private void Start()
        {
            Subscribe(LevelManager.Instance);
        }

        public void AddEnemy(EnemyController enemy)
        {
            if (!_enemies.Add(enemy)) return;
            TotalEnemies++;
            CurrentEnemies++;
            OnAddEnemy();
        }

        public void RemoveEnemy(EnemyController enemy)
        {
            if (!_enemies.Remove(enemy)) return;
            CurrentEnemies--;
            OnRemoveEnemy();
        }

        private void OnAddEnemy()
        {
            NotifyAll(LevelManager.EvaluateCondition);
        }
        
        private void OnRemoveEnemy()
        {
            NotifyAll(LevelManager.EvaluateCondition);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            _enemies.Clear();
            _enemies = null;
        }
    }
}
