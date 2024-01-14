using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.General;
using Project;
using Project.Locomotion;
using UnityEngine;

namespace Project.Characters.Player
{
    public class PlayerModel : MonoBehaviour
    {
        public PlayerSettings Stats => data;
        public float CurrentSpeed { get; private set; }
        [SerializeField] private PlayerSettings data;
        private IMovement _move;
        private NullChecker<IMovement> _moveCheck;

        private void Awake()
        {
            SetMovement(new TransformMovement(transform));
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!data)
            {
                Debug.LogError($"The player {gameObject.name} is missing PlayerSettings.", gameObject);
            }
#endif
        }

        
        public void Move(Vector3 dir)
        {
            _move.Move(dir * (CurrentSpeed * Time.deltaTime));
        }

        #region Setters
        public void SetCurrentSpeed(float value) => CurrentSpeed = value;

        /// <summary>
        /// Sets the movement logic of the player and catches if it is null.
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="dispose"></param>
        public void SetMovement(IMovement movement, bool dispose = false)
        {
            if (dispose && _moveCheck)
                _move.Dispose();
            _move = movement;
            _moveCheck.Set(_move);
        }

        #endregion
    }
}