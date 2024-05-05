using UnityEngine;

namespace Game.CustomCollider
{
    public class SphereTrigger : Trigger
    {
        public float Radius
        {
            get
            {
                var scale = MyTransform ? MyTransform.lossyScale : transform.lossyScale;
                var highestAxis = Mathf.Max(scale.x, Mathf.Max(scale.y, scale.z));
                return radius * highestAxis;
            }
            set => radius = value;
        }
        
        [SerializeField] private float radius = 1;

        protected override void OnDrawGizmos()
        {
            
            base.OnDrawGizmos();
            Gizmos.DrawWireSphere(Offset, Radius);
        }
    }
}