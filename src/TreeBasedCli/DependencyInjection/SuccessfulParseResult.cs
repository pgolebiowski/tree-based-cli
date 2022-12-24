namespace TreeBasedCli.DependencyInjection
{
    public class SuccessfulParseResult<T> : IParseResult<T>
    {
        public SuccessfulParseResult(T value)
        {
            this.Value = value;
        }

        public T Value { get; }
    }
}