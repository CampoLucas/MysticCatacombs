using UnityEngine;

namespace Game.Entities.Steering.Testing
{
    public class SeekPathfinder : Seek
    {
        private Pathfinding.Pathfinding _pathfinding;
        
        public SeekPathfinder(Transform origin, float strength, Pathfinding.Pathfinding pathfinding) : base(origin, strength)
        {
            _pathfinding = pathfinding;
        }

        public override Vector3 GetDir(Transform target)
        {
            Debug.Log("SeekPathfinder GetDir target");
            _pathfinding.SetTarget(target);
            //_pathfinding.SetNodes();
            var point = _pathfinding.CalculateWaypoint();
            Debug.Log($"point is {point}");

            return (point - Origin.position).normalized * Strength;
        }

        public override Vector3 GetDir(Vector3 position)
        {
            _pathfinding.SetTarget(position);
            var point = _pathfinding.CalculateWaypoint();

            return (point - Origin.position).normalized * Strength;
        }

        public override void Dispose()
        {
            base.Dispose();
            _pathfinding = null;
        }
    }
}