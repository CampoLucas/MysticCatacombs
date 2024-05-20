using System;

namespace Game.Interfaces
{
    public interface IDamageable : IDisposable
    {
        public float CurrentHealth { get; }
        Action OnTakeDamage { get; set; }
        Action OnDie { get; set; }
        Action<float> OnHealthUpdated { get; set; }
        void TakeDamage(float damage);
        void Die();
        bool IsAlive();
        bool IsInvulnerable();
        bool HasTakenDamage();
        float MaxHealth();
    }
}