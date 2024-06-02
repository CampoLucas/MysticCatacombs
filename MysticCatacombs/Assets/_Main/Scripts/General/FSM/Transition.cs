using Game.StateMachine.Interfaces;

namespace Game.StateMachine
{
    /// <summary>
    /// Represents a transition between states in a state machine.
    /// </summary>
    public class Transition : ITransition
    {
        /// <summary>
        /// Name of the target state.
        /// </summary>
        public string ToName { get; }
        /// <summary>
        /// Property to indicate if the transition should be forced.
        /// </summary>
        public bool ForceTransition { get; }
        /// <summary>
        /// The condition that must be met for the transition to occur.
        /// </summary>
        public IPredicate Condition { get; private set; }

        public Transition(string to, IPredicate condition, bool forceTransition = false)
        {
            ToName = to;
            Condition = condition;
            ForceTransition = forceTransition;
        }
        
        /// <summary>
        /// Dispose method that calls the Dispose method of the Condition property and sets it to null.
        /// </summary>
        public void Dispose()
        {
            Condition.Dispose();
            Condition = null;
        }
    }
}