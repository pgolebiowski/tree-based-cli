using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace TreeBasedCli
{
    public class BranchCommand : Command
    {
        public BranchCommand(
            string label,
            string[] description,
            IReadOnlyList<Command> childCommands) : base(label, description)
        {
            this.ChildCommands = childCommands;
        }

        public IReadOnlyList<Command> ChildCommands { get; }

        public bool TryGetChildCommand(string label, [NotNullWhen(true)] out Command? command)
        {
            command = this.ChildCommands.FirstOrDefault(x => x.Label == label);
            return command != null;
        }
    }
}
