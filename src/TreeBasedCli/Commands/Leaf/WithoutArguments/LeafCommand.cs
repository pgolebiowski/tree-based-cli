using System.Threading.Tasks;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents a leaf command that comes with no options, and thus also no arguments or parser.
    /// However, it comes with a handler that leverages dependency injection.
    /// Derive from this class for the simplest kinds of leaf commands, which require no parameters,
    /// but the task logic does have a dependency on other existing objects.
    /// </summary>
    public class LeafCommand<THandler> : LeafCommand
        where THandler : ILeafCommandHandler
    {
        private readonly IDependencyInjectionService dependencyInjectionService;

        /// <inheritdoc cref="TreeBasedCli.LeafCommand{THandler}" />
        public LeafCommand(
            string label,
            string[] description,
            IDependencyInjectionService dependencyInjectionService)
                : base(label, description, new CommandOption[] { })
        {
            this.dependencyInjectionService = dependencyInjectionService;
        }

        public override Task TaskToRun(CommandArguments commandArguments)
        {
            THandler handler = this.dependencyInjectionService.Resolve<THandler>();
            return handler.HandleAsync(this);
        }
    }
}
