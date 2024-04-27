using System.Collections.Generic;
using BehaviourTreeAsset.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BehaviourTreeAsset.Runtime
{
    [CreateAssetMenu()]

    public class BehaviourTree : ScriptableObject, IBehaviour
    {
        public Node RootNode => rootNode;
        public NodeState CurrentState { get; private set; } = NodeState.Running;
        public GameObject Owner { get; private set; }
        public List<Node> Nodes => nodes;

        [HideInInspector] [SerializeField] private Node rootNode;
        [SerializeField] private List<Node> nodes = new();
        
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
            RootNode.DoAwake(Owner);
        }

        public NodeState DoUpdate()
        {
            if (RootNode.CurrentState == NodeState.Running)
            {
                CurrentState = RootNode.DoUpdate();
            }

            return CurrentState;
        }

        public void Destroy()
        {
            RootNode.Destroy();
            rootNode = null;
            Owner = null;
        }

        public void SetRootNode(Node newNode)
        {
            rootNode = newNode;
        }

        public Node CreateNode(System.Type type)
        {
            var node = CreateInstance(type) as Node;
            if (node != null)
            {
                node.Name = node.NodeName();
#if UNITY_EDITOR
                node.guid = GUID.Generate().ToString();
#endif
                nodes.Add(node);
                
#if UNITY_EDITOR
                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();
#endif
            }

            return node;
        }

        public void DeleteNode(Node node)
        {
            node.Destroy();
            Nodes.Remove(node);
            
#if UNITY_EDITOR
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
#endif
            
        }

        public bool AddChild(Node parent, Node child)
        {
            return parent.AddChild(child);
        }

        public bool RemoveChild(Node parent, Node child)
        {
            return parent.RemoveChild(child);
        }

        public List<Node> GetChildren(Node parent)
        {
            return parent.Children;
        }

        public BehaviourTree Clone()
        {
            var t = Instantiate(this);
            t.rootNode = t.rootNode.Clone();

            return t;
        }

        public void OnPopulateTree()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].OnPopulateTree();
            }
        }
    }
}
