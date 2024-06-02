using System;
using System.Collections.Generic;
using Game.StateMachine.Interfaces;
using Game.StateMachine.Predicates;
using UnityEngine;
using IState = Game.StateMachine.Interfaces.IState;

namespace Game.StateMachine
{
    /// <summary>
    /// A generic class that implements a Finite State Machine for a generic type T.
    /// </summary>
    public class FSM : IStateMachine, IDisposable
    {
        public GameObject Owner { get; private set; }
        public string DefaultState { get; private set; } = "";
        public string CurrentState { get; private set; }
        public bool Enable { get; set; }

        private IStateNode _current;
        private List<string> _states = new();
        private Dictionary<string, IStateNode> _nodes = new();
        private HashSet<ITransition> _anyTransitions = new();
        private bool _enable;

        public FSM(GameObject owner, bool enable = true)
        {
            Owner = owner;
            
            if (!Owner)
            {
#if UNITY_EDITOR
                Debug.LogError("FSM: The owner given is null.");
#endif
                return;
            }

            Enable = enable;
        }

        public void Update()
        {
            if (!Enable) return;
            
            if (_current == null)
            {
#if UNITY_EDITOR
                Debug.LogError("FSM: The current state is null. returning", Owner);
#endif
                return;
            }

            if (TryGetTransition(out var tr))
            {
                SetState(tr.ToName);
            }
            
            _current.State.Update();
        }

        public bool SetDefaultState(string name)
        {
            if (!ContainsState(name)) return false;

            DefaultState = name;
            return true;
        }

        public bool SetState(string name)
        {
            if (_nodes.TryGetValue(name, out var stateNode))
            {
                if (stateNode == _current && !_current.State.CanTransitionToItself())
                {
                    return false;
                }
                
                if (_current != null) _current.State.Exit();
                _current = stateNode;
                CurrentState = name;
                _current.State.Start();

                return true;
            }

            return false;
        }

        public bool SetToDefault()
        {
            return SetState(DefaultState);
        }
        
        public bool AddTransition(string from, string to, IPredicate condition, bool forceTransition = false)
        {
            if (!ContainsState(from) || !ContainsState(to)) return false;

            var fromState = GetNode(from);
            return fromState.AddTransition(to, condition, forceTransition);;
        }
        
        public bool AddTransition(string from, string to, Func<bool> condition, bool forceTransition = false)
        {
            return AddTransition(from, to, new FuncPredicate(condition), forceTransition);
        }
        
        public bool AddAnyTransition(string to, IPredicate condition, bool forceTransition = false)
        {
            if (!ContainsState(to)) return false;

            return _anyTransitions.Add(new Transition(to, condition, forceTransition));
        }
        
        public bool AddAnyTransition(string to, Func<bool> condition, bool forceTransition = false)
        {
            return AddAnyTransition(to, new FuncPredicate(condition), forceTransition);
        }
        
        private IStateNode GetNode(string name)
        {
            if (!ContainsState(name))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"FSM: There a are no states with the name {name}, returning null", Owner);
#endif
            }
            _nodes.TryGetValue(name, out var stateNode);
            return stateNode;
        }
        
        public bool AddState(string name, IState state)
        {
            if (!_nodes.ContainsKey(name))
            {
                var node = new StateNode(state);
                if (!ContainsState(DefaultState))
                {
                    DefaultState = name;
                }
                
                _nodes[name] = node;
                _states.Add(name);
                node.State.Awake(this);
                return true;
            }

#if UNITY_EDITOR
            Debug.LogWarning($"FSM: Already contains a state with the name '{name}'", Owner);
#endif
            
            return false;
        }
        
        public void AddState(Dictionary<string, IState> states)
        {
            var keys = states.Keys;
            
            foreach (var key in keys)
            {
                AddState(key, states[key]);
            }
        }
        
        public bool RemoveState(string state)
        {
            if (!_nodes.ContainsKey(state)) return false;
            
            _nodes[state].Dispose();
            _nodes.Remove(state);
            _states.Remove(state);
            return true;
        }
        
        public IState GetState(string name)
        {
            return GetNode(name).State;
        }

        public TState GetStateType<TState>(string name) where TState : IState
        {
            return (TState)GetState(name);
        }

        public bool ContainsState(string name)
        {
            return _nodes.ContainsKey(name);
        }

        private bool TryGetTransition(out ITransition transition)
        {
            transition = null;
            if (TryGetTransition(_anyTransitions, out transition))
            {
                return true;
            }
            else if (TryGetTransition(_current.Transitions, out transition))
            {
                return true;
            }

            return false;
        }
        
        private bool TryGetTransition(HashSet<ITransition> transitions, out ITransition transition)
        {
            transition = null;
            foreach (var tr in transitions)
            {
                if (!tr.Condition.Evaluate()) continue;
                if (!_current.State.CanTransition() && !tr.ForceTransition) continue;
                transition = tr;
                return true;
            }

            return false;
        }
        
        public void Dispose()
        {
            Owner = null;
            _current = null;

            for (var i = 0; i < _states.Count; i++)
            {
                RemoveState(_states[i]);
            }
            
            _states.Clear();
            _states = null;
            _nodes.Clear();
            _nodes = null;

            foreach (var tr in _anyTransitions)
            {
                tr.Dispose();
            }
            
            _anyTransitions.Clear();
            _anyTransitions = null;
        }

        public bool CanTransition() => _current == null || _current.State.CanTransition();

        public void Draw()
        {
            if (_current == null) return;
            _current.State.Draw();
        }
        
        public void DrawOnSelected()
        {
            if (_current == null) return;
            _current.State.DrawSelected();
            
            if (Owner == null) return;
#if UNITY_EDITOR
            UnityEditor.Handles.Label(Owner.transform.position, CurrentState);
#endif
        }
    }
}
