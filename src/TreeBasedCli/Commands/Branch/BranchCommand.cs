using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents a branch command in the command tree, which is a non-terminal
    /// command that serves as a container for subcommands (leaf or branch commands).
    /// Branch commands do not have an associated action, and are used to organize and
    /// structure the CLI's functionality.
    /// </summary>
    public class BranchCommand : Command
    {
        /// <inheritdoc cref="TreeBasedCli.BranchCommand" />
        public BranchCommand(
            string label,
            string[] description,
            IReadOnlyList<Command> childCommands) : base(label, description)
        {
            this.ChildCommands = childCommands;
        }

        internal IReadOnlyList<Command> ChildCommands { get; }

        internal bool TryGetChildCommand(string label, [NotNullWhen(true)] out Command? command)
        {
            command = this.ChildCommands.FirstOrDefault(x => x.Label == label);
            return command != null;
        }
    }
}
