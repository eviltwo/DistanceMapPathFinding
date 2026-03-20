namespace DistanceMapPathfinding.Maps
{
    public interface IDistanceMap
    {
        float GetDistance(int index);

        int GetNeighbors(int index, System.Span<int> neighbors);
    }
}
