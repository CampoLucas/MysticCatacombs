using System;
using UnityEngine;

namespace Project.Locomotion
{
    public class TransformMovement : IMovement
    {
        private Transform _origin;

        public TransformMovement(Transform origin)
        {
            _origin = origin;
        }
        
        public void Move(Vector3 direction)
        {
            _origin.position += direction;
        }

        public void Dispose()
        {
            _origin = null;
        }
    }
}