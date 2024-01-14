using System;
using System.Collections.Generic;

namespace Game.StateMachine.States
{
    public class CompoundState : State
    {
        public class CompState : IDisposable
        {
            public State State => _state;
            
            private State _state;
            private Func<bool> _condition;

            public CompState(State state, Func<bool> condition)
            {
                _state = state;
                _condition = condition;
            }
            
            public void Execute()
            {
                if (_condition())
                    _state.Update();
            }

            public void Dispose()
            {
                _state.Dispose();
                _state = null;
                _condition = null;
            }
        }
        
        private List<CompState> _states;

        public CompoundState(IEnumerable<CompState> states)
        {
            _states = new (states);
        }

        protected override void OnAwake()
        {
            for (var i = 0; i < _states.Count; i++)
            {
                var state = _states[i];

                if (CheckState(state.State))
                    state.State.Awake(Fsm);
                else
                    _states.RemoveAt(i);
            }
        }

        protected override void OnStart()
        {
            for (var i = 0; i < _states.Count; i++)
            {
                var state = _states[i];
                state.State.Start();
            }
        }
        
        protected override void OnUpdate()
        {
            for (var i = 0; i < _states.Count; i++)
            {
                var state = _states[i];
                state.Execute();
            }
        }
        
        protected override void OnExit()
        {
            for (var i = 0; i < _states.Count; i++)
            {
                var state = _states[i];
                state.State.Exit();
            }
        }

        protected override void OnDestroy()
        {
            for (var i = 0; i < _states.Count; i++)
            {
                var state = _states[i];
                state.Dispose();
            }
        }

        private bool CheckState(State state)
        {
            if (state == this)
                return false;
            
            if (state is CompoundState)
                return false;

            if (Fsm.ContainsState(state))
                return false;
            
            return true;
        }
    }
}