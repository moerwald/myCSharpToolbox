using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueues;

namespace CollectionTest
{
    [TestClass]
    public class PriorityQueueTest
    {
        public class Employee : IComparable<Employee>
        {
            public string lastName;
            public double priority; // smaller values are higher priority

            public Employee(string lastName, double priority)
            {
                this.lastName = lastName;
                this.priority = priority;
            }

            public override string ToString()
            {
                return "(" + lastName + ", " + priority.ToString("F1") + ")";
            }

            public int CompareTo(Employee other)
            {
                if (this.priority < other.priority) return -1;
                else if (this.priority > other.priority) return 1;
                else return 0;
            }
        } // Employee


        static void TestPriorityQueue(int numOperations)
        {
            Random rand = new Random(0);
            PriorityQueue<Employee> pq = new PriorityQueue<Employee>();
            for (int op = 0; op < numOperations; ++op)
            {
                int opType = rand.Next(0, 2);

                if (opType == 0) // enqueue
                {
                    string lastName = op + "man";
                    double priority = (100.0 - 1.0) * rand.NextDouble() + 1.0;
                    pq.Enqueue(new Employee(lastName, priority));
                    Assert.IsTrue(pq.IsConsistent(), $"Test fails after enqueue operation # {op}");
                }
                else // dequeue
                {
                    if (pq.Count() > 0)
                    {
                        Employee e = pq.Dequeue();
                        Assert.IsTrue(pq.IsConsistent(), $"Test fails after enqueue operation # {op}");
                    }
                }
            } // for
        } // TestPriorityQueue


        [TestMethod]
        public void TestEnqueueAndDequeue()
        {
            #region Arrange

            #endregion

            #region Act
            TestPriorityQueue(10);
            #endregion

            #region Assert

            #endregion
        }
    }
}
