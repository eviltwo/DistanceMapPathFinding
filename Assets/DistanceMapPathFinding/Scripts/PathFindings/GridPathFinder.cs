using DistanceMapPathfinding.Maps;
using DistanceMapPathfinding.Utilities;
using UnityEngine;

namespace DistanceMapPathfinding.PathFindings
{
    public class GridPathFinder : PathFinder
    {
        private readonly Vector3Int _size;

        public GridPathFinder(IGridDistanceMap distanceMap) : base(distanceMap)
        {
            _size = distanceMap.Size;
        }

        public bool FindNext(Vector3Int position, out Vector3Int nextPosition, bool useRandomTieBreak = true)
        {
            var index = GridUtility.PositionToIndex(position, _size);
            if (base.FindNext(index, out var nextIndex, useRandomTieBreak))
            {
                nextPosition = GridUtility.IndexToPosition(nextIndex, _size);
                return true;
            }
            else
            {
                nextPosition = default;
                return false;
            }
        }

        public int FindNext(Vector3Int position, System.Span<Vector3Int> nextPositions)
        {
            var index = GridUtility.PositionToIndex(position, _size);
            System.Span<int> indexBuffer = stackalloc int[32];
            var foundCount = base.FindNext(index, indexBuffer);
            for (var i = 0; i < foundCount; i++)
            {
                nextPositions[i] = GridUtility.IndexToPosition(indexBuffer[i], _size);
            }

            return foundCount;
        }

        public int FindPath(Vector3Int position, System.Span<Vector3Int> pathPositions, bool useRandomTieBreak = true)
        {
            var index = GridUtility.PositionToIndex(position, _size);
            System.Span<int> indexBuffer = stackalloc int[128];
            var nodeCount = FindPath(index, indexBuffer, useRandomTieBreak);
            for (var i = 0; i < nodeCount; i++)
            {
                pathPositions[i] = GridUtility.IndexToPosition(indexBuffer[i], _size);
            }

            return nodeCount;
        }
    }
}
