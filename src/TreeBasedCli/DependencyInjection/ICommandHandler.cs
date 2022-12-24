using System.Threading.Tasks;

namespace TreeBasedCli.DependencyInjection
{
    public interface ICommandHandler<T> where T : IParsedCommandArguments
    {
        Task HandleAsync(T arguments, LeafCommand executedCommand);
    }
}