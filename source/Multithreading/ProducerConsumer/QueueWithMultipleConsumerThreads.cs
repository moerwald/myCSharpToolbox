
namespace Multithreading.ProducerConsumer
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    using log4net;

    public class  QueueWithMultipleConsumerThreads<T>
    {
        private readonly ConcurrentBag<Worker<T>> workers = new ConcurrentBag<Worker<T>>();
        private readonly BlockingCollection<T> queue = new BlockingCollection<T>();

        private readonly CancellationTokenSource cancelationSource;

        public QueueWithMultipleConsumerThreads(uint numberOfWorkerThreads, Action<T> consumeAction  )
        {
            if (numberOfWorkerThreads == 0) { throw new ArgumentException($"{nameof(numberOfWorkerThreads)} must be > 0"); }
            if (consumeAction == null) { throw new ArgumentNullException(nameof(consumeAction));}

            this.cancelationSource = new CancellationTokenSource();

            for (var i = 0; i < numberOfWorkerThreads; i++)
            {
                // Create a worker and assign it to a thread
                var threadName = $"Worker {i}";
                var logger = LogManager.GetLogger(threadName);

                var w = new Worker<T>(this.queue, threadName, consumeAction, logger, this.cancelationSource.Token);

                this.workers.Add(w);
                w.StartWork();
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
            this.cancelationSource.Cancel();
            while (!this.workers.IsEmpty)
            {
                Worker<T> w;
                this.workers.TryTake(out w);
                w?.Shutdown();
            }
        }
    }
}
