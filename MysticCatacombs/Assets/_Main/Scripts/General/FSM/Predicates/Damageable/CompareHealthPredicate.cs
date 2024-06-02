using Game.Interfaces;
using UnityEngine;

namespace Game.StateMachine.Predicates
{
    public class CompareHealthPredicate : DamageablePredicate
    {
        private readonly float _amountToCheck;
        private readonly bool _usePercentage;
        private readonly CompareType _compareType;
        private float _catchMaxHealth;
        private float _catchCurrentHealth;
        private float _catchCurrentToCompare;
        private bool _result;

        public CompareHealthPredicate(float amount, bool usePercentage, CompareType compareType, IDamageable damageable)
            : base(damageable)
        {
            _amountToCheck = amount;
            _usePercentage = usePercentage;
            _compareType = compareType;
        }

        public CompareHealthPredicate(float amount, bool usePercentage, CompareType compareType, GameObject target,
            bool searchInChildren = false) : base(target, searchInChildren)
        {
            _amountToCheck = amount;
            _usePercentage = usePercentage;
            _compareType = compareType;
        }

        public CompareHealthPredicate(float amount, bool usePercentage, CompareType compareType, IModel model) :
            base(model)
        {
            _amountToCheck = amount;
            _usePercentage = usePercentage;
            _compareType = compareType;
        }
        
        public override bool Evaluate()
        {
            return base.Evaluate() && CompareHealth();
        }

        private bool CompareHealth()
        {
            var maxHealth = Damageable.MaxHealth();
            var currentHealth = Damageable.CurrentHealth;

            if (maxHealth != _catchMaxHealth || currentHealth != _catchCurrentHealth)
            {
                _catchCurrentHealth = currentHealth;
                _catchMaxHealth = maxHealth;
                
                _catchCurrentToCompare = _usePercentage ? (currentHealth / maxHealth * 100f) : currentHealth;
                
                switch (_compareType)
                {
                    case CompareType.LowerThan:
                        _result = _catchCurrentToCompare < _amountToCheck;
                        break;
                    case CompareType.LowerOrEqualThan:
                        _result = _catchCurrentToCompare <= _amountToCheck;
                        break;
                    case CompareType.GreaterThan:
                        _result = _catchCurrentToCompare > _amountToCheck;
                        break;
                    case CompareType.GreaterOrEqualThan:
                        _result = _catchCurrentToCompare >= _amountToCheck;
                        break;
                    case CompareType.EqualTo:
                        _result = Mathf.Approximately(_catchCurrentToCompare, _amountToCheck);
                        break;
                    default:
                        _result = false;
                        break;
                }
            }

            return _result;
        }
    }

    public enum CompareType
    {
        LowerThan,
        LowerOrEqualThan,
        GreaterThan,
        GreaterOrEqualThan,
        EqualTo,
    }
}