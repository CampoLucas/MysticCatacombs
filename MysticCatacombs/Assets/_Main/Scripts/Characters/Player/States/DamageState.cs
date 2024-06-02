using UnityEngine;

namespace Game.Player.States
{
    public class DamageState : EntityState
    {
        protected override void OnStart()
        {
            base.OnStart();
            View.CrossFade(Model.GetData().HitAnimation.name);
            var timer = Model.GetData().HitAnimation.Duration;
            Model.SetTimer(timer);
            holdState = true;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Model.GetTimerComplete())
            {
                Model.RunTimer();
            }
            else
            {
                StateMachine.SetToDefault();
                holdState = false;
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

        protected override void OnExit()
        {
            base.OnExit();
            Model.SetTimer(0);
        }

        public override bool CanTransitionToItself() => true;
    }
}