namespace DistanceMapPathfinding.Maps
{
    public interface IDistanceMap
    {
        int GetLength();

        float GetDistance(int index);

        int GetNeighbors(int index, int[] neighbors);
    }
}
