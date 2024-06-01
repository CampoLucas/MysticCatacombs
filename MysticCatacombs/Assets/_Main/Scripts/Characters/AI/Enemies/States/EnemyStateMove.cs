using Game.Enemies.States;
using Game.Interfaces;
using UnityEngine;

namespace Game.Player.States
{
    public class EnemyStateMove<T> : EnemyStateBase<T>
    {
        private ISteering _steering;
        private float _speed;
        private bool _move;
        private float _moveAmount;

        public EnemyStateMove(ISteering steering, float speed, ISteeringDecorator[] decorators = null, bool move = true, float moveAmount = 1)
        {
            _steering = steering;
            _speed = speed;
            _move = move;
            _moveAmount = moveAmount;
            
            if (decorators == null) return;
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
            if (_move)
                Model.Move(Model.Transform.forward, _speed);
            //Model.Move(dir);
            Model.Rotate(dir);
            View.UpdateMovementValues(Controller.MoveAmount() * _moveAmount);
        }
        
        public override void Exit()
        {
            base.Exit();
            Model.Move(Vector3.zero);
        }

        public override void Dispose()
        {
            if (_steering != null) _steering.Dispose();
            _steering = null;
            
            base.Dispose();
        }
    }
}