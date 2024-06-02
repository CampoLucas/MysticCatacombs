using Game.Interfaces;
using Game.StateMachine.Interfaces;
using UnityEngine;

namespace Game.StateMachine.Predicates
{
    public class IsDeathPredicate : DamageablePredicate
    {
        public IsDeathPredicate(IDamageable damageable) : base(damageable) { }
        public IsDeathPredicate(GameObject target, bool searchInChildren = false) : base(target, searchInChildren) { }
        public IsDeathPredicate(IModel model) : base(model) { }
        
        public override bool Evaluate()
        {
            return base.Evaluate() && !Damageable.IsAlive();
        }
    }
}