using Game.Enemies.States;
using Game.SO;
using UnityEngine;

namespace Game.Player.States
{
    public class EnemyStateAttack<T> : EnemyStateBase<T>
    {
        
        private Attack _initAttack;
        private Attack _currentAttack;
        private Attack _nextAttack;

        private float _timer;
        private bool _triggered;

        public EnemyStateAttack(AttackSO currentAttack)
        {
            _initAttack = currentAttack.GetAttack();
            _nextAttack = _initAttack;
        }

        public override void Start()
        {
            base.Start();
            Attack(_nextAttack);
            if (_currentAttack.TrModule.TryGetTransition(out var transition))
            {
                _nextAttack = transition;
            }
            else if (_currentAttack != _initAttack)
            {
                _nextAttack = _initAttack;
            }
            
            Continue = false;
            Model.Rotate((Controller.Target.Transform.position - Model.transform.position).normalized, 80);
        }

        public override void Execute()
        {
            base.Execute();
            _timer += Time.deltaTime;
            if (_timer < _currentAttack.AnimModule.Duration)
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
                
                _currentAttack.MoveModule.DoMove(Model, _timer);
                
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

            
        }

        private void Attack(Attack attack)
        {
            _currentAttack = attack;
            
            View.UpdateMovementValues(0);
            View.CrossFade(_currentAttack.AnimModule.Animation);
            _timer = 0;
            _triggered = false;
        }

        private bool TriggerEvent(float t)
        {
            return _currentAttack.EvModule.Triggered(t);
        }
    }
}