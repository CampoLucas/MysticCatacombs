using System;
using Game.StateMachine.Interfaces;

namespace Game.StateMachine.Predicates
{
    /// <summary>
    /// Implements the IPredicate interface and encapsulates a boolean function delegate.
    /// </summary>
    public class FuncPredicate : IPredicate
    {
        private Func<bool> _func;

        public FuncPredicate(Func<bool> func)
        {
            _func = func;
        }

        /// <summary>
        /// Checks if the function delegate is not null and invokes it, returning the result.
        /// </summary>
        /// <returns></returns>
        public bool Evaluate() => _func != null && _func();
        
        /// <summary>
        /// Dispose method that sets the function delegate to null for cleanup.
        /// </summary>
        public void Dispose()
        {
            _func = null;
        }
    }
}