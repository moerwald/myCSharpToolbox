namespace Multithreading.ProducerConsumer
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    using log4net;

    /// <summary>
    /// A worker receives a collection to take elements from. After an element was succefully retrived it will call <see cref="consumeAction"/>. 
    /// Stopping the worker can be done via <see cref="RequestStop"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Worker<T>
    {
        public Worker(BlockingCollection<T> collection, string workerName, Action<T> consumeAction, ILog logger)
        {
            if (collection == null) { throw new ArgumentNullException(nameof(collection));}
            if (workerName == null) { throw new ArgumentNullException(nameof(workerName));}
            if (consumeAction == null) { throw new ArgumentNullException(nameof(consumeAction));}
            if (logger == null) { throw new ArgumentNullException(nameof(logger));}

            this.collection = collection;
            this.workerName = workerName;
            this.consumeAction = consumeAction;
            this.cancelationTokenSource = new CancellationTokenSource();
            this.cancelationToken = this.cancelationTokenSource.Token;
            this.logger = logger;
        }

        public void DoWork()
        {
            while (!this.shouldStop)
            {
                try
                {
                    var item = this.collection.Take(this.cancelationToken); // Take should block, until an element was added.
                    this.consumeAction?.Invoke(item); // Invoke the given action with the dequeued item
                }
                catch (Exception exception)
                {
                    this.logger.Warn($"[{this.workerName}]: Exception occurred: {exception}");
                }
            }

            this.logger.Debug($"[{this.workerName}]: Shutdown gracefully");
        }
        public void RequestStop()
        {
            this.logger.Debug($"[{this.workerName}]: {nameof(this.RequestStop)}");
            this.cancelationTokenSource.Cancel();
            this.shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data member will be accessed by multiple threads.
        private volatile bool shouldStop;

        private readonly BlockingCollection<T> collection;

        private readonly string workerName;

        private readonly Action<T> consumeAction;

        private readonly CancellationToken cancelationToken;

        private readonly CancellationTokenSource cancelationTokenSource;

        private readonly ILog logger;
    }
}
