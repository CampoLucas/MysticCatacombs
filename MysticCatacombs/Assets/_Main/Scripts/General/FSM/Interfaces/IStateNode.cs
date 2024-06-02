using System;
using System.Collections.Generic;

namespace Game.StateMachine.Interfaces
{
    /// <summary>
    /// Interface representing a node in a state machine.
    /// It is responsible for holding a state and its possible transitions. 
    /// </summary>
    public interface IStateNode : IDisposable
    {
        IState State { get; }
        HashSet<ITransition> Transitions { get; }

        bool AddTransition(string to, IPredicate condition, bool forceTransition = false);
    }
}