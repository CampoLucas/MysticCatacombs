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
        
        /// <summary>
        /// Moves the object to a direction.
        /// </summary>
        /// <param name="direction"></param>

        public void Move(in Vector3 direction)
        {
            _origin.position += direction;
        }

        
        /// <summary>
        /// Moves the object to a normalized direction, multiplied by a speed and delta.
        /// </summary>
        /// <param name="normalizedDir">A direction</param>
        /// <param name="speed"></param>
        /// <param name="delta"></param>
        public void Move(in Vector3 normalizedDir, in float speed, in float delta = -1)
        {
            var d = delta < 0 ? Time.deltaTime : delta;
            Move(normalizedDir.normalized * (speed * d));
        }

        /// <summary>
        /// Disposes of all the references.
        /// </summary>
        public void Dispose()
        {
            _origin = null;
        }
    }
}