using System.Collections.Generic;
using DistanceMapPathfinding.Collections;
using NUnit.Framework;

namespace DistanceMapPathFinding.Tests
{
    public class BinaryHeapTest
    {
        private BinaryHeap<int> _heap;

        [SetUp]
        public void Setup()
        {
            _heap = new BinaryHeap<int>();
        }

        [TestCase(new[] { 1, 3, 2, 5, 4 }, new[] { 1, 2, 3, 4, 5 })]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 5 })]
        [TestCase(new[] { 5, 4, 3, 2, 1 }, new[] { 1, 2, 3, 4, 5 })]
        [TestCase(new[] { 3, 3, 2, 2, 2, 1 }, new[] { 1, 2, 2, 2, 3, 3 })]
        public void Sort(int[] inputValues, int[] expectedValues)
        {
            foreach (var inputValue in inputValues)
            {
                _heap.Push(inputValue);
            }

            var results = new List<int>();
            for (int i = 0; i < inputValues.Length * 2; i++)
            {
                if (_heap.Count == 0) break;
                results.Add(_heap.Pop());
            }

            CollectionAssert.AreEqual(expectedValues, results);
        }

        [Test]
        public void StartEmpty()
        {
            Assert.AreEqual(0, _heap.Count);
        }

        [TestCase(new[] { 1, 3, 2, 5, 4 })]
        public void ValidLength(int[] inputValues)
        {
            foreach (var inputValue in inputValues)
            {
                _heap.Push(inputValue);
            }

            Assert.AreEqual(inputValues.Length, _heap.Count);
        }

        [TestCase(new[] { 1, 3, 2, 5, 4 })]
        public void PopToEmpty(int[] inputValues)
        {
            foreach (var inputValue in inputValues)
            {
                _heap.Push(inputValue);
            }

            for (var i = 0; i < inputValues.Length; i++)
            {
                _heap.Pop();
            }

            Assert.AreEqual(0, _heap.Count);
        }
    }
}
