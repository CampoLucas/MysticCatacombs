using Game.Interfaces;
using Game.StateMachine.Interfaces;
using UnityEngine;

namespace Game.StateMachine.Predicates
{
    public class DamageablePredicate : IPredicate
    {
        protected IDamageable Damageable;
        protected readonly bool HasDamageable;

        public DamageablePredicate(IDamageable damageable)
        {
            if (damageable == null)
            {
#if UNITY_EDITOR
                Debug.LogError("Error: damageable is null");
#endif
                HasDamageable = false;
                return;
            }
            
            Damageable = damageable;
            HasDamageable = true;
        }

        public DamageablePredicate(GameObject target, bool searchInChildren = false)
        {
            if (!target)
            {
#if UNITY_EDITOR
                Debug.LogError("Error: target GameObject is null");
#endif
                HasDamageable = false;
                return;
            }
            
            Damageable = target.GetComponent<IDamageable>();

            if (searchInChildren && Damageable == null)
            {
                Damageable = target.GetComponentInChildren<IDamageable>();
            }

            if (Damageable == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Error: couldn't find damageable");
#endif
                HasDamageable = false;
                return;
            }

            HasDamageable = true;
        }

        public DamageablePredicate(IModel model)
        {
            if (model == null)
            {
#if UNITY_EDITOR
                Debug.LogError("Error: model is null");
#endif
                HasDamageable = false;
                return;
            }
            
            Damageable = model.Damageable;
            HasDamageable = true;
        }
        
        public virtual bool Evaluate()
        {
            if (!HasDamageable)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Warning: using a damageable predicate without a damageable.");
#endif
            }
            return HasDamageable;
        }
        
        public virtual void Dispose()
        {
            Damageable = null;
        }
    }
}