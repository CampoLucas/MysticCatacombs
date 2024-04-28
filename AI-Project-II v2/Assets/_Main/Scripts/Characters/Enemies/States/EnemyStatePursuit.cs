using Game.Scripts.VisionCone;
using Game.Interfaces;
using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStatePursuit<T> : EnemyStateBase<T>
    {
        protected ISteering Steering;
        protected ISteering ObsAvoidance;

        public EnemyStatePursuit(ISteering steering, ISteering obsAvoidance)
        {
            Steering = steering;
            ObsAvoidance = obsAvoidance;
        }

        public override void Start()
        {
            base.Start();
            Model.SetFollowing(true);
            Model.SetMovement(Model.GetRunningMovement());
            Model.SetVisionConeColor(VisionConeEnum.InSight);
        }


        public override void Execute()
        {
            base.Execute();
            
            //Tree.Execute();
            Follow();
            View.UpdateMovementValues(Model.GetMoveAmount());
        }

        public override void Exit()
        {
            base.Exit();
            Model.Move(Vector3.zero);
            View.UpdateMovementValues(Model.GetMoveAmount());
        }

        protected virtual void Follow()
        {
            
        }

        public override void Dispose()
        {
            base.Dispose();
            Steering = null;
            ObsAvoidance = null;
        }
    }
}