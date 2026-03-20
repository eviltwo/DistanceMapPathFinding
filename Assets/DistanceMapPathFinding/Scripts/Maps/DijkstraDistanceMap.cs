using System.Collections.Generic;

namespace DistanceMapPathfinding.Maps
{
    public class DijkstraDistanceMap
    {
        private readonly ICostMap _costMap;

        private readonly float[] _distances;

        private readonly bool[] _visited;

        private readonly List<int> _openSet = new();

        public static readonly int[] NeighborBuffer = new int[8];

        public bool IsFinished => _openSet.Count == 0;

        public DijkstraDistanceMap(ICostMap costMap, IReadOnlyList<int> startPositions)
        {
            _costMap = costMap;
            _distances = new float[costMap.GetNodeCount()];
            for (var i = 0; i < _distances.Length; i++)
            {
                _distances[i] = float.PositiveInfinity;
            }

            foreach (var startPosition in startPositions)
            {
                _distances[startPosition] = 0;
                _openSet.Add(startPosition);
            }

            _visited = new bool[costMap.GetNodeCount()];
        }

        public void NextStep()
        {
            if (IsFinished) return;
            var position = PopFromOpenSet();
            _visited[position] = true;

            // Push neighbors to open set
            var hitCount = _costMap.GetNeighbors(position, NeighborBuffer);
            for (var i = 0; i < hitCount; i++)
            {
                var neighborPosition = NeighborBuffer[i];
                if (_visited[neighborPosition]) continue;
                var totalCost = _distances[position] + _costMap.GetCost(position, neighborPosition);
                if (totalCost >= _distances[neighborPosition]) continue;
                _distances[neighborPosition] = totalCost;
                _openSet.Add(neighborPosition);
            }
        }

        public int GetNodeCount() => _distances.Length;

        public float GetDistance(int position)
        {
            return _distances[position];
        }

        private int PopFromOpenSet()
        {
            var minValue = float.MaxValue;
            var minIndex = 0;
            for (var i = 0; i < _openSet.Count; i++)
            {
                var value = _distances[_openSet[i]];
                if (value < minValue)
                {
                    minValue = value;
                    minIndex = i;
                }
            }

            var minPosition = _openSet[minIndex];
            _openSet.RemoveAt(minIndex);
            return minPosition;
        }
    }
}
