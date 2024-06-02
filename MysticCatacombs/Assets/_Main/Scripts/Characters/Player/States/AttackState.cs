using System;
using Game.SO;
using UnityEngine;

namespace Game.Player.States
{
    public class AttackState : EntityState
    {
        private Attack _initAttack;
        private Attack _currentAttack;
        private Func<bool> _attackFlag;

        private float _timer;
        private bool _triggered;

        public AttackState(AttackSO attack, Func<bool> attackFlag)
        {
            _initAttack = attack.GetAttack();
            _attackFlag = attackFlag;
        }

        protected override void OnStart()
        {
            base.OnStart();
            Attack(_initAttack);
            holdState = true;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
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

                if (_timer >= _currentAttack.TrModule.TimeToTransition && _currentAttack.TrModule.TryGetTransition(out var transition) && _attackFlag())
                {
                    Attack(transition);
                }
                
            }
            else
            {
                holdState = false;
                StateMachine.SetToDefault();
                // if (Controller.MoveDirection() != Vector3.zero)
                // {
                //     Controller.StateMachine.SetState(_inMoving);
                // }
                // else
                // {
                //     Controller.StateMachine.SetState(_inIdle);
                // }
            }
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