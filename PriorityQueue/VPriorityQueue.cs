/**
 * Created by Vidar Magnusson on 19/1 - 2019
 * https://github.com/ViddeM/
 */

using System;
using System.Collections.Generic;

namespace CSUtils.PriorityQueue
{

    // A simple min-heap implementation of a priorityQueue, implemented with an array.
    public class VPriorityQueue<T> : IPriorityQueue<T>
    {
        // NOTE: To get the accurate child/parent calculations, 
        // we want to start at index 1 
        private Entry[] entries;
        private int maxSize;
        private int currSize;
        private const int defaultSize = 10;


        private class Entry
        {
            public readonly T data;
            public readonly float Priority;

            public Entry(T data, float priority)
            {
                this.data = data;
                this.Priority = priority;
            }
        }

        public VPriorityQueue()
        {
            entries = new Entry[defaultSize + 1];
            maxSize = defaultSize;
            currSize = 0;
        }

        /// <summary>
        /// Enqueues the given entry with the given priority,
        /// lowest priorities are dequeued out first.
        /// </summary>
        public void Enqueue(T newEntry, float priority)
        {
            if (currSize >= maxSize)
            {
                Expand();
            }

            Entry e = new Entry(newEntry, priority);
            // + 1 since we're using 1 indexing
            currSize++;
            entries[currSize] = e;
            PlaceNode(currSize);
        }

        /// <summary>
        /// Dequeues the entry with the highest priority
        /// </summary>
        public T Dequeue()
        {
            if (currSize < 1)
            {
                throw new IndexOutOfRangeException("Trying to dequeue from empty heap!");
            }

            Swap(1, currSize);
            T entry = entries[currSize].data;
            RemoveAt(currSize);

            if (currSize > 1)
            {
                PushDown();
            }

            return entry;
        }

        /// <summary>
        /// If the given data exists within the queue,
        /// updates the priority of that element to the new value.
        /// </summary>
        public void UpdatePriority(T data, float priority)
        {

            int index = Find(data);
            if (index < 0)
            {
                throw new Exception("Entry not found: " + data);
            }

            entries[index] = new Entry(data, priority);

            int parentIndex = GetParentIndex(index);
            if (parentIndex > 0 && entries[parentIndex].Priority > priority)
            {
                PlaceNode(index);
            }
            else
            {
                PushDown(index);
            }
        }

        /// <summary>
        /// Returns true if the data exists in the queue.
        /// </summary>
        public bool Contains(T data)
        {
            if (Find(data) < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the number of elements in the queue.
        /// </summary>
        public int Count()
        {
            return currSize;
        }

        #region Private Methods

        // Increases the size of the internal array.
        private void Expand()
        {
            maxSize = 2 * maxSize;
            Entry[] newEntries = new Entry[maxSize + 1];
            for (int i = 1; i <= currSize; i++)
            {
                newEntries[i] = entries[i];
            }

            entries = newEntries;
        }

        // Finds the correct place for the newly added node.
        private void PlaceNode(int index)
        {
            int nodeIndex = index;
            bool keepGoing = true;
            while (keepGoing && nodeIndex > 1)
            {
                int parentIndex = GetParentIndex(nodeIndex);
                if (parentIndex < 0)
                { return; }

                if (entries[nodeIndex].Priority < entries[parentIndex].Priority)
                {
                    Swap(nodeIndex, parentIndex);
                    nodeIndex = parentIndex;
                }
                else
                {
                    keepGoing = false;
                }
            }
        }

        private void Swap(int a, int b)
        {
            Entry tmp = entries[a];
            entries[a] = entries[b];
            entries[b] = tmp;
        }

        private int GetParentIndex(int index)
        {
            if (index <= 1)
            {
                return -1;
            }

            return (index - (index % 2)) / 2;
        }

        private int GetLeftChildIndex(int index)
        {
            if (index < 1)
            {
                return -1;
            }

            if (index * 2 + 1 > currSize)
            {
                return -1;
            }

            return 2 * index;
        }

        private int GetRightChildIndex(int index)
        {
            if (index < 1)
            {
                return -1;
            }

            if (index * 2 + 1 > currSize)
            {
                return -1;
            }

            return 2 * index + 1;
        }

        private void RemoveAt(int index)
        {
            entries[index] = null;
            currSize--;
        }


        /// <summary>
        /// pushes down the root to it's correct position
        /// </summary>
        private void PushDown(int index = 1)
        {
            if (index > currSize)
            { return; }

            int currIndex = index;
            bool keepGoing = true;
            int leftChild = GetLeftChildIndex(currIndex);
            int rightChild = GetRightChildIndex(currIndex);
            while ((leftChild > 0 || rightChild > 0) && keepGoing)
            {
                int smallestChild = MinEntryIndex(leftChild, rightChild);
                if (entries[smallestChild].Priority < entries[currIndex].Priority)
                {
                    Swap(smallestChild, currIndex);
                    currIndex = smallestChild;
                    rightChild = GetRightChildIndex(currIndex);
                    leftChild = GetLeftChildIndex(currIndex);
                }
                else
                {
                    keepGoing = false;
                }
            }
        }

        private int MinEntryIndex(int a, int b)
        {
            if (entries[a].Priority < entries[b].Priority)
            {
                return a;
            }
            return b;
        }

        private int Find(T entry)
        {
            for (int i = 1; i <= currSize; i++)
            {
                if (EqualityComparer<T>.Default.Equals(entries[i].data, entry))
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion
    }
}