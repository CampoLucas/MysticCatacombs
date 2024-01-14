using System;
using Game.StateMachine;
using Game.StateMachine.States;
using UnityEngine;

namespace Project.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        private InputManager _input;
        private FSM _fsm;

        private void Awake()
        {
            _input = GetComponent<InputManager>();
            
            InitFsm();
        }

        private void Update()
        {
            _fsm.Update();
        }

        private void FixedUpdate()
        {
            _fsm.FixedUpdate();
        }

        private void InitFsm()
        {
            _fsm = new FSM(gameObject);

            var gravity = new MoveState(-0.67f, () => Vector3.up);
            var move = new MoveState(5, () => _input.MoveDirection);
            var movement = new CompoundState(new[]
            {
                new CompoundState.CompState(gravity, () => true),
                new CompoundState.CompState(move, () => true),
            });

            _fsm.AddState("Move", movement);
            _fsm.SetState("Move");
        }
    }
}