using System.Collections.Generic;
using System.Linq;
using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents a command that can be executed in a command-line interface.
    /// </summary>
    public class Command
    {
        public Command(string label, string[] description)
        {
            this.Label = label;
            this.Description = description;
        }

        /// <summary>
        /// Gets the label of the command, which is used to identify it in the command-line interface.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Gets the description of the command, which explains what the command does.
        /// This property is an array, because each element is a going to be rendered
        /// as a separate paragraph in the help panel.
        /// </summary>
        public string[] Description { get; }

        internal BranchCommand? Parent { get; set; }
        internal CommandTree? Tree { get; set; }

        internal string LabelVisibleForUserInConsole
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

        internal IReadOnlyCollection<Command> Path
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

        internal string ConsoleArgumentsRepresentingPath
            => this.Path.Select(x => x.LabelVisibleForUserInConsole).Join(" ");

        internal string ConsoleArgumentsRepresentingHelpPath
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
