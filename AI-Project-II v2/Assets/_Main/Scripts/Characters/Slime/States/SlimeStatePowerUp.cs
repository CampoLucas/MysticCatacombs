﻿using UnityEngine;

namespace Game.Entities.Slime.States
{
    public sealed class SlimeStatePowerUp<T> : SlimeStateBase<T>
    {
        public override void Start()
        {
            base.Start();

            if (Model.HasReachedMaxLevel())
            {
                //Tree.Execute();
            }
            else
            {
                Model.IncreaseLevel();
                //Model.ClearJumpDelay();
                //Model.Jump(Vector3.zero, 3);
                Model.SetTimer(2f);
            }

        }

        public override void Execute()
        {
            base.Execute();
            
            if (Model.GetTimerComplete())
            {
                Model.RunTimer();
                //Model.IncreaseSize();
            }
            else
            {
                //Tree.Execute();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}