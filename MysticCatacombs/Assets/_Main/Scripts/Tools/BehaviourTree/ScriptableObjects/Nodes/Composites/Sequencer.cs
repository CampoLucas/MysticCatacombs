using System;
using BehaviourTreeAsset.Interfaces;

namespace BehaviourTreeAsset.Runtime.Nodes
{
    public class Sequencer : Composite
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
                    return NodeState.Failure;
                case NodeState.Success or NodeState.Disable:
                    _index++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _index == GetChildCount() ? NodeState.Success : NodeState.Running;
        }

        protected override void OnNodeDestroy()
        {
            _currentChild = null;
        }
    }
}