using System;
using Game.Entities;
using Game.Managers;
using UnityEngine;

namespace Game.Player
{
    public class PlayerModel : EntityModel
    {
        protected override void Start()
        {
            base.Start();
            Damageable.OnDie += OnGameOverHandler;
        }

        private void OnGameOverHandler()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }

        public override void Dispose()
        {
            if (Damageable != null)
                Damageable.OnDie -= OnGameOverHandler;
            base.Dispose();
        }
    }
}