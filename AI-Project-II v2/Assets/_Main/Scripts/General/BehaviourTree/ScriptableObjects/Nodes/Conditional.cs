namespace BehaviourTreeAsset.Runtime
{
    public abstract class Conditional : Node
    {
        public sealed override int ChildCapacity() => 0;
        public sealed override bool IsRoot() => false;
    }
}