using DistanceMapPathfinding.Maps;
using UnityEngine;

namespace DistanceMapPathfinding
{
    public class PathFinder
    {
        private readonly IDistanceMap _distanceMap;

        public PathFinder(IDistanceMap distanceMap)
        {
            _distanceMap = distanceMap;
        }

        public bool FindNext(int position, out int nextPosition, bool useRandomTieBreak = true)
        {
            System.Span<int> nextBuffer = stackalloc int[32];
            var nexts = FindNext(position, nextBuffer);
            if (nexts == 0)
            {
                nextPosition = -1;
                return false;
            }

            var useIndex = useRandomTieBreak ? Random.Range(0, nexts) : 0;
            nextPosition = nextBuffer[useIndex];
            return true;
        }

        public int FindNext(int position, System.Span<int> nextPositions)
        {
            System.Span<int> neighborBuffer = stackalloc int[32];
            var neighborCount = _distanceMap.GetNeighbors(position, neighborBuffer);
            if (neighborCount == 0)
            {
                return 0;
            }

            var minDistance = _distanceMap.GetDistance(position);
            var foundCount = 0;
            for (var i = 0; i < neighborCount; i++)
            {
                var neighborPosition = neighborBuffer[i];
                var distance = _distanceMap.GetDistance(neighborPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    foundCount = 0;
                    nextPositions[foundCount] = neighborPosition;
                    foundCount++;
                }
                else if (Mathf.Approximately(distance, minDistance) && distance < float.PositiveInfinity)
                {
                    if (foundCount >= nextPositions.Length)
                    {
                        Debug.LogWarning("Returned more neighbors than expected.");
                        continue;
                    }

                    nextPositions[foundCount] = neighborPosition;
                    foundCount++;
                }
            }

            return foundCount;
        }

        public int FindPath(int position, System.Span<int> pathPositions, bool useRandomTieBreak = true)
        {
            var currentPosition = position;
            for (var i = 0; i < pathPositions.Length; i++)
            {
                if (FindNext(currentPosition, out var nextPosition, useRandomTieBreak))
                {
                    pathPositions[i] = nextPosition;
                    currentPosition = nextPosition;
                }
                else
                {
                    return i;
                }
            }

            return pathPositions.Length;
        }
    }
}
