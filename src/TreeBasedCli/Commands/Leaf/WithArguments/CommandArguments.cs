using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TreeBasedCli.Exceptions;
using TreeBasedCli.Extensions;

namespace TreeBasedCli
{
    /// <inheritdoc cref="ICommandArguments" />
    public class CommandArguments : ICommandArguments
    {
        private Dictionary<string, IReadOnlyCollection<string>> argumentsAsDictionary;

        /// <inheritdoc cref="ICommandArguments" />
        public CommandArguments(LeafCommand command, IReadOnlyCollection<string> arguments)
        {
            this.Command = command ?? throw new ArgumentNullException(nameof(command));
            this.Arguments = arguments ?? Array.Empty<string>();

            this.argumentsAsDictionary = this.ProcessArgumentsAsDictionary();
        }

        /// <inheritdoc cref="ICommandArguments.Arguments" />
        public IReadOnlyCollection<string> Arguments { get; }

        /// <inheritdoc cref="ICommandArguments.Command" />
        public LeafCommand Command { get; }

        /// <inheritdoc cref="ICommandArguments.ContainsArgument(string)" />
        public bool ContainsArgument(string optionLabel)
        {
            return this.argumentsAsDictionary.ContainsKey(optionLabel);
        }

        /// <inheritdoc cref="ICommandArguments.TryGetArgument(string, out IUserInputToCommandOption?)" />
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

        /// <inheritdoc cref="ICommandArguments.GetArgumentOrNull(string)" />
        public IUserInputToCommandOption? GetArgumentOrNull(string optionLabel)
        {
            if (this.TryGetArgument(optionLabel, out IUserInputToCommandOption? result))
            {
                return result;
            }

            return null;
        }

        /// <inheritdoc cref="ICommandArguments.GetArgument(string)" />
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
