namespace TreeBasedCli
{
    /// <summary>
    /// Represents a failed parse result with an error message.
    /// </summary>
    public class FailedParseResult<T> : IParseResult<T>
    {
        /// <inheritdoc cref="FailedParseResult{T}" />
        public FailedParseResult(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; }
    }
}