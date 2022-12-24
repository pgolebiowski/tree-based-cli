namespace TreeBasedCli.DependencyInjection
{
    public static class ParseResult
    {
        public static SuccessfulParseResult<T> ForValue<T>(T value)
            => new SuccessfulParseResult<T>(value);

        public static FailedParseResult<T> ForError<T>(string errorMessage)
            => new FailedParseResult<T>(errorMessage);
    }
}