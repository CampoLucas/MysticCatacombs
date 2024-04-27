using BehaviourTreeAsset.Runtime;
using UnityEngine;

namespace BehaviourTreeAsset.Interfaces
{
    public interface IBehaviour
    {
        Node RootNode { get; }
        NodeState CurrentState { get; }
        GameObject Owner { get; }

        void DoAwake(GameObject owner);
        NodeState DoUpdate();
        void Destroy();
        void SetRootNode(Node newNode);
    }
}