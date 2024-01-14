using UnityEngine;

namespace Project.Utils.Mask
{
    public static class UtilsMask
    {
        public static int IgnoreMask(int[] mask, int allMasks = Physics.AllLayers)
        {
            var ignoredLayers = 0;
            for (var i = 0; i < mask.Length; i++)
            {
                ignoredLayers += allMasks << mask[i];
            }

            ignoredLayers *= -1;
            return allMasks - ignoredLayers;
        }
        
        public static int IgnoreMask(int mask, int allMasks = Physics.AllLayers)
        {
            var ignoredLayers = 0;
            ignoredLayers += allMasks << mask;

            ignoredLayers *= -1;
            return allMasks - ignoredLayers;
        }

        public static int Invert(in int mask, in int allMasks = Physics.AllLayers)
        {
            return ~mask & allMasks;
        }

        public static bool Contains(int mask, int layerMask)
        {
            var shiftedLayer = 1 << mask;
            return (layerMask & shiftedLayer) != 0;
        }
    }
}