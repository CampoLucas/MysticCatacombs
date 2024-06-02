using UnityEngine;

namespace Game.Player.States
{
    public class MoveState : EntityState
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            Model.Move(Controller.MoveDirection(), Model.GetData().MoveSpeed);
            Model.Rotate(Controller.MoveDirection());
            View.UpdateMovementValues(Controller.MoveAmount());
        }
        
        protected override void OnExit()
        {
            base.OnExit();
            Model.Move(Vector3.zero);
        }
    }
}