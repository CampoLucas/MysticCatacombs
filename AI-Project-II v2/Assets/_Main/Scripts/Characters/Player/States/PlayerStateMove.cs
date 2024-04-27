using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateMove<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inLightAttack;
        private readonly T _inHeavyAttack;
        private readonly T _inDamage;
        private readonly T _inDead;
        
        public PlayerStateMove(in T inIdle, in T inLightAttack, in T inHeavyAttack, in T inDamage, in T inDead): base(inDamage, inDead)
        {
            _inIdle = inIdle;
            _inLightAttack = inLightAttack;
            _inHeavyAttack = inHeavyAttack;
            _inDamage = inDamage;
            _inDead = inDead;
        }

        public override void Start()
        {
            base.Start();
            Model.Damageable.OnTakeDamage += TakeDamageHandler;
            Model.Damageable.OnDie += OnDieHandler;
            Model.SetMovement(Model.GetRunningMovement());
        }

        public override void Execute()
        {
            base.Execute();
            

            if (Controller.MoveDirection() == Vector3.zero)
            {
                Controller.StateMachine.SetState(_inIdle);
            }

            if (Controller.DoLightAttack())
            {
                Controller.StateMachine.SetState(_inLightAttack);
            }
            
            if (Controller.DoHeavyAttack())
            {
                Controller.StateMachine.SetState(_inHeavyAttack);
            }
            
            Model.Move(Controller.MoveDirection());
            Model.Rotate(Controller.MoveDirection());
            View.UpdateMovementValues(Controller.MoveAmount());
        }
        
        private void TakeDamageHandler()
        {
            Controller.StateMachine.SetState(_inDamage);
        }

        private void OnDieHandler()
        {
            Controller.StateMachine.SetState(_inDead);
        }
        
        public override void Exit()
        {
            base.Exit();
            Model.Damageable.OnTakeDamage -= TakeDamageHandler;
            Model.Damageable.OnDie -= OnDieHandler;
            Model.Move(Vector3.zero);
        }
    }
}