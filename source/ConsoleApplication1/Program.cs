using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    using System.Diagnostics;
    using System.Dynamic;
    using System.Threading;

    using Multithreading.ProducerConsumer;

    class Program
    {
        static void Main(string[] args)
        {
            int highestComsumedItem = 0;
            var q = new QueueWithMultipleConsumerThreads<int>(100, i =>
                        {
                            //Interlocked.Exchange(ref highestComsumedItem,i);
                            Console.WriteLine($"Consumed {i} from thread {Thread.CurrentThread.Name}, id: {Thread.CurrentThread.ManagedThreadId}");
                        });

            for (int i = 0; i < 10000; i++)
            {
                q.Enque(i);
            }


            Thread.Sleep(5000);
            q.Shutdown();

            Console.WriteLine($"Remaing q item: {q.Count()}");
        }
    }
}
