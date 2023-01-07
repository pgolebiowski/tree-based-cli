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
        /// <inheritdoc cref="TreeBasedCli.LeafCommand{THandler}" />
        public LeafCommand(
            string label,
            string[] description)
                : base(label, description, new CommandOption[] { })
        {
        }

        /// <summary>
        /// Represents the task that will be run when this leaf command is executed. This method executes
        /// the logic of the leaf command using the <typeparamref name="THandler"/> object, which
        /// implements the <see cref="ILeafCommandHandler"/> interface, and has an asynchronous
        /// method <see cref="ILeafCommandHandler.HandleAsync"/>. This method resolves
        /// a <typeparamref name="THandler"/> object using the dependency injection service of the
        /// command tree it belongs to.
        /// </summary>
        public override Task TaskToRun(CommandArguments commandArguments)
        {
            THandler handler = this.DependencyInjectionService.Resolve<THandler>();
            return handler.HandleAsync(this);
        }
    }
}
