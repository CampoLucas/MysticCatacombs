using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateIdle<T> : PlayerStateBase<T>
    {
        private readonly T _inMoving;
        private readonly T _inLightAttack;
        private readonly T _inHeavyAttack;

        public PlayerStateIdle(in T inMoving, in T inLightAttack, in T inHeavyAttack, in T inDamage, in T inDead): base(inDamage, inDead)
        {
            _inMoving = inMoving;
            _inLightAttack = inLightAttack;
            _inHeavyAttack = inHeavyAttack;
        }

        public override void Execute()
        {
            base.Execute();
            if (Controller.MoveDirection() != Vector3.zero)
            {
                Controller.StateMachine.SetState(_inMoving);
            }

            if (Controller.DoLightAttack())
            {
                Controller.StateMachine.SetState(_inLightAttack);
            }

            if (Controller.DoHeavyAttack())
            {
                Controller.StateMachine.SetState(_inHeavyAttack);
            }
            
            View.UpdateMovementValues(Controller.MoveAmount());
            
        }
    }
}