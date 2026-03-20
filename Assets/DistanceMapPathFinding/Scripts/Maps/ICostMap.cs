namespace DistanceMapPathfinding.Maps
{
    public interface ICostMap
    {
        int GetNodeCount();

        float GetCost(int from, int to);

        int GetNeighbors(int index, System.Span<int> neighbors);
    }
}
