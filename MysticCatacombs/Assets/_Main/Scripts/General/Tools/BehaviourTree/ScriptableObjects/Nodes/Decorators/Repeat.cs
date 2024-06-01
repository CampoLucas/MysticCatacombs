namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class Repeat : Decorator
    {
        public int amount = 1;
        public bool infinite;
        public bool stopOnFailure;

        private int _count;
        private NodeState _prevNodeState;

        protected override void OnStart()
        {
            _count = 0;
            _prevNodeState = NodeState.Success;
        }

        protected override NodeState OnUpdate()
        {
            //if (!infinite && amount <= 0) return State.Success;
            if (!infinite && _prevNodeState == NodeState.Success)
            {
                if (_count >= amount) return NodeState.Success;
                _count++;
            }
            
            _prevNodeState = GetChild().DoUpdate();

            if (stopOnFailure && _prevNodeState == NodeState.Failure) 
                return NodeState.Failure;
            
            return NodeState.Running;
        }
    }
}