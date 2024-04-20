using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Locomotion
{
    /// <summary>
    /// It handles rigidBody movement. It can also set up the rigidBody values on the constructor.
    /// </summary>
    public class RbMovement : IMovement
    {
        private Rigidbody _rb;

        /// <summary>
        /// Base constructor, it takes the rigidBody without setting it's values.
        /// </summary>
        public RbMovement(ref Rigidbody rigidBody)
        {
            _rb = rigidBody;
        }

        /// <summary>
        /// It takes the rigidBody and sets it's values.
        /// </summary>
        public RbMovement(ref Rigidbody rigidBody, in float mass = 1, in float drag = 0, in float angularDrag = 0.05f, 
            in bool useGravity = true, in bool isKinematic = false) : this(ref rigidBody)
        {
            // var m = mass < 0 ? 0.000001f : mass;
            //
            // _rb.mass = m;
            // _rb.drag = drag;
            // _rb.angularDrag = angularDrag;
            // _rb.useGravity = useGravity;
            // _rb.isKinematic = isKinematic;
            RbSetting.SetRigidBody(ref rigidBody, new RbSetting(mass, drag, angularDrag, useGravity, isKinematic));
        }

        /// <summary>
        /// It takes the rigidBody and sets it's values using a RbSetting.
        /// </summary>
        public RbMovement(ref Rigidbody rigidBody, RbSetting setting) : this(ref rigidBody)
        {
            RbSetting.SetRigidBody(ref rigidBody, setting);
        }
        
        /// <summary>
        /// Moves the object to a direction.
        /// </summary>
        /// <param name="direction"></param>
        public void Move(in Vector3 direction)
        {
            _rb.velocity += direction;
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
        /// Disposes of all references.
        /// </summary>
        public void Dispose()
        {
            _rb = null;
        }
    }
}
