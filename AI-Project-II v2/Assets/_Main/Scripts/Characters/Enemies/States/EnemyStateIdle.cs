using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateIdle<T> : EnemyStateBase<T>
    {
        public override void Start()
        {
            base.Start();
            //var timer = Model.GetRandomTime(0.5f);
            //Model.SetTimer(timer);
            //Continue = false;
        }

        public override void Execute()
        {
            base.Execute();
            
            
            // if (Model.GetTimerComplete())
            // {
            //     Model.RunTimer();
            // }
            // else
            // {
            //     Continue = true;
            // }

            View.UpdateMovementValues(0);
        }

        public override void Exit()
        {
            base.Exit();
            Model.SetTimer(0);
        }
    }
}