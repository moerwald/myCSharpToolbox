using System;
using System.Collections.Generic;

// Demonstrate a Priority Queue implemented with a Binary Heap
namespace PriorityQueues
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        // Implementation of the min binary heap
        // Origin code from: https://visualstudiomagazine.com/Articles/2012/11/01/Priority-Queues-with-C.aspx?Page=2
        // For interactive explanation see: https://www.cs.usfca.edu/~galles/visualization/Heap.html
        private List<T> data;

        private object lockObject = new object();

        public PriorityQueue()
        {
            this.data = new List<T>();
        }

        public void Enqueue(T item)
        {
            lock (this.lockObject)
            {
                // When using a list as internal storage, the trick is that for any parent node in the list at index [pi], the two child nodes are located at indexes [2 * pi + 1] and [2 * pi + 2]. 
                // For a given child node at index [ci], its parent is located at index [(ci - 1) / 2].
                data.Add(item);
                int ci = data.Count - 1; // child index; start at end
                while (ci > 0)
                {
                    int pi = (ci - 1) / 2; // parent index
                    if (data[ci].CompareTo(data[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done

                    T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
                    ci = pi;
                }
            }
        }

        public T Dequeue()
        {
            lock (this.lockObject)
            {
                // assumes pq is not empty; up to calling code
                int lastIndex = data.Count - 1; // last index (before removal)
                T frontItem = data[0];   // fetch the front
                data[0] = data[lastIndex];
                data.RemoveAt(lastIndex);

                --lastIndex; // last index (after removal)
                int parentIndex = 0; // parent index. start at front of pq
                while (true)
                {
                    int leftChildIndex = parentIndex * 2 + 1; // left child index of parent
                    if (leftChildIndex > lastIndex) break;  // no children so done
                    int rightChildIndex = leftChildIndex + 1;     // right child
                    if (rightChildIndex <= lastIndex && data[rightChildIndex].CompareTo(data[leftChildIndex]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                        leftChildIndex = rightChildIndex;
                    if (data[parentIndex].CompareTo(data[leftChildIndex]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
                    T tmp = data[parentIndex]; data[parentIndex] = data[leftChildIndex]; data[leftChildIndex] = tmp; // swap parent and child
                    parentIndex = leftChildIndex;
                }
                return frontItem;
            }
        }

        public T Peek()
        {
            lock (this.lockObject)
            {
                T frontItem = data[0];
                return frontItem;
            }
        }

        public int Count()
        {
            return data.Count;
        }

        public override string ToString()
        {
            lock (this.lockObject)
            {
                string s = "";
                for (int i = 0; i < data.Count; ++i)
                    s += data[i].ToString() + " ";
                s += "count = " + data.Count;
                return s;
            }
        }

        public bool IsConsistent()
        {
            lock (this.lockObject)
            {
                // is the heap property true for all data?
                if (data.Count == 0) return true;
                int lastIndex = data.Count - 1; // last index
                for (int parentIndex = 0; parentIndex < data.Count; ++parentIndex) // each parent index
                {
                    int leftChildIndex = 2 * parentIndex + 1; // left child index
                    int rightChildIndex = 2 * parentIndex + 2; // right child index

                    if (leftChildIndex <= lastIndex && data[parentIndex].CompareTo(data[leftChildIndex]) > 0) return false; // if lc exists and it's greater than parent then bad.
                    if (rightChildIndex <= lastIndex && data[parentIndex].CompareTo(data[rightChildIndex]) > 0) return false; // check the right child too.
                }
                return true; // passed all checks
            }
        } // IsConsistent
    } // PriorityQueue

} // ns
