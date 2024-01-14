using System;

namespace Game.StateMachine
{
    public class FuncPredicate : IPredicate
    {
        private Func<bool> _func;

        public FuncPredicate(Func<bool> func)
        {
            _func = func;
        }

        public bool Evaluate() => _func();

        public void Dispose()
        {
            _func = null;
        }
    }
}