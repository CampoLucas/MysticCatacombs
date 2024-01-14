using System;
using UnityEngine;

namespace Project.Characters.Player
{
    public class InputManager : MonoBehaviour
    {
        public Vector3 MoveDirection { get; private set; }

        private void Update()
        {
            MoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        }
    }
    
}