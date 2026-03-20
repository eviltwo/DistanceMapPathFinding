using UnityEngine;

namespace DistanceMapPathfinding.Maps
{
    public interface IGridDistanceMap : IDistanceMap
    {
        Vector3Int Size { get; }

        float GetDistance(Vector3Int position);
    }
}
