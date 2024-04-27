using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeAsset.Runtime
{
    public sealed class Root : Node
    {
        public override int ChildCapacity() => 1;
        public override bool IsRoot() => true;

        protected override NodeState OnUpdate()
        {
            return GetChild().DoUpdate();
        }
    }
}
