
namespace Multithreading.ProducerConsumer
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    using log4net;

    public class  QueueWithMultipleConsumerThreads<T>
    {
        private readonly ConcurrentBag<Thread> threads = new ConcurrentBag<Thread>();
        private readonly ConcurrentBag<Worker<T>> workers = new ConcurrentBag<Worker<T>>();
        private readonly BlockingCollection<T> queue = new BlockingCollection<T>();

        public QueueWithMultipleConsumerThreads(uint numberOfWorkerThreads, Action<T> consumeAction  )
        {
            if (numberOfWorkerThreads == 0) { throw new ArgumentException($"{nameof(numberOfWorkerThreads)} must be > 0"); }
            if (consumeAction == null) { throw new ArgumentNullException(nameof(consumeAction));}

            for (var i = 0; i < numberOfWorkerThreads; i++)
            {
                // Create a worker and assign it to a thread
                var threadName = $"Worker thread {i}";
                var logger = LogManager.GetLogger(threadName);

                var w = new Worker<T>(this.queue, threadName, consumeAction, logger);
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
            while (!this.workers.IsEmpty)
            {
                Worker<T> w;
                this.workers.TryTake(out w);
                w?.RequestStop();
            }

            while (!this.threads.IsEmpty)
            {
                Thread t;
                this.threads.TryTake(out t);
                t?.Join(1000);
            }
        }
    }
}
