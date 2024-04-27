using System;
using Game.StateMachine;
using Project.Characters.Player;
using Project.Locomotion;
using UnityEngine;

namespace Project.Characters
{
    public class MovementState : State
    {
        private MovementData _data;
        private IMovement _movement;
        private Func<Vector3> _moveDir;
        
        public MovementState(IMovement movement, MovementData movementSettings, Func<Vector3> moveDir)
        {
            _movement = movement;
            _data = movementSettings;
        }

        protected override void OnUpdate()
        {
            _movement.Move(_moveDir(), _data.GetSpeed(1));
        }
    }
}