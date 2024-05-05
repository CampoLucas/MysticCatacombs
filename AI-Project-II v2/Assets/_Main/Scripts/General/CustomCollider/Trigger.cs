using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.CustomCollider
{
    public class Trigger : MonoBehaviour
    {
         public Transform MyTransform { get; private set; }

        public bool Enabled
        {
            get => _enabled;
            private set => _enabled = value;
        }
        
        public Vector3 Offset
        {
            get
            {
                var tr = (MyTransform ? MyTransform : transform);
                var pos = tr.localPosition + offset;
                
                return tr.TransformPoint(pos);
            }
        }

        public Action<Collider> OnEnter = delegate {  };
        public Action<Collider> OnStay = delegate {  };
        public Action<Collider> OnExit = delegate {  };
        
        [Header("Detection Settings")]
        [SerializeField] protected LayerMask damageLayerMask;
        
        [Header("Size & Scale")]
        [SerializeField] protected Vector3 offset;
        
        protected int CachedHits;
        protected List<Collider> CollidersInBox = new();
        protected List<Collider> CachedNewColliders;
        
        private bool _enabled;

        private void Awake()
        {
            MyTransform = transform;
        }

        private void Update()
        {
            if (Enabled)
            {
                Detect();
            }
        }

        protected virtual void Detect() { }

        public virtual void EnableCollider()
        {
            Enabled = true;
            CollidersInBox.Clear();
        }
        
        public virtual void DisableCollider()
        {
            Enabled = false;
            for (var i = 0; i < CollidersInBox.Count; i++)
            {
                OnExit(CollidersInBox[i]);
            }
            CollidersInBox.Clear();
        }

        protected virtual void OnDrawGizmos()
        {
            var color = Color.green;
            if (!Enabled)
                color = new Color(0.4f, 0, 0, 1);
            if (CollidersInBox.Count > 0)
                color = new Color(0.5f, 0, 1, 1);
            Gizmos.color = color;
            
        }

        protected virtual void OnDestroy()
        {
            MyTransform = null;
            if (OnEnter != null)
            {
                var subscribers = OnEnter.GetInvocationList();
                for (var i = 0; i < subscribers.Length; i++)
                {
                    OnEnter -= (Action<Collider>)subscribers[i];
                }
            }
            
            if (OnExit != null)
            {
                var subscribers = OnExit.GetInvocationList();
                for (var i = 0; i < subscribers.Length; i++)
                {
                    OnExit -= (Action<Collider>)subscribers[i];
                }
            }
            
            if (OnStay != null)
            {
                var subscribers = OnStay.GetInvocationList();
                for (var i = 0; i < subscribers.Length; i++)
                {
                    OnStay -= (Action<Collider>)subscribers[i];
                }
            }

            if (CollidersInBox != null)
            {
                CollidersInBox.Clear();
                CollidersInBox = null;
            }

            if (CachedNewColliders != null)
            {
                CachedNewColliders.Clear();
                CachedNewColliders = null;
            }
        }
    }
}