using System;
using UnityEngine;

namespace Game.Entities.Steering.Testing
{
    public interface ISteering : IDisposable
    {
        Vector3 GetDir(Transform target);
        Vector3 GetDir(Vector3 position);
    }

    public interface ISteeringDecorator : ISteering
    {
        ISteering Child { get; }

        void SetChild(ISteering child);
    }
}