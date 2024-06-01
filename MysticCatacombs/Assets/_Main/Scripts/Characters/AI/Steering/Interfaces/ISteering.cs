using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface ISteering : IDisposable, IDrawable
    {
        Vector3 CatchDirection { get; }
        /// <summary>
        /// Calculates the direction using a Transform and returns it. 
        /// </summary>
        Vector3 GetDir(Transform target);

        /// <summary>
        /// Calculates the direction using a position and returns it. 
        /// </summary>
        /// <returns></returns>
        Vector3 GetDir(Vector3 position);
    }
}
