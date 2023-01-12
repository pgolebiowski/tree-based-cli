namespace TreeBasedCli
{
    /// <summary>
    /// Represents a successful parse result that contains the parsed object of <typeparamref name="T"/>.
    /// It has a single property <see cref="Value"/> that holds the parsed object.
    /// </summary>
    public class SuccessfulParseResult<T> : IParseResult<T>
    {
        /// <inheritdoc cref="SuccessfulParseResult{T}" />
        public SuccessfulParseResult(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the parsed object.
        /// </summary>
        public T Value { get; }
    }
}