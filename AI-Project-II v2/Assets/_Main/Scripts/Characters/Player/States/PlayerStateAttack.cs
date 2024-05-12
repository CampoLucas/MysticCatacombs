using System;
using Game.SO;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateAttack<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;
        private AttackSO _initAttack;
        private AttackSO _currentAttack;
        private Func<bool> _attackFlag;

        private float _timer;
        private bool _triggered;

        public PlayerStateAttack(T inIdle, T inMoving, T inDamage, T inDead, AttackSO attack, Func<bool> attackFlag): base(inDamage, inDead)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
            _initAttack = attack;
            _attackFlag = attackFlag;
        }

        public override void Start()
        {
            base.Start();
            Attack(_initAttack);
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

                if (_currentAttack.TryGetTransition(out var transition) && _timer >= _currentAttack.TransitionTime && _attackFlag())
                {
                    Debug.Log("DoAttack");
                    Attack(transition);
                }
                
            }
            else
            {
                if (Controller.MoveDirection() != Vector3.zero)
                {
                    Controller.StateMachine.SetState(_inMoving);
                }
                else
                {
                    Controller.StateMachine.SetState(_inIdle);
                }
            }
        }

        private void Attack(AttackSO attack)
        {
            _currentAttack = attack;
            
            View.UpdateMovementValues(0);
            View.CrossFade(_currentAttack.Animation);
            _timer = 0;
            _triggered = false;
        }

        private bool TriggerEvent(float t)
        {
            return _currentAttack.TriggerTime.Evaluate(t) > 0;
        }
    }
}