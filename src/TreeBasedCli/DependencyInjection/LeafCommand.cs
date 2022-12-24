using System;
using System.Collections.Generic;

namespace TreeBasedCli.DependencyInjection
{
    /// <summary>
    /// Same as <see cref="LeafCommand"/> but powered with dependency injection.
    /// </summary>
    public class LeafCommand<TArguments, TParser, THandler> : LeafCommand, ICommand<TArguments, TParser, THandler>
        where TArguments : IParsedCommandArguments
        where TParser : ICommandArgumentParser<TArguments>
        where THandler : ICommandHandler<TArguments>
    {
        private readonly IDependencyInjectionService dependencyInjectionService;

        public LeafCommand(
            string label,
            string[] description,
            IReadOnlyList<CommandOption> options,
            IDependencyInjectionService dependencyInjectionService)
                : base(label, description, options)
        {
            this.dependencyInjectionService = dependencyInjectionService;

            this.ConfigureTaskToRun();
        }

        private void ConfigureTaskToRun()
        {
            this.TaskToRun = arguments =>
            {
                TParser parser = this.dependencyInjectionService.ResolveParser<TArguments, TParser>();
                THandler handler = this.dependencyInjectionService.ResolveHandler<TArguments, THandler>();

                IParseResult<TArguments> parseResult = parser.Parse(arguments);

                switch (parseResult)
                {
                    case SuccessfulParseResult<TArguments> successResult:
                        return handler.HandleAsync(successResult.Value, this);

                    case FailedParseResult<TArguments> failedResult:
                        throw new WrongCommandUsageException(this, failedResult.ErrorMessage);

                    default:
                        throw new Exception($"Unknown parse result type: '{parseResult.GetType().Name}'.");
                }
            };
        }
    }
}