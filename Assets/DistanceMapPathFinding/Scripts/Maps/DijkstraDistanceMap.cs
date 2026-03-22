using System.Collections.Generic;
using DistanceMapPathfinding.Collections;

namespace DistanceMapPathfinding.Maps
{
    public class DijkstraDistanceMap : IDistanceMap
    {
        private readonly ICostMap _costMap;

        private readonly float[] _distances;

        private readonly bool[] _visited;

        private readonly BinaryHeap<int> _heap;

        public bool IsFinished => _heap.Count == 0;

        public DijkstraDistanceMap(ICostMap costMap, IReadOnlyList<int> startPositions)
        {
            _costMap = costMap;
            _distances = new float[costMap.GetNodeCount()];
            for (var i = 0; i < _distances.Length; i++)
            {
                _distances[i] = float.PositiveInfinity;
            }

            _heap = new BinaryHeap<int>(new DistanceComparer(_distances));
            foreach (var startPosition in startPositions)
            {
                _distances[startPosition] = 0;
                _heap.Push(startPosition);
            }

            _visited = new bool[costMap.GetNodeCount()];
        }

        public void NextStep()
        {
            if (_heap.Count == 0) return;
            var position = _heap.Pop();
            if (_visited[position]) return;
            _visited[position] = true;

            // Push neighbors to open set
            System.Span<int> neighborBuffer = stackalloc int[32];
            var hitCount = _costMap.GetNeighbors(position, neighborBuffer);
            for (var i = 0; i < hitCount; i++)
            {
                var neighborPosition = neighborBuffer[i];
                if (_visited[neighborPosition]) continue;
                var totalCost = _distances[position] + _costMap.GetCost(position, neighborPosition);
                if (totalCost >= _distances[neighborPosition]) continue;
                _distances[neighborPosition] = totalCost;
                _heap.Push(neighborPosition);
            }
        }

        public float GetDistance(int position)
        {
            return _distances[position];
        }

        public int GetNeighbors(int index, System.Span<int> neighbors)
        {
            return _costMap.GetNeighbors(index, neighbors);
        }

        private class DistanceComparer : IComparer<int>
        {
            private readonly float[] _distances;

            public DistanceComparer(float[] distances)
            {
                _distances = distances;
            }

            public int Compare(int x, int y)
            {
                return _distances[x].CompareTo(_distances[y]);
            }
        }
    }
}
