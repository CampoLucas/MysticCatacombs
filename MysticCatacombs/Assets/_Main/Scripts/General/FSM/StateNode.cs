using System.Collections.Generic;
using Game.StateMachine.Interfaces;

namespace Game.StateMachine
{
    /// <summary>
    /// Represents a node in a state machine.
    /// </summary>
    public class StateNode : IStateNode
    {
        /// <summary>
        /// The current state.
        /// </summary>
        public IState State { get; private set; }
        /// <summary>
        /// set of transitions from this state.
        /// </summary>
        public HashSet<ITransition> Transitions { get; private set; }

        public StateNode(IState state)
        {
            State = state;
            Transitions = new();
        }
        
        /// <summary>
        /// Method to add a transition to the Transitions set.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="condition"></param>
        /// <param name="forceTransition"></param>
        /// <returns>Returns true if the transition was added successfully, false if it was already present.</returns>
        public bool AddTransition(string to, IPredicate condition, bool forceTransition = false)
        {
            return Transitions.Add(new Transition(to, condition, forceTransition));
        }
        
        /// <summary>
        /// Disposes of the State and all transitions, and then clears the Transitions set.
        /// </summary>
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
}