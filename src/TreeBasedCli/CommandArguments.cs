using System;
using System.Collections.Generic;
using System.Linq;
using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    public class CommandArguments
    {
        private Dictionary<string, IReadOnlyCollection<string>> argumentsAsDictionary;

        public CommandArguments(LeafCommand command, IReadOnlyCollection<string> arguments)
        {
            this.Command = command ?? throw new ArgumentNullException(nameof(command));
            this.Arguments = arguments ?? Array.Empty<string>();

            this.argumentsAsDictionary = this.ProcessArgumentsAsDictionary();
        }

        public IReadOnlyCollection<string> Arguments { get; }
        public LeafCommand Command { get; }

        public bool TryGet(string optionLabel, out CommandArgument result)
        {
            if (this.argumentsAsDictionary.TryGetValue(optionLabel, out var found))
            {
                result = new CommandArgument(this.Command, optionLabel, found);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public CommandArgument GetOrNull(string optionLabel)
        {
            if (this.TryGet(optionLabel, out var result))
            {
                return result;
            }

            return null;
        }

        public CommandArgument GetArgument(string optionLabel)
        {
            var found = this.GetOrNull(optionLabel);
            
            if (found != null)
            {
                return found;
            }

            throw ThrowHelper.WrongCommandUsage(
                this.Command,
                $"The command '{this.Command.ConsoleArgumentsRepresentingPath}' ",
                $"requires the option '{optionLabel}' to be specified.");
        }

        private Dictionary<string, IReadOnlyCollection<string>> ProcessArgumentsAsDictionary()
        {
            var result = new Dictionary<string, IReadOnlyCollection<string>>();

            if (this.Arguments.IsEmpty())
            {
                return result;
            }

            var labels = this.Command.OptionLabels;

            if (labels.IsEmpty())
            {
                ThrowHelper.WrongCommandUsage(
                    this.Command,
                    $"The command '{this.Command.ConsoleArgumentsRepresentingPath}' ",
                    "has not been designed to be invoked with arguments.");
            }

            var arguments = new Queue<string>(this.Arguments);
            var currentOption = arguments.Dequeue();

            if (!labels.Contains(currentOption))
            {
                ThrowHelper.WrongCommandUsage(
                    this.Command,
                    $"The command '{this.Command.ConsoleArgumentsRepresentingPath}' ",
                    $"does not have an option '{currentOption}'.");
            }

            var currentOptionValues = new List<string>();

            while (!arguments.IsEmpty())
            {
                var front = arguments.Dequeue();

                if (labels.Contains(front))
                {
                    result.Add(currentOption, currentOptionValues);
                    currentOption = front;
                    currentOptionValues = new List<string>();
                    continue;
                }

                currentOptionValues.Add(front);
            }

            result.Add(currentOption, currentOptionValues);
            return result;
        }
    }
}
