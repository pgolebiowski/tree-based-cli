namespace TreeBasedCli.DependencyInjection
{
    public interface IDependencyInjectionService
    {
        TParser ResolveParser<TArguments, TParser>()
            where TArguments : IParsedCommandArguments
            where TParser : ICommandArgumentParser<TArguments>;

        THandler ResolveHandler<TArguments, THandler>()
            where TArguments : IParsedCommandArguments
            where THandler : ICommandHandler<TArguments>;
    }
}