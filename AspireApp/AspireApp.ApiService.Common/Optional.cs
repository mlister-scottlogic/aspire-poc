namespace AspireApp.ApiService.Common
{
    public readonly struct Optional<T>
    {
        private readonly T? value;

        private Optional(T? value)
        {
            this.value = value;
        }

        public static Optional<T> Some(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new Optional<T>(value);
        }

        public static Optional<T> None()
        {
            return new Optional<T>(default);
        }

        public readonly TResult Match<TResult>(Func<T, TResult> success, Func<TResult> failure)
        {
            if (value is not null)
            {
                return success(value);
            }
            else
            {
                return failure();
            }
        }

        public async readonly Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> success, Func<Task<TResult>> failure)
        {
            if (value is not null)
            {
                return await success(value);
            }
            else
            {
                return await failure();
            }
        }
    }
}
