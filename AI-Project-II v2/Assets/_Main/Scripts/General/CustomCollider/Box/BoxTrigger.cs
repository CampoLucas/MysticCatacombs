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
            Vector3 halfSize = size / 2f;

            // Define local axes before rotation
            Vector3[] localAxes = new Vector3[]
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
            Vector3[] corners = new Vector3[8];
            for (int i = 0; i < 8; i++)
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
        
        public void DrawWireCubeRotation(Vector3 center, Vector3 size, Transform origin)
        {
            // Calculate half size to get proper corner positions
            Vector3 halfSize = size / 2f;

            // Define local corners before rotation
            Vector3[] localCorners = new Vector3[]
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

            // Convert local corners to world space
            Vector3[] worldCorners = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                Vector3 cornerWithOffset = origin.InverseTransformPoint(center) + localCorners[i];
                worldCorners[i] = cornerWithOffset;
            }

            // Draw the lines of the cube
            Gizmos.DrawLine(worldCorners[0], worldCorners[1]);
            Gizmos.DrawLine(worldCorners[1], worldCorners[2]);
            Gizmos.DrawLine(worldCorners[2], worldCorners[3]);
            Gizmos.DrawLine(worldCorners[3], worldCorners[0]);

            Gizmos.DrawLine(worldCorners[4], worldCorners[5]);
            Gizmos.DrawLine(worldCorners[5], worldCorners[6]);
            Gizmos.DrawLine(worldCorners[6], worldCorners[7]);
            Gizmos.DrawLine(worldCorners[7], worldCorners[4]);

            Gizmos.DrawLine(worldCorners[0], worldCorners[4]);
            Gizmos.DrawLine(worldCorners[1], worldCorners[5]);
            Gizmos.DrawLine(worldCorners[2], worldCorners[6]);
            Gizmos.DrawLine(worldCorners[3], worldCorners[7]);
        }
    }
}
