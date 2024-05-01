using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.FSM
{
    public class State<T> : IState<T>
    {
        public Action OnStart;
        public Action OnExecute;
        public Action OnExit;
        
        private Dictionary<T, IState<T>> _transitions = new();
        private List<T> _transitionList = new();

        public virtual void Start()
        {
            OnStart?.Invoke();
        }

        public virtual void Execute()
        {
            OnExecute?.Invoke();
        }

        public virtual void Exit()
        {
            OnExit?.Invoke();
        }

        public IState<T> GetTransition(T input) => _transitions.ContainsKey(input) ? _transitions[input] : null;

        public void AddTransition(T input, IState<T> state)
        {
            _transitions[input] = state;
            _transitionList.Add(input);
        }

        public void AddTransition(Dictionary<T, IState<T>> transitions)
        {
            _transitions = _transitions.Union(transitions).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        
        /// <summary>
        /// Iterates throw a list to remove the provided state.
        /// </summary>
        /// <param name="state"></param>
        public void RemoveTransition(IState<T> state)
        {
            // foreach(var item in _transitions)
            // {
            //     if (item.Value != state) continue;
            //     _transitions.Remove(item.Key);
            //     break;
            // }

            // This is better because it doesn't iterate a dictionary, but a list.
            // Also using a for loop is a better choice because ehn using a foreach loop, you cannot modify the collection while iterating over it without causing an exception.
            // With a for loop you can remove items safely.
            for (var i = _transitionList.Count - 1; i >= 0; i--)
            {
                if (_transitions[_transitionList[i]] != state) continue;
                _transitions.Remove(_transitionList[i]);
                _transitionList.RemoveAt(i);
            }
        }

        public void RemoveTransition(T input)
        {
            if (_transitions.ContainsKey(input))
            {
                _transitions.Remove(input);
                _transitions.Remove(input);
            }
        }

        /// <summary>
        /// A method that removes all transitions and nullifies the transition dictionary and list.
        /// </summary>
        public virtual void Dispose()
        {
            for (var i = _transitionList.Count - 1; i >= 0; i--)
            {
                RemoveTransition(_transitionList[i]);
            }
            _transitions = null;
            _transitionList = null;
            Logging.LogDestroy("Transition Dictionary Nulled");
        }

        public virtual bool CanTransition() => true;
    }
}
