﻿using Game.Scripts.VisionCone;
using Game.Interfaces;
using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateSeek<T> : EnemyStatePursuit<T>
    {
        public EnemyStateSeek(ISteering steering, ISteering obsAvoidance) : base(steering, obsAvoidance) {}

        public override void Start()
        {
            base.Start();
            Model.SetTimer(Random.Range(8f, 16f));
            //CalculatePath();
            Model.SetTarget(Controller.Target.Transform);
            Model.SetVisionConeColor(VisionConeEnum.InSight);
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
                Model.SetFollowing(false);
            }
        }

        private void CalculatePath()
        {
            var pos = Model.transform.position;
            var targetPos = Controller.Target.Transform.position;

            if (Model.SetNodes(pos, targetPos))
                Model.CalculatePath();
        }

        protected override void Follow()
        {
            // if (!Model.IsTargetInRange())
            // {
            //     Logging.LogPathfinder($"Is target in range: {Model.IsTargetInRange()}");
            //     CalculatePath();
            // }
            // Model.FollowTarget(Model.GetPathfinder(), Steering, ObsAvoidance);
            var dir = Model.GetWaypoint() - Model.Transform.position;
            
            
            Model.Move(dir.normalized);
        }
    }
}