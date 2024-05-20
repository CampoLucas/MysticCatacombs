using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Game.Pathfinding
{
    /// <summary>
    /// Runs a algorithm to find a path.
    /// </summary>
    [Serializable]
    public class Pathfinder : IDisposable
    {
        /// <summary>
        /// The resulting path of the algorithm.
        /// </summary>
        public List<Vector3> Waypoints { get; private set; } = new();
        
        /// <summary>
        /// The index of the current point in the list of Waypoints.
        /// </summary>
        public int CurrentIndex { get; private set; }
        
        /// <summary>
        /// Reference to the target.
        /// </summary>
        public Transform Target => target;
        /// <summary>
        /// To know if the pathfinding is enabled.
        /// </summary>
        public bool Enabled { get; private set; }
        
        /// <summary>
        /// Event for when the object using pathfinding reached their point.
        /// </summary>
        public Action OnPointReached;
        
        /// <summary>
        /// Event for when the object using pathfinding reached the end of the path.
        /// </summary>
        public Action OnEndOfPathReached;
        
        /// <summary>
        /// Event for when the target is set to a new one..
        /// </summary>
        public Action<Transform> OnTargetChanged;
        
        /// <summary>
        /// Event for when the target position is set to a new one.
        /// </summary>
        public Action<Vector3> OnTargetPosChanged;
        
        /// <summary>
        /// Event for when the enable state changes.
        /// </summary>
        public Action<bool> OnSetEnabled;
        
        /// <summary>
        /// Event for when the pathfinding algorithm fishes calculating a path.
        /// </summary>
        public Action<bool> OnPathCalculated;

        [Header("Target to Follow")]
        [SerializeField] private Transform target;

        [Header("Settings")] 
        [SerializeField] private float entityRadius;
        [SerializeField] private float radiusToCheck;
        [SerializeField] private float closestNodeRange;
        [SerializeField] private float minDistanceToReachNode;
        [SerializeField] private bool runAsynchronously;

        [Header("Detection Settings")]
        [SerializeField] private LayerMask nodeLayer;
        [FormerlySerializedAs("unWalkableLayer")] [SerializeField] private LayerMask unwalkableLayer;

#if UNITY_EDITOR
        [Header("Gizmos")] 
        [SerializeField] private bool showGizmos = true;
        [SerializeField] private bool onSelected;

        [Header("Path Gizmos")]
        [SerializeField] private bool showPath = true;
        [SerializeField] private Color lineColor = Color.yellow;
        [SerializeField] private Color startPointColor = Color.green;
        [SerializeField] private Color endPointColor = Color.red;
        [SerializeField] private Color nextPointColor = Color.cyan;
        [SerializeField] private Color currentPointColor = Color.magenta;

        [Header("Detection Gizmos")] 
        [SerializeField] private bool showDetection = true;
        [SerializeField] private Color entityRadiusColor = Color.yellow;
        [SerializeField] private Color radiusToCheckColor = Color.red;
        [SerializeField] private Color closestNodeColor = Color.blue;
#endif

        private Transform _origin;
        private AStar<Node> _aStar;
        private Node _startNode;
        private Node _endNode;
        private NodeGrid _grid;
        private Vector3 _targetPos;
        private Vector3 _currentPoint;
        private Vector3 _startPoint;
        private Vector3 _endPoint;
        private bool _prevEnableState;

        #region Public Methods

        /// <summary>
        /// Initializes the pathfinder.
        /// </summary>
        public void InitPathfinder(Transform origin)
        {
            _origin = origin;
            _aStar = new AStar<Node>(OnPathCalculatedHandler);
            _grid = NodeGrid.GetInstance();
            if (!_grid || _aStar == null)
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
        /// Enables or disables the pathfinding.
        /// If there is no node grid it will forcefully disable it.
        /// </summary>
        /// <param name="value"></param>
        public void SetEnable(bool value)
        {
            var v = false;
            if (value)
            {
                _grid = NodeGrid.GetInstance();
                v = _grid && _aStar != null;
            }

            _prevEnableState = Enabled;
            Enabled = v;
            OnSetEnabledHandler(v);
        }

        /// <summary>
        /// Returns true if the Waypoints list is not empty and if it's length is grater than 0.
        /// </summary>
        /// <returns></returns>
        public bool HasPath() => Waypoints is { Count: > 0 };

        /// <summary>
        /// Returns true if can use pathfinding.
        /// </summary>
        /// <returns></returns>
        public bool CanRunPathfinder() => Enabled && _startNode && _endNode;

        /// <summary>
        /// Sets a new target.
        /// </summary>
        /// <param name="newTarget"></param>
        public bool SetTarget(Transform newTarget)
        {
            if (target && newTarget != target)
            {
                target = newTarget;
                OnTargetChangedHandler(newTarget);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Removes the target for a position.
        /// </summary>
        /// <param name="pos"></param>
        public bool SetTarget(Vector3 pos)
        {
            if (_targetPos != pos)
            {
                target = null;
                _targetPos = pos;
                OnTargetPosChangedHandler(pos);
                return true;
            }
                
            return false;
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
        /// Runs the pathfinding algorithm asynchronously.
        /// </summary>
        public async void RunAsynchronously()
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
                Debug.Log("Pathfinding: Run A* asynchronously.");
#endif
                var nodePath = await _aStar.RunAsync(_startNode, Satisfies, GetConnections, GetCost, Heuristic);
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

        /// <summary>
        /// Checks if the target is near the path, if not it calculates the path.
        /// Also checks if it reached the current point and changes to the next one.
        /// and catches the current point to move to.
        /// </summary>
        /// <returns>Current Waypoint</returns>
        public Vector3 CalculateWaypoint()
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
                if (runAsynchronously)
                {
                    RunAsynchronously();
                }
                else
                {
                    Run();
                }
            }

            _currentPoint = GetCurrentWaypoint();
            return _currentPoint;
        }

        /// <summary>
        /// Gets the position of the current point to move.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCurrentPoint() => _currentPoint;

        #endregion

        private Vector3 GetCurrentWaypoint()
        {
            var currentPoint = Waypoints[CurrentIndex];
            currentPoint.y = _origin.position.y;
            
            if (Vector3.Distance(_origin.position, currentPoint) < minDistanceToReachNode)
            {
                OnPointHandler();
                SetNextNode();
            }

            return Waypoints[CurrentIndex];
        }

        private void SetNextNode()
        {
            if (CurrentIndex >= Waypoints.Count - 1)
            {
                OnEndOfPathHandler();
                return;
            }
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

        #region Event Handlers

        private void OnPointHandler()
        {
            OnPointReached?.Invoke();
        }

        private void OnEndOfPathHandler()
        {
            OnEndOfPathReached?.Invoke();
        }

        private void OnTargetChangedHandler(Transform transform)
        {
            OnTargetChanged?.Invoke(transform);
        }

        private void OnTargetPosChangedHandler(Vector3 pos)
        {
            OnTargetPosChanged?.Invoke(pos);
        }

        private void OnSetEnabledHandler(bool value)
        {
            if (_prevEnableState != value) OnSetEnabled?.Invoke(value);
        }

        private void OnPathCalculatedHandler(bool value)
        {
            OnPathCalculated?.Invoke(value);
        }

        #endregion
        
        public void Dispose()
        {
            Waypoints = null;
            target = null;
            OnPointReached = null;
            OnEndOfPathReached = null;
            OnTargetChanged = null;
            OnTargetPosChanged = null;
            OnSetEnabled = null;
            OnPathCalculated = null;
            _origin = null;
            if (_aStar != null) _aStar.Dispose();
            _aStar = null;
            _startNode = null;
            _grid = null;
        }

        #region Gizmos

        [Conditional("UNITY_EDITOR")]
        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!showGizmos || onSelected) return;
            Draw();
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!showGizmos || !onSelected) return;
            Draw();
#endif
        }

#if UNITY_EDITOR

        private void Draw()
        {
            if (showDetection)
            {
                DrawEntityRadius();
                DrawClosestNodeRadius();
                DrawRadiusToCheck();
            }
            
            
            if (Waypoints == null || Waypoints.Count < 0) return;

            if (showPath)
            {
                DrawStartPoint();
                DrawEndPoint();
                DrawCurrentPoint();
                DrawPath();
            }
            
        }
        
        private void DrawPath()
        {
            var totalWaypoints = Waypoints.Count;
            for (var i = 0; i < totalWaypoints; i++)
            {
                if (i + 1 > totalWaypoints - 1) continue;
                
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

        #endregion
        
    }
}