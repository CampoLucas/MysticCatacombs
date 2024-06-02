using Game.Interfaces;
using Game.StateMachine.Interfaces;
using UnityEngine;

namespace Game.StateMachine.Predicates
{
    public class TakenDamagePredicate : DamageablePredicate
    {
        public TakenDamagePredicate(IDamageable damageable) : base(damageable) { }
        public TakenDamagePredicate(GameObject target, bool searchInChildren = false) : base(target, searchInChildren) { }
        public TakenDamagePredicate(IModel model) : base(model) { }
        
        public override bool Evaluate()
        {
            return base.Evaluate() && Damageable.HasTakenDamage();
        }
    }
}