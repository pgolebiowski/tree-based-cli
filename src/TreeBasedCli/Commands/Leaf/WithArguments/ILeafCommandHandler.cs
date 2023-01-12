using System.Threading.Tasks;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents a handler for <see cref="TreeBasedCli.LeafCommand{TArguments, TParser, THandler}" />,
    /// a leaf command that has arguments and leverages dependency injection.
    /// </summary>
    public interface ILeafCommandHandler<T> where T : IParsedCommandArguments
    {
        /// <summary>
        /// Asynchronous method that represents the task of handling logic of the leaf command. 
        /// </summary>
        /// <param name="arguments">The parsed command arguments that the handler operates on.</param>
        /// <param name="executedCommand">The executed command, which can be used to access the command tree.</param>
        Task HandleAsync(T arguments, LeafCommand executedCommand);
    }
}