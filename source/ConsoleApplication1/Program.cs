
namespace ConsoleApplication1
{
    using System;
    using System.Threading;

    using Multithreading.ProducerConsumer;

    class Program
    {
        static void Main(string[] args)
        {
            var q = new QueueWithMultipleConsumerThreads<int>(
                numberOfWorkerThreads: 10,
                consumeAction: i =>
                        {
                            Console.WriteLine($"Consumed {i} from thread {Thread.CurrentThread.Name}, id: {Thread.CurrentThread.ManagedThreadId}");
                        });

            // Add some entries to the q
            for (int i = 0; i < 10000; i++) { q.Enque(i); }
            
            Thread.Sleep(5000); // Give the q time to work
            q.Shutdown();

            Console.WriteLine($"Remaing q item: {q.Count()}");
        }
    }
}
