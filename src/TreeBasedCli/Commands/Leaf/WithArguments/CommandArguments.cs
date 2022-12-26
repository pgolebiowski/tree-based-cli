using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TreeBasedCli.Exceptions;
using TreeBasedCli.Extensions;

namespace TreeBasedCli
{
    public class CommandArguments : ICommandArguments
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

        public bool ContainsArgument(string optionLabel)
        {
            return this.argumentsAsDictionary.ContainsKey(optionLabel);
        }

        public bool TryGetArgument(string optionLabel, [NotNullWhen(true)] out IUserInputToCommandOption? result)
        {
            if (this.argumentsAsDictionary.TryGetValue(optionLabel, out IReadOnlyCollection<string>? found))
            {
                result = new UserInputToCommandOption(this.Command, optionLabel, found);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public IUserInputToCommandOption? GetArgumentOrNull(string optionLabel)
        {
            if (this.TryGetArgument(optionLabel, out IUserInputToCommandOption? result))
            {
                return result;
            }

            return null;
        }

        public IUserInputToCommandOption GetArgument(string optionLabel)
        {
            IUserInputToCommandOption? found = this.GetArgumentOrNull(optionLabel);

            if (found != null)
            {
                return found;
            }

            throw ThrowHelper.WrongCommandUsage(
                this.Command,
                $"The command '{this.Command.PathAsExecutableCliPrompt}' ",
                $"requires the option '{optionLabel}' to be specified.");
        }

        private Dictionary<string, IReadOnlyCollection<string>> ProcessArgumentsAsDictionary()
        {
            var result = new Dictionary<string, IReadOnlyCollection<string>>();

            if (this.Arguments.IsEmpty())
            {
                return result;
            }

            IReadOnlySet<string> labels = this.Command.OptionLabels;

            if (labels.IsEmpty())
            {
                ThrowHelper.WrongCommandUsage(
                    this.Command,
                    $"The command '{this.Command.PathAsExecutableCliPrompt}' ",
                    "has not been designed to be invoked with arguments.");
            }

            var arguments = new Queue<string>(this.Arguments);
            string currentOption = arguments.Dequeue();

            if (!labels.Contains(currentOption))
            {
                ThrowHelper.WrongCommandUsage(
                    this.Command,
                    $"The command '{this.Command.PathAsExecutableCliPrompt}' ",
                    $"does not have an option '{currentOption}'.");
            }

            var currentOptionValues = new List<string>();

            while (!arguments.IsEmpty())
            {
                string front = arguments.Dequeue();

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