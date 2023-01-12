using System.Threading.Tasks;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents a handler for <see cref="TreeBasedCli.LeafCommand{THandler}" />,
    /// a leaf command that has no arguments but leverages dependency injection.
    /// </summary>
    public interface ILeafCommandHandler
    {
        /// <summary>
        /// Asynchronous method that represents the task of handling logic of the leaf command. 
        /// </summary>
        /// <param name="executedCommand">The executed command, which can be used to access the command tree.</param>
        Task HandleAsync(LeafCommand executedCommand);
    }
}
