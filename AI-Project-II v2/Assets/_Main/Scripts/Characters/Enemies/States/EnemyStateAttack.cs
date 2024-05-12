using Game.Enemies.States;
using Game.SO;
using UnityEngine;

namespace Game.Player.States
{
    public class EnemyStateAttack<T> : EnemyStateBase<T>
    {
        
        private AttackSO _initAttack;
        private AttackSO _currentAttack;

        private float _timer;
        private bool _triggered;

        public EnemyStateAttack(AttackSO currentAttack)
        {
            _initAttack = currentAttack;
            _currentAttack = _initAttack;
        }

        public override void Start()
        {
            base.Start();
            View.UpdateMovementValues(0);
            View.CrossFade(_currentAttack.Animation);
            _timer = 0;
            _triggered = false;
            Continue = false;
        }

        public override void Execute()
        {
            base.Execute();
            _timer += Time.deltaTime;
            if (_timer < _currentAttack.Duration)
            {

                if (!_triggered && TriggerEvent(_timer))
                {
                    _triggered = true;
                    Model.CurrentWeapon().Begin();
                }

                if (_triggered && !TriggerEvent(_timer))
                {
                    _triggered = false;
                    Model.CurrentWeapon().End();
                }
                
                _currentAttack.AttackMove(Model, _timer);
                
            }
            else
            {
                Continue = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            Continue = true;

            if (_currentAttack.TryGetTransition(out var transition))
            {
                _currentAttack = transition;
            }
            else if (_currentAttack != _initAttack)
            {
                _currentAttack = _initAttack;
            }
        }

        private bool TriggerEvent(float t)
        {
            return _currentAttack.TriggerTime.Evaluate(t) > 0;
        }
    }
}