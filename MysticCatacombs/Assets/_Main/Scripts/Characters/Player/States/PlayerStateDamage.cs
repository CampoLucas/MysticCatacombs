using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateDamage<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;

        public PlayerStateDamage(in T inIdle, in T inMoving, in T inDamage, in T inDead): base(inDamage, inDead)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
        }

        public override void Start()
        {
            base.Start();
            View.CrossFade(Model.GetData().HitAnimation.name);
            var timer = Model.GetData().HitAnimation.Duration;
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
        }
    }
}