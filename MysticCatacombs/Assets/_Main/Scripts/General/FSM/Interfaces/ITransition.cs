using System;

namespace Game.StateMachine.Interfaces
{
    /// <summary>
    /// Interface representing a transition between states in a state machine.
    /// </summary>
    public interface ITransition : IDisposable
    {
        /// <summary>
        /// Name of the target to transition to.
        /// </summary>
        string ToName { get; }
        /// <summary>
        /// Transitions even of the state can't transition.
        /// </summary>
        bool ForceTransition { get; }
        
        /// <summary>
        /// Condition to be met for the transition to occur.
        /// </summary>
        IPredicate Condition { get; }
    }
}