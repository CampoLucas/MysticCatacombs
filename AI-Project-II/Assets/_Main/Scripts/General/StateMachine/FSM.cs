using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.StateMachine
{
    public class FSM : IDisposable
    {
        public class StateNode : IDisposable
        {
            public IState State { get; private set; }
            public HashSet<ITransition> Transitions { get; private set; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(string to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }

            public void Dispose()
            {
                State.Dispose();
                State = null;
                
                foreach (var t in Transitions)
                {
                    t.Dispose();
                }
                Transitions.Clear();
                Transitions = null;
            }
        }

        public GameObject Owner { get; private set; }
        public string CurrentState { get; private set; }

        private StateNode _current;
        private List<string> _stateNames = new();
        private Dictionary<string, StateNode> _nodes = new();
        private HashSet<ITransition> _anyTransitions = new();

        public FSM(GameObject owner)
        {
            Owner = owner;
        }
        
        public void Update()
        {
            var transition = GetTransition();
            if (transition != null /* && _current != null && _current.State.CanTransition()*/
               ) SetState(transition.ToName);

            if (_current != null) _current.State.Update();
        }

        public void FixedUpdate()
        {
            if (_current != null)
                _current.State.FixedUpdate();
        }

        public void SetState(string stateName)
        {
            if (_nodes.TryGetValue(stateName, out var target) && target != _current)
            {
                if (_current != null)
                    _current.State.Exit();
                _current = target;
                CurrentState = stateName;
                _current.State.Start();
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"FSM: There is no state with the name of {stateName}. Skipping.");
#endif
            }
        }

        public void AddTransition(string from, string to, IPredicate condition)
        {
            if (!ContainsState(from) || !ContainsState(to)) return;

            var fromState = GetNode(from);
            fromState.AddTransition(to, condition);
        }

        public void AddTransition(string from, Dictionary<string, IPredicate> transitions)
        {
            if (!ContainsState(from)) return;

            var fromState = GetNode(from);
            var keys = transitions.Keys.ToList();

            for (var i = 0; i < keys.Count; i++) fromState.AddTransition(keys[i], transitions[keys[i]]);
        }

        public void AddAnyTransition(string to, IPredicate condition)
        {
            if (!ContainsState(to)) return;

            _anyTransitions.Add(new Transition(to, condition));
        }

        public StateNode AddState(string name, IState state, bool replace = false)
        {
            var contains = _nodes.ContainsKey(name);
            
            if (!contains || replace)
            {
                var node = new StateNode(state);

                if (replace && contains)
                {
                    _stateNames.Remove(name);
                }
                
                _nodes[name] = node;
                _stateNames.Add(name);
                node.State.Awake(this);
                return node;
            }
            return null;
        }

        public void AddState(Dictionary<string, IState> states)
        {
            var keys = states.Keys.ToList();

            for (var i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                var node = AddState(key, states[key]);
#if UNITY_EDITOR
                Debug.LogError($"FSM: Node is null");
#endif
            }
        }

        public void RemoveState(string state)
        {
            if (_nodes.ContainsKey(state))
            {
                _nodes[state].Dispose();
                _nodes[state] = null;
                _stateNames.Remove(state);
            }
        }

        public StateNode GetNode(string name)
        {
            if (_nodes.TryGetValue(name, out var target))
                return target;
            return null;
        }

        public bool ContainsState(string name)
        {
            return _nodes.ContainsKey(name);
        }

        public bool ContainsState(IState state)
        {
            var result = false;
            for (var i = 0; i < _stateNames.Count; i++)
            {
                var name = _stateNames[i];
                if (_nodes[name] != state) continue;
                result = true;
                break;
            }

            return result;
        }

        private ITransition GetTransition()
        {
            return GetTransition(_anyTransitions) ?? GetTransition(_current.Transitions);
        }

        private ITransition GetTransition(HashSet<ITransition> transitions)
        {
            foreach (var transition in transitions.Where(transition => transition.Condition.Evaluate()))
                return transition;
            return null;
        }

        public void Dispose()
        {
            Owner = null;
            _current = null;

            for (var i = 0; i < _stateNames.Count; i++)
            {
                RemoveState(_stateNames[i]);
            }
            _stateNames.Clear();
            _stateNames = null;
            _nodes.Clear();
            _nodes = null;

            foreach (var t in _anyTransitions)
            {
                t.Dispose();
            }
            
            _anyTransitions.Clear();
            _anyTransitions = null;
        }
    }
}