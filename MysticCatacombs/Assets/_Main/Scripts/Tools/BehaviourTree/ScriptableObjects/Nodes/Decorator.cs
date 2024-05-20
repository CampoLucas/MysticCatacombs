namespace BehaviourTreeAsset.Runtime
{
    public abstract class Decorator : Node
    {
        public sealed override int ChildCapacity() => 1;
        public sealed override bool IsRoot() => false;
    }
}