using DistanceMapPathfinding.Utilities;
using UnityEngine;

namespace DistanceMapPathfinding.Maps
{
    public class GridCostMap : ICostMap
    {
        private readonly Vector3Int _size;

        public Vector3Int Size => _size;

        private readonly float[] _costs;

        private readonly bool[] _walkables;

        private static readonly Vector3Int[] Directions =
        {
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.forward,
            Vector3Int.back
        };

        public GridCostMap(Vector3Int size)
        {
            _size = size;
            _costs = new float[size.x * size.y * size.z];
            _walkables = new bool[size.x * size.y * size.z];
        }

        public GridCostMap(Vector3Int size, float defaultCost, bool defaultWalkable)
            : this(size)
        {
            FillArray(_costs, defaultCost);
            FillArray(_walkables, defaultWalkable);
        }

        public int GetNodeCount()
        {
            return _size.x * _size.y * _size.z;
        }

        public void SetCost(Vector3Int position, float cost)
        {
            var index = GridUtility.PositionToIndex(position, _size);
            _costs[index] = cost;
        }

        public float GetCost(int from, int to)
        {
            return _costs[to];
        }

        public void SetWalkable(Vector3Int position, bool walkable)
        {
            var index = GridUtility.PositionToIndex(position, _size);
            _walkables[index] = walkable;
        }

        public bool IsWalkable(Vector3Int position)
        {
            var index = GridUtility.PositionToIndex(position, _size);
            return _walkables[index];
        }

        public int GetNeighbors(int index, int[] neighbors)
        {
            var position = GridUtility.IndexToPosition(index, _size);
            var hitCount = 0;
            foreach (var direction in Directions)
            {
                var neighborPosition = position + direction;
                if (!IsInside(neighborPosition, _size)) continue;
                var neighborIndex = GridUtility.PositionToIndex(position + direction, _size);
                if (_walkables[neighborIndex])
                {
                    neighbors[hitCount] = neighborIndex;
                    hitCount++;
                }
            }

            return hitCount;
        }

        private static bool IsInside(Vector3Int position, Vector3Int size)
        {
            for (var i = 0; i < 3; i++)
            {
                if (position[i] < 0 || position[i] >= size[i]) return false;
            }

            return true;
        }

        private static void FillArray<T>(T[] array, T value)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
    }
}
