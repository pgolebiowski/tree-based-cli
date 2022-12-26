namespace TreeBasedCli
{
    public interface ICommand<TArguments, TParser, THandler>
        where TArguments : IParsedCommandArguments
        where TParser : ICommandArgumentParser<TArguments>
        where THandler : ICommandHandler<TArguments>
    {
    }
}