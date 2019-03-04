using System.Collections.Generic;
using System.Linq;
using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    public abstract class Command
    {
        public string Label { get; set; }
        public string[] Description { get; set; }
        public BranchCommand Parent { get; set; }
        public CommandTree Tree { get; set; }

        public string LabelVisibleForUserInConsole
        {
            get
            {
                var result = this.Label.Escape("\"");

                if (result.Contains(" "))
                {
                    result = result.Wrap('"');
                }

                return result;
            }
        }

        public IReadOnlyCollection<Command> Path
        {
            get
            {
                var result = new Stack<Command>();

                for (var command = this; command != null; command = command.Parent)
                {
                    result.Push(command);
                }

                return result;
            }
        }

        public string ConsoleArgumentsRepresentingPath
            => this.Path.Select(x => x.LabelVisibleForUserInConsole).Join(" ");

        public string ConsoleArgumentsRepresentingHelpPath
        {
            get
            {
                var rootLabel = this.Tree.Root.LabelVisibleForUserInConsole;
                var remainingPath = this.Path.Skip(1);

                if (remainingPath.IsEmpty())
                {
                    return $"{rootLabel} help";
                }

                var remainingLabels = remainingPath.Select(x => x.LabelVisibleForUserInConsole).Join(" ");
                return $"{rootLabel} help {remainingLabels}";
            }
        }
    }
}
