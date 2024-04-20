using UnityEngine;

namespace Project.Locomotion
{
    /// <summary>
    /// Class that contains physics settings for the RigidBody.
    /// </summary>
    [System.Serializable]
    public struct RbSetting
    {
        public float Mass => mass;
        public float Drag => drag;
        public float AngularDrag => angularDrag;
        public bool UseGravity => useGravity;
        public bool IsKinematic => isKinematic;
        
        [Header("Settings")]
        [SerializeField] private float mass;
        [SerializeField] private float drag;
        [SerializeField] private float angularDrag;
        [SerializeField] private bool useGravity;
        [SerializeField] private bool isKinematic;

        public RbSetting(in float mass = 1, in float drag = 0, in float angularDrag = 0.05f, 
            in bool useGravity = true, in bool isKinematic = false)
        {
            this.mass = mass;
            this.drag = drag;
            this.angularDrag = angularDrag;
            this.useGravity = useGravity;
            this.isKinematic = isKinematic;
        }

        /// <summary>
        /// Static method to set any rigidBody's settings.
        /// </summary>
        /// <param name="rigidBody"></param>
        /// <param name="setting"></param>
        public static void SetRigidBody(ref Rigidbody rigidBody, in RbSetting setting)
        {
            var mass = setting.mass < 0 ? 0.000001f : setting.mass;

            rigidBody.mass = mass;
            rigidBody.drag = setting.drag;
            rigidBody.angularDrag = setting.angularDrag;
            rigidBody.useGravity = setting.useGravity;
            rigidBody.isKinematic = setting.isKinematic;
        }
    }
}