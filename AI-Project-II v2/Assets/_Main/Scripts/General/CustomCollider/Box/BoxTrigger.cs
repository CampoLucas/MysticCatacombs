using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.CustomCollider
{
    public class BoxTrigger : Trigger
    {
        public Vector3 Size
        {
            get
            {
                var tr = (MyTransform ? MyTransform : transform);
                return new(size.x * tr.lossyScale.x, size.y * tr.lossyScale.y, size.z * tr.lossyScale.z);
            }
            set => size = value;
        }
        
        [SerializeField] protected Vector3 size;

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            //Gizmos.matrix = transform.localToWorldMatrix;
            DrawWireCubeRotation(Offset, Size, transform.rotation);
            Gizmos.DrawSphere(Offset, 0.05f);
        }
        
        public void DrawWireCubeRotation(Vector3 center, Vector3 size, Quaternion rotation)
        {
            // Calculate half size to get proper corner positions
            var halfSize = size / 2f;

            // Define local axes before rotation
            var localAxes = new Vector3[]
            {
                new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
                new Vector3(halfSize.x, -halfSize.y, -halfSize.z),
                new Vector3(halfSize.x, -halfSize.y, halfSize.z),
                new Vector3(-halfSize.x, -halfSize.y, halfSize.z),
                new Vector3(-halfSize.x, halfSize.y, -halfSize.z),
                new Vector3(halfSize.x, halfSize.y, -halfSize.z),
                new Vector3(halfSize.x, halfSize.y, halfSize.z),
                new Vector3(-halfSize.x, halfSize.y, halfSize.z)
            };

            // Transform local axes by rotation and center
            var corners = new Vector3[8];
            for (var i = 0; i < 8; i++)
            {
                corners[i] = center + rotation * localAxes[i];
            }

            // Draw the lines of the cube
            Gizmos.DrawLine(corners[0], corners[1]);
            Gizmos.DrawLine(corners[1], corners[2]);
            Gizmos.DrawLine(corners[2], corners[3]);
            Gizmos.DrawLine(corners[3], corners[0]);

            Gizmos.DrawLine(corners[4], corners[5]);
            Gizmos.DrawLine(corners[5], corners[6]);
            Gizmos.DrawLine(corners[6], corners[7]);
            Gizmos.DrawLine(corners[7], corners[4]);

            Gizmos.DrawLine(corners[0], corners[4]);
            Gizmos.DrawLine(corners[1], corners[5]);
            Gizmos.DrawLine(corners[2], corners[6]);
            Gizmos.DrawLine(corners[3], corners[7]);
        }
    }
}
