using System;
using Game.StateMachine;
using Game.StateMachine.States;
using UnityEngine;

namespace Project.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerModel _model;
        private InputManager _input;
        private FSM _fsm;

        private void Awake()
        {
            _model = GetComponent<PlayerModel>();
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

            var move = new MovementState(_model.Movement, _model.Stats.Move, _input.MoveDir);
            var run = new MovementState(_model.Movement, _model.Stats.Run, _input.MoveDir);
            
            //Idle
            //Movement state
            //Running state
            //Damage state
            //Attack state
            //Death state

            _fsm.AddState("Move", move);
            _fsm.AddState("Run", run);
            _fsm.SetState("Move");
        }
    }
}