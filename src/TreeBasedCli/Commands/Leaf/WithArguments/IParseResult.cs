namespace TreeBasedCli
{
    /// <summary>
    /// Represents the result of parsing with <see cref="ICommandArgumentParser{T}.Parse" />.
    /// It has two implementations that indicate whether a parse operation was successful or not:
    /// <see cref="SuccessfulParseResult{T}" /> and <see cref="FailedParseResult{T}" />.
    /// </summary>
    public interface IParseResult<T>
    {
    }
}