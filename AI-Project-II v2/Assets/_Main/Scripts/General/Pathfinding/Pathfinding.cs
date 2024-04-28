#if true
//#define GIZMOS
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Game.Pathfinding
{
    [System.Serializable]
    public class Pathfinding : IDisposable
    {
        public List<Vector3> Waypoints { get; private set; } = new();
        public int CurrentIndex { get; private set; }
        public Transform Target => target;
        public bool Enabled { get; private set; }

        [Header("Target to Follow")]
        [SerializeField] private Transform target;

        [Header("Settings")] 
        [SerializeField] private float entityRadius;
        [SerializeField] private float radiusToCheck;
        [SerializeField] private float closestNodeRange;
        [SerializeField] private float minDistanceToReachNode;

        [Header("Detection Settings")]
        [SerializeField] private LayerMask nodeLayer;
        [FormerlySerializedAs("unWalkableLayer")] [SerializeField] private LayerMask unwalkableLayer;

#if true
        [Header("Gizmos")] 
        [SerializeField] private bool showGizmos = true;
        [SerializeField] private bool onSelected;
        
        [Header("Path Gizmos")]
        [SerializeField] private Color lineColor = Color.yellow;
        [SerializeField] private Color startPointColor = Color.green;
        [SerializeField] private Color endPointColor = Color.red;
        [SerializeField] private Color nextPointColor = Color.cyan;
        [SerializeField] private Color currentPointColor = Color.magenta;

        [Header("Detection Gizmos")] 
        [SerializeField] private Color entityRadiusColor = Color.yellow;
        [SerializeField] private Color radiusToCheckColor = Color.red;
        [SerializeField] private Color closestNodeColor = Color.blue;
        
#endif

        private Transform _origin;
        private AStar<Node> _aStar = new();
        private Node _startNode;
        private Node _endNode;
        private NodeGrid _grid;
        private Vector3 _targetPos;
        private Vector3 _currentPoint;
        private Vector3 _startPoint;
        private Vector3 _endPoint;

        /// <summary>
        /// Initializes the pathfinder.
        /// </summary>
        public void InitPathfinder(Transform origin)
        {
            _origin = origin;
            _grid = NodeGrid.GetInstance();
            if (!_grid)
            {
                Debug.LogWarning("Pathfinding: not enabled");
                Enabled = false;
            }
            else
            {
                Enabled = true;
            }
            
            if (!Enabled) return;
            if (Target) SetTarget(Target);
        }

        /// <summary>
        /// Returns true if can use pathfinding.
        /// </summary>
        /// <returns></returns>
        public bool CanRunPathfinder() => Enabled && _startNode && _endNode;

        /// <summary>
        /// Sets a new target.
        /// </summary>
        /// <param name="newTarget"></param>
        public void SetTarget(Transform newTarget)
        {
            if (target && newTarget != target) target = newTarget;
        }
        
        /// <summary>
        /// Removes the target for a position.
        /// </summary>
        /// <param name="pos"></param>
        public void SetTarget(Vector3 pos)
        {
            target = null;
            _targetPos = pos;
        }

        /// <summary>
        /// Gets the target position.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetTargetPos()
        {
            return target ? target.position : _targetPos;
        }

        /// <summary>
        /// Returns true if the target distance is to far from the last node.
        /// </summary>
        /// <returns></returns>
        public bool IsNearPath()
        {
            if (!Enabled || !_endNode) return false;

            var endPos = _endPoint;
            var targetPos = GetTargetPos();
            endPos.y = targetPos.y;
            return Vector3.Distance(targetPos, endPos) <= radiusToCheck;
        }

        /// <summary>
        /// Runs the pathfinding algorithm and saves a path
        /// </summary>
        public void Run()
        {
            if (!Enabled || !_startNode || !_endNode)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Pathfinding: Couldn't run the pathfinding algorithm, is enable: {Enabled}, has startNode: {_startNode}, has endNode: {_endNode}");
#endif
                return;
            }

            var path = new List<Vector3>();
            _endPoint = GetTargetPos();
            _startPoint = _origin.position;
            path.Add(_startPoint);
            
            if (!InView(_startPoint, _endPoint))
            {
#if UNITY_EDITOR
                Debug.Log("Pathfinding: Run A*.");
#endif
                var nodePath = _aStar.Run(_startNode, Satisfies, GetConnections, GetCost, Heuristic);
                for (var i = 0; i < nodePath.Count; i++)
                {
                    path.Add(nodePath[i].Transform.position);
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log($"Pathfinding: No obstructions in view.");
#endif
            }
            path.Add(_endPoint);
            
            AStar<Node>.CleanUpPath(path, InView, out path);

            Waypoints = path;
            CurrentIndex = 0;
            _startPoint = Waypoints[0];
            _endPoint = Waypoints[Waypoints.Count - 1];
        }

        /// <summary>
        /// Sets the start and end nodes.
        /// </summary>
        /// <returns></returns>
        public bool SetNodes()
        {
            return SetNode(GetTargetPos(), ref _endNode) && SetNode(_origin.position, ref _startNode);
        }

        public Vector3 GetWaypoint()
        {
            if (!Enabled)
            {
#if UNITY_EDITOR
                Debug.LogError("Pathfinding: Can't get waypoint.");
#endif
                return Vector3.zero;
            }

            // If the target is not near the path, it recalculates.
            if (!IsNearPath())
            {
                SetNodes();
                Run();
            }

            _currentPoint = GetCurrentWaypoint();
            return _currentPoint;
        }

        private Vector3 GetCurrentWaypoint()
        {
            if (Vector3.Distance(_origin.position, Waypoints[CurrentIndex]) < minDistanceToReachNode)
            {
                SetNextNode();
            }

            return Waypoints[CurrentIndex];
        }

        private void SetNextNode()
        {
            if (CurrentIndex >= Waypoints.Count - 1) return;
            CurrentIndex++;
        }

        private bool SetNode(Vector3 origin, ref Node node)
        {
            return Enabled && _grid.GetClosestNode(origin, closestNodeRange, out node, nodeLayer);
        }

        private bool InView(Vector3 a, Vector3 b)
        {
            var dir = (b - a).normalized;
            var distance = Vector3.Distance(a, b);

            return !Physics.SphereCast(a, entityRadius, dir, out var hit, distance, unwalkableLayer);
        }

        private bool Satisfies(Node curr)
        {
            return curr == _endNode;
        }
        
        private List<Node> GetConnections(Node curr)
        {
            return curr.Neightbourds;
        }
        
        private float Heuristic(Node curr)
        {
            const float multiplierDistance = 2f;
            const float multiplierTrap = 20f;
            var cost = 0f;

            cost += Vector3.Distance(curr.transform.position, _endNode.transform.position) * multiplierDistance;

            if (curr.IsTrap)
                cost *= multiplierTrap;
            if (_endNode.IsTrap)
                cost *= multiplierTrap;
            
            
            return cost;
        }

        private float GetCost(Node parent, Node son)
        {
            const float multiplierDistance = 1f;
            const float multiplierTrap = 20f;
            var cost = 0f;

            cost += Vector3.Distance(parent.transform.position, son.transform.position) * multiplierDistance;
            if (son.IsTrap)
                cost *= multiplierTrap;

            return cost;
        }
        
        public void Dispose()
        {
            Waypoints = null;
            target = null;
        }

        [Conditional("UNITY_EDITOR")]
        public void OnDrawGizmos()
        {
            if (!showGizmos || onSelected) return;
            Draw();
        }

        [Conditional("UNITY_EDITOR")]
        public void OnDrawGizmosSelected()
        {
            if (!showGizmos || !onSelected) return;
            Draw();
        }

#if true

        private void Draw()
        {
            DrawEntityRadius();
            DrawClosestNodeRadius();
            
            if (Waypoints == null || Waypoints.Count < 0) return;
            
            DrawStartPoint();
            DrawEndPoint();
            DrawCurrentPoint();
            DrawPath();
            DrawRadiusToCheck();
        }
        
        private void DrawPath()
        {
            var totalWaypoints = Waypoints.Count;
            for (var i = 0; i < totalWaypoints; i++)
            {
                Debug.Log($"path {i}");
                if (i + 1 > totalWaypoints - 1) continue;
                Debug.Log($"path 2 {i}");
                
                var curr = Waypoints[i];
                var next = Waypoints[i + 1];
                Gizmos.color = lineColor;
                Gizmos.DrawLine(curr, next);
            }
            
        }

        private void DrawStartPoint()
        {
            if (!_startNode) return;
            
            Gizmos.color = startPointColor;
            Gizmos.DrawSphere(_startPoint, 0.1f);
        }

        private void DrawEndPoint()
        {
            if (!_endNode) return;
            
            Gizmos.color = endPointColor;
            Gizmos.DrawSphere(_endPoint, 0.1f);
        }

        private void DrawCurrentPoint()
        {
            Gizmos.color = currentPointColor;
            Gizmos.DrawSphere(_currentPoint, 0.2f);
        }

        private void DrawEntityRadius()
        {
            if (!_origin) return;
            Gizmos.color = entityRadiusColor;
            Gizmos.DrawWireSphere(_origin.position, entityRadius);
        }

        private void DrawRadiusToCheck()
        {
            Gizmos.color = radiusToCheckColor;
            Gizmos.DrawWireSphere(_endPoint, radiusToCheck);
        }

        private void DrawClosestNodeRadius()
        {
            if (!_origin) return;
            Gizmos.color = closestNodeColor;
            Gizmos.DrawWireSphere(_origin.position, closestNodeRange);
        }
#endif
        
    }
}