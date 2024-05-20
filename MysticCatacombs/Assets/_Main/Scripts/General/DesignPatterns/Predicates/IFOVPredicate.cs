using System;
using UnityEngine;

namespace Game.DesignPatterns.Predicate
{
    public interface IFOVPredicate : IDisposable
    {
        bool Evaluate(Transform target);
    }
}