using System;

namespace Game.StateMachine
{
    /// <summary>
    /// Used to perform search operations based on a set of criteria.
    /// </summary>
    public interface IPredicate : IDisposable
    {
        /// <summary>
        /// Test a condition and returns a boolean value - true or false.
        /// </summary>
        /// <returns></returns>
        bool Evaluate();
    }
}