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
                string result = this.Label.Escape("\"");

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

                for (Command command = this; command != null; command = command.Parent)
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
                string rootLabel = this.Tree.Root.LabelVisibleForUserInConsole;
                IEnumerable<Command> remainingPath = this.Path.Skip(1);

                if (remainingPath.IsEmpty())
                {
                    return $"{rootLabel} help";
                }

                string remainingLabels = remainingPath.Select(x => x.LabelVisibleForUserInConsole).Join(" ");
                return $"{rootLabel} help {remainingLabels}";
            }
        }
    }
}
