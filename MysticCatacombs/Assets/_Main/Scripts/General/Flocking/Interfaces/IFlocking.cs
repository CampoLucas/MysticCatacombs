using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IFlocking : IDisposable
    {
        Vector3 GetDir(List<IBoid> boids, IBoid self);
    }
}
