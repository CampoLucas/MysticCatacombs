using System;
using Game.Entities;
using Game.Managers;

namespace Game.Player
{
    public class PlayerModel : EntityModel
    {
        private void Start()
        {
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
            Damageable.OnDie -= OnGameOverHandler;
            base.Dispose();
        }
    }
}