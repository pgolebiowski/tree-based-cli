using System.Threading.Tasks;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents a handler for <see cref="TreeBasedCli.LeafCommand{THandler}" />,
    /// a leaf command that has no arguments but leverages dependency injection.
    /// </summary>
    public interface ILeafCommandHandler
    {
        Task HandleAsync(LeafCommand executedCommand);
    }
}
