using Game.Pathfinding;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class SeekPathfinder : Seek
    {
        private Pathfinder pathfinder;
        
        public SeekPathfinder(Transform origin, float strength, Pathfinder pathfinder) : base(origin, strength)
        {
            this.pathfinder = pathfinder;
        }

        protected override Vector3 CalculateDir(Transform target)
        {
            pathfinder.SetTarget(target);
            var point = pathfinder.CalculateWaypoint();
            var originPos = Origin.position;
            point.y = originPos.y;
             
            return (point - originPos).normalized * Strength;
        }
        
        protected override Vector3 CalculateDir(Vector3 position)
        {
            pathfinder.SetTarget(position);
            var point = pathfinder.CalculateWaypoint();
            var originPos = Origin.position;
            point.y = originPos.y;

            return (point - originPos).normalized * Strength;
        }

        public override void Dispose()
        {
            base.Dispose();
            pathfinder = null;
        }
    }
}