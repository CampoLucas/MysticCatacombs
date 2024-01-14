using Game.StateMachine;
using Project.Characters.Player;
using Project.Locomotion;
using UnityEngine;

namespace Project.Characters
{
    public class MovementState : State
    {
        private MovementData data;
        
        public MovementState(MovementData movementSettings)
        {
            data = movementSettings;
        }

        protected override void OnAwake()
        {
            
        }

        protected override void OnUpdate()
        {
            
        }
    }

    public class PlayerState
    {
        protected PlayerModel Model;
        protected PlayerController Controller;
    }
}