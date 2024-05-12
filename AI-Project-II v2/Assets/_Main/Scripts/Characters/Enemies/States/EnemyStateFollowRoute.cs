using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateFollowRoute<T> : EnemyStateBase<T>
    {
        public override void Execute()
        {
            base.Execute();

            //Tree.Execute();
            if (Model.ReachedWaypoint())
            {
                Model.ChangeWaypoint();
            }
            //Model.FollowTarget(Model.GetNextWaypoint(), Controller.GetObsAvoid());
            View.UpdateMovementValues(0.5f);
        }

        public override void Exit()
        {
            base.Exit();
            Model.Move(Vector3.zero);
            //View.UpdateMovementValues(Model.GetMoveAmount());
        }
    }
}