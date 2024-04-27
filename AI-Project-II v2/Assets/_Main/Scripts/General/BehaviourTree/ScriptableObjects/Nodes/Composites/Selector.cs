using System;
using BehaviourTreeAsset.Interfaces;

namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class Selector : Composite
    {
        private int _index;
        private INode _currentChild;

        protected override void OnStart()
        {
            _index = 0;
        }

        protected override NodeState OnUpdate()
        {
            _currentChild = GetChild(_index);

            switch (_currentChild.DoUpdate())
            {
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Failure:
                    _index++;
                    return _index > GetChildCount() - 1 ? NodeState.Failure : NodeState.Running;
                case NodeState.Success:
                    return NodeState.Success;
                case NodeState.Disable:
                    _index++;
                    return _index > GetChildCount() - 1 ? NodeState.Success : NodeState.Running;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnNodeDestroy()
        {
            _currentChild = null;
        }
    }
}