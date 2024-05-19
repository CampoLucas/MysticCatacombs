using UnityEngine;

namespace Game.Managers.Level
{
    [CreateAssetMenu(fileName = "KillEnemies", menuName = "Game/Level/Objectives/KillEnemies")]
    public class KillEnemiesCondition : LevelCondition
    {
        [Header("Settings")]
        [SerializeField] private KillAmount condition;
        [SerializeField] private float enemiesToKill;

        public override bool Evaluate()
        {
            return SatisfiedCondition();
        }

        private bool SatisfiedCondition()
        {
            var enemyManager = EnemyManager.Instance;

            if (!enemyManager) return false;
            
            var current = enemyManager.CurrentEnemies;
            var value = false;
            switch (condition)
            {
                case KillAmount.All:
                    value = current == 0;
                    break;
                case KillAmount.Amount:
                    value = current <= enemiesToKill;
                    break;
                case KillAmount.Percentage:
                    value = ((float)current / enemyManager.TotalEnemies) * 100 <= enemiesToKill;
                    break;
            }

            return value;
        }
    }

    public enum KillAmount
    {
        All,
        Amount,
        Percentage,
    }
}