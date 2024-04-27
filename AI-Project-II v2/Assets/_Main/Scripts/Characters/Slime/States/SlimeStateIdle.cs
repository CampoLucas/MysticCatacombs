using UnityEngine;

namespace Game.Entities.Slime.States
{
    public sealed class SlimeStateIdle<T> : SlimeStateBase<T>
    {
        public override void Start()
        {
            base.Start();
            Model.SetTimer(1.5f);
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
                //Tree.Execute();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Model.SetTimer(0);
        }
    }
}