using System.Collections.Generic;
using System.Linq;
using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    public class Command
    {
        public Command(string label, string[] description)
        {
            this.Label = label;
            this.Description = description;
        }

        public string Label { get; }
        public string[] Description { get; }

        internal BranchCommand? Parent { get; set; }
        internal CommandTree? Tree { get; set; }

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

                for (Command? command = this; command != null; command = command.Parent)
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
