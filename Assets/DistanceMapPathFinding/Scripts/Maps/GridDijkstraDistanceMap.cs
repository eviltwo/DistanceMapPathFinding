using System.Collections.Generic;
using DistanceMapPathfinding.Utilities;
using UnityEngine;

namespace DistanceMapPathfinding.Maps
{
    public class GridDijkstraDistanceMap : DijkstraDistanceMap, IGridDistanceMap
    {
        private readonly Vector3Int _size;

        public Vector3Int Size => _size;

        public GridDijkstraDistanceMap(IGridCostMap costMap, IReadOnlyList<Vector3Int> startPositions) : base(costMap, PositionToIndex(startPositions, costMap.Size))
        {
            _size = costMap.Size;
        }

        public float GetDistance(Vector3Int position)
        {
            return base.GetDistance(GridUtility.PositionToIndex(position, _size));
        }

        private static int[] PositionToIndex(IReadOnlyList<Vector3Int> positions, Vector3Int size)
        {
            var indices = new int[positions.Count];
            for (var i = 0; i < positions.Count; i++)
            {
                indices[i] = GridUtility.PositionToIndex(positions[i], size);
            }

            return indices;
        }
    }
}
