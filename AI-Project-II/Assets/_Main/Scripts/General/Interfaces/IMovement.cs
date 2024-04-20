using System;
using UnityEngine;

namespace Project
{
    public interface IMovement : IDisposable
    {
        void Move(in Vector3 direction);
        void Move(in Vector3 normalizedDir, in float speed, in float delta = -1);
    }
}