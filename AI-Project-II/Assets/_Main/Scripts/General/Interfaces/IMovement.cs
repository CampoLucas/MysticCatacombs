using System;
using UnityEngine;

namespace Project
{
    public interface IMovement : IDisposable
    {
        void Move(Vector3 direction);
    }
}