using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CustomCollider
{
    public class BoxCastTrigger : BoxTrigger
    {
        private RaycastHit[] _hits = new RaycastHit[2];

        protected override void Detect()
        {
            base.Detect();
            CachedHits = Physics.BoxCastNonAlloc(Offset, Size/2, MyTransform.forward, _hits, MyTransform.localRotation, Mathf.Infinity, damageLayerMask);
            CachedNewColliders ??= new List<Collider>();

            // Check for new colliders
            for (var i = 0; i < CachedHits; i++)
            {
                var hit = _hits[i].collider;
                if (hit != null && !CollidersInBox.Contains(hit))
                {
                    if (!hit.enabled) continue;
                    OnEnter(hit);
                    CachedNewColliders.Add(hit);
                }
                else if (hit != null)
                {
                    if (!hit.enabled) continue;
                    OnStay(hit);
                }
            }

            // Check for removed colliders
            for (var i = CollidersInBox.Count - 1; i >= 0; i--)
            {
                var other = CollidersInBox[i];
                if (!Array.Exists(_hits, element => element.collider == other))
                {
                    if (!other.enabled) continue;
                    OnExit(other);
                    CollidersInBox.RemoveAt(i);
                }
            }

            // Update _collidersInBox list
            CollidersInBox.AddRange(CachedNewColliders);
            CachedNewColliders.Clear();
        }

        public override void EnableCollider()
        {
            base.EnableCollider();
            _hits = new RaycastHit[2];
        }

        public override void DisableCollider()
        {
            base.DisableCollider();
            foreach (var other in CollidersInBox)
            {
                OnExit?.Invoke(other);
            }
            CollidersInBox.Clear();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _hits = null;
        }
    }
}