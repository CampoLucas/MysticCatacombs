using System;
using System.Collections.Generic;
using BehaviourTreeAsset.Runtime;
using UnityEngine;

namespace BehaviourTreeAsset.Interfaces
{
    public interface INode
    {
        NodeState CurrentState { get; }
        bool Started { get; }
        bool Enabled { get; }
        string Name { get; }
        string Description { get; }
        Vector2 Position { get; }
        List<Node> Children { get; }

        void DoAwake(GameObject owner);
        NodeState DoUpdate();
        void Enable();
        void Disable();
        void Destroy();

        Node GetChild();
        Node GetChild(int index);
        int GetChildCount();
        int ChildCapacity();
        bool AddChild(Node node);
        bool RemoveChild(Node node);
        bool ContainsChild(Node node);
        bool ContainsChildInChildren(Node node);
    }
    
    /*
     * Node Ideas:
     * SubBehaviour : Action, IBehaviourRunner
     * NodeReference : Action
     * Resetable : Decorator
     * DoOnce : Resetable
     * DoNTimes : Resetable
     * ReturnFailure : Decorator
     * ReturnSuccess : Decorator
     */
}