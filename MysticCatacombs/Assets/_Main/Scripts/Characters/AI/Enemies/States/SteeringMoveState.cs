using Game.Interfaces;
using UnityEngine;

namespace Game.Player.States
{
    public class SteeringMoveState : EntityState
    {
        private ISteering _steering;
        private float _speed;
        private bool _move;
        private float _moveAmount;

        public SteeringMoveState(ISteering steering, float speed, ISteeringDecorator[] decorators = null, 
            bool move = true, float moveAmount = 1)
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
        
        protected override void OnStart()
        {
            base.OnStart();

            Controller.SetSteering(_steering);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            var dir = Controller.MoveDirection();
            dir.y = 0;
            if (_move)
                Model.Move(Model.Transform.forward, _speed);
            //Model.Move(dir);
            Model.Rotate(dir);
            View.UpdateMovementValues(Controller.MoveAmount() * _moveAmount);
        }
        
        protected override void OnExit()
        {
            base.OnExit();
            Model.Move(Vector3.zero);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            if (_steering != null) _steering.Dispose();
            _steering = null;
        }
    }
}