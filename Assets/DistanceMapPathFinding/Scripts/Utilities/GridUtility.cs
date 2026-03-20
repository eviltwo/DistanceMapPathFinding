using UnityEngine;

namespace DistanceMapPathfinding.Utilities
{
    public static class GridUtility
    {
        public static int PositionToIndex(Vector3Int point, Vector3Int size)
        {
            return point.x + point.y * size.x + point.z * size.x * size.y;
        }

        public static Vector3Int IndexToPosition(int index, Vector3Int size)
        {
            return new Vector3Int(index % size.x, index / size.x % size.y, index / (size.x * size.y));
        }

        public static bool IsInside(Vector3Int position, Vector3Int size)
        {
            for (var i = 0; i < 3; i++)
            {
                if (position[i] < 0 || position[i] >= size[i]) return false;
            }

            return true;
        }
    }
}
