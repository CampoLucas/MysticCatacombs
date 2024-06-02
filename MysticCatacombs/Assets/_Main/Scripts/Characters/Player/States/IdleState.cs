using UnityEngine;

namespace Game.Player.States
{
    public class IdleState : EntityState
    {
        
        protected override void OnUpdate()
        {
            base.OnUpdate();
            // if (Controller.MoveDirection() != Vector3.zero)
            // {
            //     Controller.StateMachine.SetState(_inMoving);
            // }
            //
            // if (Controller.DoLightAttack())
            // {
            //     Controller.StateMachine.SetState(_inLightAttack);
            // }
            //
            // if (Controller.DoHeavyAttack())
            // {
            //     Controller.StateMachine.SetState(_inHeavyAttack);
            // }
            
            View.UpdateMovementValues(Controller.MoveAmount());
            
        }
    }
}