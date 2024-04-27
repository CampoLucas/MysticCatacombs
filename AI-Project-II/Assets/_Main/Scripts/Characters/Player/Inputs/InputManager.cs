using System;
using Project.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Characters.Player
{
    public class InputManager : MonoBehaviour
    {
        public Vector3 MoveDirection { get; private set; }
        public float MoveAmount { get; private set; }
        public bool LightAttack { get; private set; }
        public bool HeavyAttack { get; private set; }

        private PlayerInputs _input;

        private void OnEnable()
        {
            _input ??= new PlayerInputs();
            SubscribeEvents();
            _input.Enable();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
            _input.Disable();
        }

        private void Update()
        {
            if (LightAttack && HeavyAttack)
            {
                LightAttack = false;
                HeavyAttack = true;
            }
        }

        public Vector3 MoveDir()
        {
            return MoveDirection;
        }

        #region EventHandelers

        private void SubscribeEvents()
        {
            _input.Player.Movement.performed += MovementPerformedHandler;
            _input.Player.Movement.canceled += MovementCanceledHandler;
            _input.Player.LightAttack.performed += LightAttackPerformedHandler;
            _input.Player.LightAttack.canceled += LightAttackCanceledHandler;
            _input.Player.HeavyAttack.performed += HeavyAttackPerformedHandler;
            _input.Player.HeavyAttack.canceled += HeavyAttackCanceledHandler;
        }

        private void UnsubscribeEvents()
        {
            _input.Player.Movement.performed -= MovementPerformedHandler;
            _input.Player.Movement.canceled -= MovementCanceledHandler;
            _input.Player.LightAttack.performed -= LightAttackPerformedHandler;
            _input.Player.LightAttack.canceled -= LightAttackCanceledHandler;
            _input.Player.HeavyAttack.performed -= HeavyAttackPerformedHandler;
            _input.Player.HeavyAttack.canceled -= HeavyAttackCanceledHandler;
        }

        private void MovementPerformedHandler(InputAction.CallbackContext context)
        {
            MoveDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        }

        private void MovementCanceledHandler(InputAction.CallbackContext context)
        {
            MoveDirection = Vector3.zero;
        }

        private void LightAttackPerformedHandler(InputAction.CallbackContext context)
        {
            LightAttack = true;
        }

        private void LightAttackCanceledHandler(InputAction.CallbackContext context)
        {
            LightAttack = false;
        }

        private void HeavyAttackPerformedHandler(InputAction.CallbackContext context)
        {
            HeavyAttack = true;
        }

        private void HeavyAttackCanceledHandler(InputAction.CallbackContext context)
        {
            HeavyAttack = false;
        }

        #endregion

        private void OnDestroy()
        {
            _input.Dispose();
            _input = null;
        }
    }
    
}