namespace TreeBasedCli
{
    /// <summary>
    /// A utility class that provides a convenient way of creating instances
    /// of <see cref="SuccessfulParseResult{T}"/> and <see cref="FailedParseResult{T}"/> 
    /// without the need to explicitly call the constructors of these classes.
    /// </summary>
    public static class ParseResult
    {
        /// <summary>
        /// Creates an instance of <see cref="SuccessfulParseResult{T}"/>
        /// with the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The parsed object.</param>
        public static SuccessfulParseResult<T> ForValue<T>(T value)
            => new SuccessfulParseResult<T>(value);

        /// <summary>
        /// Creates an instance of <see cref="FailedParseResult{T}"/>
        /// with the specified <paramref name="errorMessage"/>.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public static FailedParseResult<T> ForError<T>(string errorMessage)
            => new FailedParseResult<T>(errorMessage);
    }
}