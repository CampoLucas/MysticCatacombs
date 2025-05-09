﻿using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateDamage<T> : EnemyStateBase<T>
    {
        public override void Start()
        {
            base.Start();
            View.CrossFade(Model.GetData().HitAnimation.EventHash);
            var timer = Model.GetData().HitAnimation.Duration;
            Model.SetTimer(timer);
            Model.SetFollowing(true);
            Continue = false;
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
                Continue = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            Model.SetTimer(0);
        }
    }
}