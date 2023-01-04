using System.Threading.Tasks;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents a leaf command that comes with no options, and thus also no arguments or parser.
    /// Further, it does not come with a handler, and thus does not leverage dependency injection.
    /// Derive from this class for the simplest kinds of leaf commands, which require no parameters,
    /// and the task logic does not have a dependency on other existing objects.
    /// </summary>
    public abstract class SimpleLeafCommand : LeafCommand
    {
        /// <inheritdoc cref="SimpleLeafCommand" />
        public SimpleLeafCommand(
            string label,
            string[] description)
                : base(label, description, new CommandOption[] { })
        { }

        /// <summary>
        /// Abstract method representing the task to be run when this leaf command is executed.
        /// This method does not accept any arguments, as this leaf command has no options or dependencies.
        /// </summary>
        public abstract Task TaskToRun();

        public override Task TaskToRun(CommandArguments commandArguments)
        {
            return this.TaskToRun();
        }
    }
}
