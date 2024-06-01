namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class ReturnSuccess : Decorator
    {
        protected override NodeState OnUpdate()
        {
            GetChild().DoUpdate();
            return NodeState.Success;
        }
    }
}