using BehaviourTreeAsset.Runtime;

namespace BehaviourTreeAsset.Interfaces
{
    public interface IBehaviourRunner
    {
        BehaviourTree Tree { get; }
    }
}