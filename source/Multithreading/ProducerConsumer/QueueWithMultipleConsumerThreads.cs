using System;

namespace Multithreading.ProducerConsumer
{
    using System.Collections.Concurrent;
    using System.Threading;

    public class  QueueWithMultipleConsumerThreads<T>
    {
        private readonly ConcurrentBag<Thread> threads = new ConcurrentBag<Thread>();
        private readonly ConcurrentBag<Worker<T>> workers = new ConcurrentBag<Worker<T>>();

        private readonly BlockingCollection<T> queue = new BlockingCollection<T>();

        public QueueWithMultipleConsumerThreads(uint numberOfWorkerThreads, Action<T> actionToBeCalled  )
        {
            if (numberOfWorkerThreads == 0) {  throw new ArgumentException($"{nameof(numberOfWorkerThreads)} must be > 0");}

            for (int i = 0; i < numberOfWorkerThreads; i++)
            {
                var threadName = $"Worker thread {i}";
                var w = new Worker<T>(this.queue, threadName, actionToBeCalled);
                var t = new Thread(w.DoWork) { IsBackground = true, Name = threadName};

                this.workers.Add(w);
                this.threads.Add(t);
                t.Start();
            }
        }

        public void Enque(T item)
        {
            this.queue.Add(item);
        }

        public int Count()
        {
            return this.queue.Count;
        }


        public void Shutdown()
        {
            foreach (var worker in this.workers)
            {
                worker.RequestStop();
            }

            foreach (var thread in this.threads)
            {
                thread.Join(2000);
            }

            // Todo: Clear bags

        }
    }
}
