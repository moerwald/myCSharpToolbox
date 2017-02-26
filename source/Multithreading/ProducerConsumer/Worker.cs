

namespace Multithreading.ProducerConsumer
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    public class Worker<T>
    {
        public Worker(BlockingCollection<T> collection, string workerName, Action<T> actionToBeCalled)
        {
            // todo: add logging interface
            this.collection = collection;
            this.workerName = workerName;
            this.actionToBeCalled = actionToBeCalled;
            this.cancelationTokenSource = new CancellationTokenSource();
            this.cancelationToken = this.cancelationTokenSource.Token;
        }

        // This method will be called when the thread is started.
        public void DoWork()
        {
            while (!this.shouldStop)
            {
                try
                {
                    //Console.WriteLine($"[{this.workerName}]: Calling collection take");
                    var item = this.collection.Take(this.cancelationToken);
                    //Console.WriteLine($"[{this.workerName}]: Retrieved item. Calling action with item {item}");
                    this.actionToBeCalled.Invoke(item);
                    //Console.WriteLine("worker thread: working...");
                }
                catch (Exception)
                {
                }
                
            }
            Console.WriteLine("worker thread: terminating gracefully.");
        }
        public void RequestStop()
        {
            this.cancelationTokenSource.Cancel();
            this.shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool shouldStop;

        private readonly BlockingCollection<T> collection;

        private readonly string workerName;

        private readonly Action<T> actionToBeCalled;

        private readonly CancellationToken cancelationToken;

        private readonly CancellationTokenSource cancelationTokenSource;
    }
}
