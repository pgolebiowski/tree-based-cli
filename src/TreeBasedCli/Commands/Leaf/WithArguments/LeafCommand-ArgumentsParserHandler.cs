using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeBasedCli.Exceptions;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents a leaf command in a command tree. It is designed to be the most flexible and powerful way
    /// to create leaf commands, and is intended for use in scenarios where you need to specify options and/or
    /// inject custom dependencies.
    /// 
    /// <para>- <typeparamref name="TArguments"/> is a custom class that represents the already-parsed arguments
    /// to your command options.</para>
    /// <para>- <typeparamref name="TParser"/> is a custom class that parses user input into an instance
    /// of <typeparamref name="TArguments"/>.</para>
    /// <para>- <typeparamref name="THandler"/> is a custom class that handles the execution
    /// of your command asynchronously.</para>
    /// 
    /// To use this class, you will need to derive from it and provide implementations for <typeparamref name="TArguments"/>,
    /// <typeparamref name="TParser"/>, <typeparamref name="THandler"/> that suit your specific needs. This allows you
    /// to tailor your leaf commands to the unique requirements of your CLI and create a highly customized
    /// and user-friendly experience for your users.
    /// </summary>
    public class LeafCommand<TArguments, TParser, THandler> : LeafCommand
        where TArguments : IParsedCommandArguments
        where TParser : ICommandArgumentParser<TArguments>
        where THandler : ILeafCommandHandler<TArguments>
    {
        /// <inheritdoc cref="TreeBasedCli.LeafCommand{TArguments, TParser, THandler}" />
        public LeafCommand(
            string label,
            string[] description,
            IReadOnlyList<CommandOption> options)
                : base(label, description, options)
        {
        }

        /// <summary>
        /// Represents the task that will be run when this leaf command is executed.
        /// It will resolve the <typeparamref name="TParser"/> and <typeparamref name="THandler"/> objects 
        /// using the dependency injection service of the command tree it belongs to.
        /// It will then parse the <paramref name="commandArguments"/> and, if the parsing is successful,
        /// feed it into the <typeparamref name="THandler"/> object, which implements
        /// the <see cref="TreeBasedCli.ILeafCommandHandler{T}"/> interface, and has an asynchronous
        /// method <see cref="TreeBasedCli.ILeafCommandHandler{T}.HandleAsync(T, LeafCommand)"/>.
        /// </summary>
        public override Task TaskToRun(CommandArguments commandArguments)
        {
            TParser parser = this.DependencyInjectionService.Resolve<TParser>();
            THandler handler = this.DependencyInjectionService.Resolve<THandler>();

            IParseResult<TArguments> parseResult = parser.Parse(commandArguments);

            switch (parseResult)
            {
                case SuccessfulParseResult<TArguments> successResult:
                    return handler.HandleAsync(successResult.Value, this);

                case FailedParseResult<TArguments> failedResult:
                    throw new WrongCommandUsageException(this, failedResult.ErrorMessage);

                default:
                    throw new Exception($"Unknown parse result type: '{parseResult.GetType().Name}'.");
            }
        }
    }
}