namespace TreeBasedCli.DependencyInjection
{
    public interface ICommandArgumentParser<T> where T : IParsedCommandArguments
    {
        IParseResult<T> Parse(CommandArguments arguments);
    }
}