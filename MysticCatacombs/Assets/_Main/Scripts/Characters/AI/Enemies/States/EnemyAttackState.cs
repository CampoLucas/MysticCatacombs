using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Game.Enemies;
using Game.SO;
using UnityEngine;

namespace Game.Player.States
{
    public class EnemyAttackState : EntityState
    {
        private Attack _initAttack;
        private Attack _currentAttack;
        private Attack _nextAttack;

        private float _timer;
        private bool _triggered;

        public EnemyAttackState(AttackSO currentAttack)
        {
            _initAttack = currentAttack.GetAttack();
            _nextAttack = _initAttack;
        }

        protected override void OnStart()
        {
            base.OnStart();
            Attack(_nextAttack);
            if (_currentAttack.TrModule.TryGetTransition(out var transition))
            {
                _nextAttack = transition;
            }
            else if (_currentAttack != _initAttack)
            {
                _nextAttack = _initAttack;
            }

            holdState = true;

            var controller = Controller as EnemyController;
            if (controller == null)
            {
                Debug.LogError("Controller is null");
                return;
            }else if (controller.Target == null)
            {
                Debug.LogError("Target is null");
                return;
            }
            
            Model.Rotate((controller.Target.Transform.position - controller.transform.position).normalized, 80);
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
                
            }
            else
            {
                holdState = false;
            }
        }

        protected override void OnExit()
        {
            base.OnExit();
            holdState = false;
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