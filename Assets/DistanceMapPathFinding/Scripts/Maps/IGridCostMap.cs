using UnityEngine;

namespace DistanceMapPathfinding.Maps
{
    public interface IGridCostMap : ICostMap
    {
        Vector3Int Size { get; }

        float GetCost(Vector3Int position);
    }
}
