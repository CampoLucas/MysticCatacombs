using Game.Interfaces;
using UnityEngine;

namespace Game.Enemies.States
{
    public class WizardStatePursuit<T> : WizardStateBase<T>
    {
        protected ISteering Steering;
        protected ISteering ObsAvoidance;

        public WizardStatePursuit(ISteering steering, ISteering obsAvoidance)
        {
            Steering = steering;
            ObsAvoidance = obsAvoidance;
        }

        public override void Start()
        {
            base.Start();
            Model.SetFollowing(true);
            Model.SetMovement(Model.GetRunningMovement());
        }


        public override void Execute()
        {
            base.Execute();
            
            Tree.Execute();
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
            Model.FollowTarget(Steering, ObsAvoidance);
        }

        public override void Dispose()
        {
            base.Dispose();
            Steering = null;
            ObsAvoidance = null;
        }
    }
}