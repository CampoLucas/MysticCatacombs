using UnityEngine;

namespace Project.Utils.Math
{
    public static class UtilsMath
    {
        public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
        {
            // Calculate the relative rotation between a and b.
            var relative = a * Quaternion.Inverse(b);
            
            // Normalize the relative rotation to have more accuracy.
            relative = relative.normalized;
            
            // Check if the dot product of relative and its inverse is negative, to know if the rotation is more than 180.
            if (Quaternion.Dot(relative, Quaternion.Inverse(relative)) < 0f)
            {
                // Take the conjugate (inverse of the rotation, same axis, opposite angle)
                relative = Quaternion.Inverse(relative);
            }

            return relative;
        }
    }
}