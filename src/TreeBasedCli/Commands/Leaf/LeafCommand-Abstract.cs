using System.Collections.Generic;
using System.Threading.Tasks;

namespace TreeBasedCli
{
    /// <summary>
    /// Abstract class representing a leaf command in a command tree.
    /// This is the base class for 3 different leaf command classes: 
    /// 
    /// <para>- <see cref="SimpleLeafCommand" /></para>
    /// <para>- <see cref="LeafCommand{THandler}" /></para>
    /// <para>- <see cref="LeafCommand{TArguments, TParser, THandler}" /></para>
    /// 
    /// Each of these derived classes offers varying levels of customization and functionality.
    /// </summary>
    public abstract class LeafCommand : Command
    {
        /// <inheritdoc cref="LeafCommand" />
        /// <param name="label">The label for the leaf command.</param>
        /// <param name="description">The description for the leaf command.</param>
        /// <param name="options">The list of options for the leaf command.</param>
        public LeafCommand(
            string label,
            string[] description,
            IReadOnlyList<CommandOption> options)
                : base(label, description)
        {
            this.Options = options;
        }

        /// <summary>
        /// Gets the list of options for this leaf command.
        /// </summary>
        public IReadOnlyList<CommandOption> Options { get; }

        /// <summary>
        /// Abstract method representing the task to be run when this leaf command is executed.
        /// </summary>
        public abstract Task TaskToRun(CommandArguments commandArguments);

        internal IReadOnlySet<string> OptionLabels
        {
            get
            {
                var result = new HashSet<string>();

                if (this.Options == null)
                    return result;

                foreach (CommandOption option in this.Options)
                {
                    result.Add(option.Label);
                }

                return result;
            }
        }
    }
}
