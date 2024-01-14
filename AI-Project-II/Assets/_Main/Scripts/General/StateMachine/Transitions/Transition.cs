namespace Game.StateMachine
{
    public class Transition : ITransition
    {
        public string ToName { get; private set; }
        public IPredicate Condition { get; private set; }

        public Transition(string to, IPredicate condition)
        {
            ToName = to;
            Condition = condition;
        }

        public void Dispose()
        {
            Condition.Dispose();
            Condition = null;
        }
    }
}