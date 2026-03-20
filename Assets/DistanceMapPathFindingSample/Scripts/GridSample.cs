using System;
using DistanceMapPathfinding.Maps;
using DistanceMapPathfinding.PathFindings;
using UnityEngine;

namespace DistanceMapPathfindingSample
{
    public class GridSample : MonoBehaviour
    {
        public Grid Grid;

        public Vector3Int Size = Vector3Int.one;

        public Transform[] StartPoints = System.Array.Empty<Transform>();

        public Transform[] Obstacles = System.Array.Empty<Transform>();

        public Transform Agent;

        public bool DrawGizmos = true;

        public bool NextStep = false;

        public bool CalculateAllSteps = false;

        private GridCostMap _costMap;

        private GridDijkstraDistanceMap _distanceMap;

        private GridPathFinder _pathFinder;

        private void Start()
        {
            // Create cost map
            _costMap = new GridCostMap(Size, 1, true);
            foreach (var obstacle in Obstacles)
            {
                _costMap.SetWalkable(Grid.WorldToCell(obstacle.position), false);
            }

            // Create distance map
            var startPositions = new Vector3Int[StartPoints.Length];
            for (var i = 0; i < StartPoints.Length; i++)
            {
                startPositions[i] = Grid.WorldToCell(StartPoints[i].position);
            }

            _distanceMap = new GridDijkstraDistanceMap(_costMap, startPositions);

            // Create path finder
            _pathFinder = new GridPathFinder(_distanceMap);
        }

        private void Update()
        {
            if (NextStep)
            {
                _distanceMap.NextStep();
                NextStep = false;
            }

            if (CalculateAllSteps)
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                while (!_distanceMap.IsFinished)
                {
                    _distanceMap.NextStep();
                }

                sw.Stop();
                Debug.Log($"Time: {sw.ElapsedMilliseconds}ms");

                CalculateAllSteps = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_costMap == null || _distanceMap == null || _pathFinder == null) return;
            if (!DrawGizmos) return;

            for (var x = 0; x < Size.x; x++)
            {
                for (var y = 0; y < Size.y; y++)
                {
                    for (var z = 0; z < Size.z; z++)
                    {
                        var cell = new Vector3Int(x, y, z);
                        var isWalkable = _costMap.IsWalkable(cell);
                        var color = isWalkable ? Color.green : Color.red;
                        var position = Grid.GetCellCenterWorld(cell);
                        var rotation = transform.rotation;
                        var scale = Grid.cellSize;
                        var matrix = Matrix4x4.TRS(position, rotation, scale);
                        if (!isWalkable)
                        {
                            Gizmos.color = color;
                            Gizmos.matrix = matrix;
                            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
                        }

                        var distance = _distanceMap.GetDistance(cell);
                        var guistyle = new GUIStyle
                        {
                            fontSize = 10,
                            normal = { textColor = color },
                            alignment = TextAnchor.MiddleCenter,
                        };
                        UnityEditor.Handles.Label(position, distance.ToString("F0"), guistyle);
                    }
                }
            }

            Gizmos.color = Color.white;
            Gizmos.matrix = Matrix4x4.identity;
            var agentPosition = Grid.WorldToCell(Agent.position);
            Span<Vector3Int> path = stackalloc Vector3Int[128];
            var nodeCount = _pathFinder.FindPath(agentPosition, path);
            for (var i = 0; i < nodeCount - 1; i++)
            {
                var from = Grid.GetCellCenterWorld(path[i]);
                var to = Grid.GetCellCenterWorld(path[i + 1]);
                Gizmos.DrawLine(from, to);
            }
        }
#endif
    }
}
