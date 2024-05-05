using Game.Entities;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateHeavyAttackOne<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;

        public PlayerStateHeavyAttackOne(T inIdle, T inMoving, T inDamage, T inDead): base(inDamage, inDead)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
        }

        public override void Start()
        {
            base.Start();
            Model.HeavyAttack();
            View.CrossFade(Model.CurrentWeapon().Stats.HeavyAttack01.EventHash);
            var timer = Model.CurrentWeapon().Stats.HeavyAttack01.Duration;
            Model.SetTimer(timer);
        }

        public override void Execute()
        {
            base.Execute();

            if (Model.GetTimerComplete())
            {
                Model.RunTimer();
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

        public override void Exit()
        {
            base.Exit();
            Model.SetTimer(0);
            Model.CancelHeavyAttack();
        }
    }
}