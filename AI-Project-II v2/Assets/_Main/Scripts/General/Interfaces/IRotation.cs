using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IRotation : IDisposable
    {
        void Rotate(Vector3 dir);

        void Rotate(Vector3 dir, float speed, float delta = -1);
    }
}