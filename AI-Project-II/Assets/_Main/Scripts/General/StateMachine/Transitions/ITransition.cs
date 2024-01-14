using System;

namespace Game.StateMachine
{
    /// <summary>
    /// Define a condition that has to be met in order to move into another state.
    /// </summary>
    public interface ITransition : IDisposable
    {
        string ToName { get; }
        IPredicate Condition { get; }
    }
}