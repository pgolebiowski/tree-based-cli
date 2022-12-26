using System.Threading.Tasks;

namespace TreeBasedCli
{
    public interface ICommandHandler<T> where T : IParsedCommandArguments
    {
        Task HandleAsync(T arguments, LeafCommand executedCommand);
    }
}