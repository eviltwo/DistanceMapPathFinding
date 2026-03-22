using System.Collections.Generic;

namespace DistanceMapPathfinding.Collections
{
    public class BinaryHeap<T>
    {
        private readonly List<T> _data = new();
        private readonly IComparer<T> _comparer;

        public int Count => _data.Count;

        public BinaryHeap(IComparer<T> comparer = null)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public void Push(T item)
        {
            _data.Add(item);
            HeapifyUp(_data.Count - 1);
        }

        public T Pop()
        {
            if (_data.Count == 0) throw new System.InvalidOperationException("Heap is empty");

            var root = _data[0];
            var lastIndex = _data.Count - 1;
            _data[0] = _data[lastIndex];
            _data.RemoveAt(lastIndex);
            if (_data.Count > 0)
            {
                HeapifyDown(0);
            }

            return root;
        }

        public T Peek()
        {
            if (_data.Count > 0) throw new System.InvalidOperationException("Heap is empty");

            return _data[0];
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                var parentIndex = (index - 1) / 2;
                if (_comparer.Compare(_data[index], _data[parentIndex]) >= 0) break;
                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            var count = _data.Count;
            while (true)
            {
                var leftIndex = index * 2 + 1;
                var rightIndex = index * 2 + 2;
                var smallestIndex = index;

                if (rightIndex < count && _comparer.Compare(_data[rightIndex], _data[smallestIndex]) < 0)
                {
                    smallestIndex = rightIndex;
                }

                if (leftIndex < count && _comparer.Compare(_data[leftIndex], _data[smallestIndex]) < 0)
                {
                    smallestIndex = leftIndex;
                }

                if (smallestIndex == index) break;
                Swap(index, smallestIndex);
                index = smallestIndex;
            }
        }

        private void Swap(int indexA, int indexB)
        {
            (_data[indexA], _data[indexB]) = (_data[indexB], _data[indexA]);
        }
    }
}
