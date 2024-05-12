using System;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities
{
    public class Movement : IMovement
    {
        private CharacterController _controller;

        public Movement(CharacterController controller)
        {
            _controller = controller;
        }

        public void Move(Vector3 dir)
        {
            _controller.Move(dir);
        }

        public void Move(Vector3 dir, float speed, float delta = -1)
        {
            if (delta < 0) delta = Time.deltaTime;
            _controller.Move(dir * (speed * delta));
        }

        public void Dispose()
        {
            _controller = null;
        }
    }
}