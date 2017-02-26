
namespace ConsoleApplication1
{
    using System;
    using System.Threading;

    using Multithreading.ProducerConsumer;

    class Program
    {
        static void Main(string[] args)
        {
            var q = new QueueWithMultipleConsumerThreads<int>(100, i =>
                        {
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
