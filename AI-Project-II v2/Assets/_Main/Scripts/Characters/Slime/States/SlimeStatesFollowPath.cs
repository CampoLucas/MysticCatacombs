using UnityEngine;

namespace Game.Entities.Slime.States
{
    public sealed class SlimeStatesFollowRoute<T> : SlimeStateBase<T>
    {
        public override void Start()
        {
            base.Start();
            
            //Model.GetRandomNode();
            //CalculatePath();

            var timer = 10f;
            Model.SetTimer(timer);
        }

        public override void Execute()
        {
            //Model.RunJumpDelay();

            if (!Model.GetTimerComplete())
            {
                //Tree.Execute();
            }
            
            // if (Model.ReachedWaypoint())
            // {
            //     Model.ChangeWaypoint();
            // }

            Follow();
            Model.RunTimer();
        }

        public override void Exit()
        {
            base.Exit();
            Model.Move(Vector3.zero);
        }

        private void CalculatePath()
        {
            // var pos = Model.transform.position;
            // var targetPos = Model.GetPathfinder().Target.position;
            // Model.SetNodes(pos, targetPos);
            // Model.CalculatePath();
        }
        
        private void Follow()
        {
            // Vector3 flockingDir = Controller.GetFlocking().GetDir();
            // Model.FollowTarget(Model.GetNextWaypoint(), flockingDir, Controller.GetAvoidance());
        }
    }
}