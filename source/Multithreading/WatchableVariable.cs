namespace Multithreading
{
    using System.Threading;

    /// <summary>
    /// Usage:
    /// 
    /// private WatchedVariable<string> state;
    ///
    ///public void WaitForBlob()
    ///{
    ///    string value = state.Value;
    ///    while (value != "Blob")
    ///    {
    ///        value = state.WaitForChange(value);
    ///    }
    ///}
    ///</summary>
    /// <typeparam name="T"></typeparam>
    public class WatchedVariable<T>
      where T : class
    {
        private volatile T value;
        private readonly object valueLock = new object();

        public T Value
        {
            get { return this.value; }
            set
            {
                lock (this.valueLock)
                {
                    this.value = value;
                    Monitor.PulseAll(this.valueLock);  // all waiting threads will resume once we release valueLock
                }
            }
        }

        public T WaitForChange(T fromValue)
        {
            lock (this.valueLock)
            {
                while (true)
                {
                    T nextValue = this.value;
                    if (nextValue != fromValue) return nextValue;  // no race condition here: PulseAll can only be reached once we hit Wait()
                    Monitor.Wait(this.valueLock);  // wait for a changed pulse
                }
            }
        }

        public WatchedVariable(T initValue)
        {
            this.value = initValue;
        }
    }
}
