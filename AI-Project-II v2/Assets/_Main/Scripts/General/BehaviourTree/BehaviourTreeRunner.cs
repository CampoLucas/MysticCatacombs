using BehaviourTreeAsset.Interfaces;
using BehaviourTreeAsset.Runtime.Nodes;
using Unity.VisualScripting;
using UnityEngine;

namespace BehaviourTreeAsset.Runtime
{
    public class BehaviourTreeRunner : MonoBehaviour, IBehaviourRunner
    {
        public BehaviourTree Tree => tree;
        [SerializeField] private BehaviourTree tree;

        private void Awake()
        {
            tree = tree.Clone();
            
            tree.DoAwake(gameObject);
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            tree.DoUpdate();
        }

        private void OnDestroy()
        {
            tree.Destroy();
        }
    }
}