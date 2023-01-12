namespace TreeBasedCli
{
    /// <summary>
    /// Generic interface that defines a method for parsing command arguments.
    /// The method defined is <see cref="Parse"/> that takes <see cref="CommandArguments"/>
    /// as input and returns an <see cref="IParseResult{T}"/> object representing
    /// the result of the parse operation.
    /// </summary>
    public interface ICommandArgumentParser<T> where T : IParsedCommandArguments
    {
        /// <summary>
        /// Parses the specified <see cref="CommandArguments"/> and returns an <see cref="IParseResult{T}"/> object 
        /// representing the result of the parse operation.
        /// </summary>
        IParseResult<T> Parse(CommandArguments arguments);
    }
}