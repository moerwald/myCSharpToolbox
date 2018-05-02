using System;
using System.Threading.Tasks;

namespace References
{
    public class WriteOnce<T>
    {
        private readonly TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

        public Task<T> ValueAsync => this.tcs.Task;
        public T Value => this.tcs.Task.Result;

        public bool TrySetInitialValue(T value)
        {
            try
            {
                this.tcs.SetResult(value);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public void SetInitialValue(T value)
        {
            if (!TrySetInitialValue(value))
                throw new InvalidOperationException("The value has already been set.");
        }

        public static implicit operator T(WriteOnce<T> readOnly) => readOnly.Value;
        public static implicit operator Task<T>(WriteOnce<T> readOnly) => readOnly.ValueAsync;
    }
}
