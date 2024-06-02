using Game.Interfaces;
using Game.StateMachine.Interfaces;
using UnityEngine;

namespace Game.StateMachine.Predicates
{
    public class IsAlivePredicate : DamageablePredicate
    {
        public IsAlivePredicate(IDamageable damageable) : base(damageable) { }
        public IsAlivePredicate(GameObject target, bool searchInChildren = false) : base(target, searchInChildren) { }
        public IsAlivePredicate(IModel model) : base(model) { }
        
        public override bool Evaluate()
        {
            return base.Evaluate() && Damageable.IsAlive();
        }
    }
}