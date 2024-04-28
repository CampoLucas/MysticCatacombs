using Game.Enemies.States;
using Game.Entities.Steering.Testing;
using Game.Interfaces;
using UnityEngine;
using ISteering = Game.Entities.Steering.Testing.ISteering;

namespace Game.Player.States
{
    public class EnemyStateMove<T> : EnemyStateBase<T>
    {
        private ISteering _steering;

        public EnemyStateMove(ISteering steering, ISteeringDecorator[] decorators)
        {
            _steering = steering;

            for (var i = 0; i < decorators.Length; i++)
            {
                decorators[i].SetChild(_steering);
                _steering = decorators[i];
            }
        }
        
        public override void Start()
        {
            base.Start();

            Controller.SetSteering(_steering);
        }

        public override void Execute()
        {
            base.Execute();

            var dir = Controller.MoveDirection();
            dir.y = 0;
            
            Model.Move(dir);
            Model.Rotate(dir);
            View.UpdateMovementValues(Controller.MoveAmount());
        }
        
        public override void Exit()
        {
            base.Exit();
            Model.Move(Vector3.zero);
        }
    }
}