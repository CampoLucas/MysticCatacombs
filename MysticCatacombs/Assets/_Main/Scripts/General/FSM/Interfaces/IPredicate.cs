using System;

namespace Game.StateMachine.Interfaces
{
    /// <summary>
    /// Used to define a criteria and determine if it is met.
    /// </summary>
    public interface IPredicate : IDisposable
    {
        /// <summary>
        /// Method to determine if the defined criteria is met.
        /// </summary>
        /// <returns></returns>
        bool Evaluate();
    }
}