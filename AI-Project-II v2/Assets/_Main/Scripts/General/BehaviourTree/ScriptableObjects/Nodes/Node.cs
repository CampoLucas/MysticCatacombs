using System;
using System.Collections.Generic;
using System.Linq;
using BehaviourTreeAsset.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace BehaviourTreeAsset.Runtime
{
    public abstract class Node : ScriptableObject, INode
    {
        #region Node Properties

        public NodeState CurrentState { get; private set; } = NodeState.Running;
        public bool Started { get; private set; }
        public bool Enabled { get; private set; }
        public List<Node> Children => children;

        #endregion

        #region View Properties

        public string Name { get => name; set => name = value; }
        public string Description { get; private set; }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                previousPosition = position;
                position = value;
                if (position != previousPosition)
                    OnPositionChange?.Invoke(position);
            }
        }

        public Action<Vector2> OnPositionChange;
        
        [HideInInspector] public string guid;

        #endregion
        
        // protected Node variables
        protected Transform Transform;
        protected GameObject Owner;

        // private node variables
        [HideInInspector] [SerializeField] private List<Node> children = new(); //ToDo: make method that sets the list depending of the ChildAmount().
        private int _childIndex;
        
        // private View variables
        [HideInInspector] [SerializeField] private Vector2 position;
        [HideInInspector] [SerializeField] private Vector2 previousPosition;

        #region Public methods
        
        public Node GetChild()
        {
            switch (ChildCapacity())
            {
                case 0:
                    return null;
                case 1:
                    return children[0];
                default:
                    var child = children[_childIndex];
                    if (_childIndex + 1 > children.Count - 1) _childIndex = 0;
                    else _childIndex++;
                    return child;
            }
        }

        public void SetChildren(List<Node> newChildren)
        {
            children = newChildren;
        }

        public int GetChildCount() => children.Count;
        
        /// <summary>
        /// The amount of child nodes a node can have.
        /// -1 means unlimited.
        /// </summary>
        public virtual int ChildCapacity() => 0;

        public Node GetChild(int index)
        {
            return children[index];
        }

        public bool AddChild(Node node)
        {
            switch (ChildCapacity())
            {
                case 0:
                    return false;
                case < 0:
                    return Add(node);
                default:
                    if (children.Count < ChildCapacity())
                        return Add(node);
                    return false;
            }
        }

        public bool RemoveChild(Node node)
        {
            return Remove(node);
        }

        public bool ContainsChild(Node node)
        {
            return Children.Contains(node);
        }

        public bool ContainsChildInChildren(Node node)
        {
            // var result = false;
            // for (var i = 0; i < _children.Count; i++)
            // {
            //     var child = _children[i];
            //     if (child.GetChildCount() == 0) continue;
            //
            //     if (child.ContainsChild(node))
            //     {
            //         result = true;
            //         break;
            //     }
            //
            //     if (child.ContainsChildInChildren(node))
            //     {
            //         result = true;
            //         break;
            //     }
            // }

            // If current node contains the node returns true.
            if (ContainsChild(node))
                return true;

            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child.ContainsChildInChildren(node))
                    return true;
            }
            

            // If no node is found returns false.
            return false;
        }

        public void ArrangeChildren(Vector2 newPos)
        {
            Debug.Log("Arrange");
            children = children.OrderBy(go => go.Position.x).ToList();
        }
        
        /// <summary>
        /// Determines if the node is a root node.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsRoot() => false;
        
        public virtual string NodeName() => GetType().Name;

        public virtual Node Clone()
        {
            var n = Instantiate(this);
            n.SetChildren(children.ConvertAll(c => c.Clone()));
            
            return n;
        }

        #endregion
        
        #region Public execution methods

        /// <summary>
        /// Assigns the nodes references and calls the OnAwake method.
        /// </summary>
        /// <param name="owner"></param>
        public void DoAwake(GameObject owner)
        {
            if (owner == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"BehaviourTreeAsset: The Owner GameObject in the DoAwake is null.");
#endif
                return;          
            }
            
            Owner = owner;
            Transform = Owner.transform;
            OnAwake();

            for (var i = 0; i < children.Count; i++)
            {
                var child = GetChild();
                if (child != null) child.DoAwake(owner);
            }
        }

        /// <summary>
        /// Updates the node's logic.
        /// </summary>
        /// <returns></returns>
        public NodeState DoUpdate()
        {
            if (CurrentState == NodeState.Disable)
            {
                return NodeState.Disable;
            }
            
            if (!Started)
            {
                OnStart();
                Started = true;
            }

            CurrentState = OnUpdate();

            if (CurrentState is NodeState.Failure or NodeState.Success)
            {
                OnStop();
                Started = false;
            }

            return CurrentState;
        }

        /// <summary>
        /// Disables the node if it is enable.
        /// </summary>
        public void Disable()
        {
            if (!Enabled) return;
            
            CurrentState = NodeState.Disable;
            OnNodeDisable();
            Enabled = false;
        }

        /// <summary>
        /// Enables the node if it was disabled.
        /// </summary>
        public void Enable()
        {
            if (Enabled) return;
            
            CurrentState = NodeState.Running;
            OnNodeEnable();
            Enabled = true;
        }

        /// <summary>
        /// Destroys the node.
        /// </summary>
        public void Destroy()
        {
            OnNodeDestroy();

            UnsubscribeToAllPositionChange();
            
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child != null)
                    child.Destroy();
            }

            children.Clear();
            children = null;
            
            Transform = null;
            Owner = null;
        }
        
        public void OnPopulateTree()
        {
            UnsubscribeToAllPositionChange();
            SubscribeToAllPositionChange();
        }

        #endregion
        
        #region Protected execution methods

        /// <summary>
        /// Called in the awake of the Behaviour.
        /// </summary>
        protected virtual void OnAwake() {}
        
        /// <summary>
        /// Called when the node begins executing its logic.
        /// </summary>
        protected virtual void OnStart() {}
        
        /// <summary>
        /// Called when the node finishes executing its logic.
        /// </summary>
        protected virtual void OnStop() {}
        
        /// <summary>
        /// The update loop of the node.
        /// It finishes executing when the state returns success or failure.
        /// </summary>
        /// <returns> The state of the node </returns>
        protected virtual NodeState OnUpdate() => NodeState.Success;
        
        /// <summary>
        /// Called when the node is disabled.
        /// </summary>
        protected virtual void OnNodeDisable() {}
        
        /// <summary>
        /// Called when the node is enabled.
        /// </summary>
        protected virtual void OnNodeEnable() {}
        
        /// <summary>
        /// Called when the node is destroyed.
        /// </summary>
        protected virtual void OnNodeDestroy() {}

        protected GameObject GetDefault(GameObject target)
        {
            return target ? target : Owner;
        }

        #endregion

        #region Private Methods

        private bool Add(Node node)
        {
            if (children.Contains(node)) return false;
            node.OnPositionChange += ArrangeChildren;
            children.Add(node);
            
            ArrangeChildren(Vector2.zero);
            return true;
        }

        private bool Remove(Node node)
        {
            if (!children.Contains(node)) return false;
            node.OnPositionChange -= ArrangeChildren;
            children.Remove(node);

            ArrangeChildren(Vector2.zero);
            return true;
        }

        private void SubscribeToAllPositionChange()
        {
            if (children.Count <= 0) return;
            for (var i = 0; i < children.Count; i++)
            {
                children[i].OnPositionChange += ArrangeChildren;
            }
        }
        
        private void UnsubscribeToAllPositionChange()
        {
            if (children.Count <= 0) return;
            for (var i = 0; i < children.Count; i++)
            {
                children[i].OnPositionChange -= ArrangeChildren;
            }
        }

        #endregion
    }
}
