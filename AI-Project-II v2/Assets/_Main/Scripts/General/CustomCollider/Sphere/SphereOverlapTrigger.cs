using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Game.CustomCollider
{
    public class SphereOverlapTrigger : SphereTrigger
    {
        private Collider[] _hits = new Collider[2];

        protected override void Detect()
        {
            base.Detect();
            CachedHits = Physics.OverlapSphereNonAlloc(Offset, Radius, _hits, damageLayerMask);
            CachedNewColliders ??= new List<Collider>();

            // Check for new colliders
            for (var i = 0; i < CachedHits; i++)
            {
                var hit = _hits[i];
                if (hit != null && !CollidersInBox.Contains(hit) && hit.enabled)
                {
                    OnEnter(hit);
                    CachedNewColliders.Add(hit);
                }
                else if (hit != null && hit.enabled)
                {
                    OnStay(hit);
                }
            }

            // Check for removed colliders
            for (var i = CollidersInBox.Count - 1; i >= 0; i--)
            {
                var other = CollidersInBox[i];
                if (!Array.Exists(_hits, element => element == other)  && other.enabled)
                {
                    OnExit(other);
                    CachedNewColliders.RemoveAt(i);
                }
            }

            // Update _collidersInBox list
            CollidersInBox.Clear();
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