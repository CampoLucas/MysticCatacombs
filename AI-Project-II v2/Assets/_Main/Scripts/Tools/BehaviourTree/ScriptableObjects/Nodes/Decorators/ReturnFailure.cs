namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class ReturnFailure : Decorator
    {
        protected override NodeState OnUpdate()
        {
            GetChild().DoUpdate();
            return NodeState.Failure;
        }
    }
}