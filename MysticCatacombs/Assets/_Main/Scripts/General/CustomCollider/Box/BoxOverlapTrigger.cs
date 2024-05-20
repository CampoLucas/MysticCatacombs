using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CustomCollider
{
    public class BoxOverlapTrigger : BoxTrigger
    {
        private Collider[] _hits = new Collider[2];

        protected override void Detect()
        {
            base.Detect();
            CachedHits = Physics.OverlapBoxNonAlloc(Offset, Size/2, _hits, MyTransform.rotation, damageLayerMask);
            CachedNewColliders ??= new List<Collider>();

            // Check for new colliders
            for (var i = 0; i < CachedHits; i++)
            {
                var hit = _hits[i];
                if (hit == null) continue;
                if (hit.enabled == false) continue;
                if (hit != null && !CollidersInBox.Contains(hit) && hit.enabled)
                {
                    if (hit != null)
                    {
                        if (OnEnter != null) OnEnter?.Invoke(hit);
                        CachedNewColliders.Add(hit);
                    }
                }
                else if (hit != null && hit.enabled)
                {
                    if (OnStay != null)
                        OnStay?.Invoke(hit);
                }
            }

            // Check for removed colliders
            for (var i = CollidersInBox.Count - 1; i >= 0; i--)
            {
                var other = CollidersInBox[i];
                if (!other.enabled)
                {
                    CollidersInBox.RemoveAt(i);
                    continue;
                }
                
                if (!Array.Exists(_hits, element => element == other))
                {
                    if (OnExit != null)
                        OnExit?.Invoke(other);
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
            _hits = new Collider[2];
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _hits = null;
        }
    }
}