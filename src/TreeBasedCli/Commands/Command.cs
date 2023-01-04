using System;
using System.Collections.Generic;
using System.Linq;
using TreeBasedCli.Extensions;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents a base class for command objects. This class serves as the base for all
    /// leaf and branch commands in the command tree, and includes properties that
    /// are common to all command types.
    /// </summary>
    public class Command
    {
        /// <inheritdoc cref="Command" />
        /// <param name="label">
        /// The label of the command, which is used to identify it in the command-line interface.
        /// </param>
        /// <param name="description">
        /// The description of the command, which explains what the command does.
        /// This property is an array, because each element is going to be rendered
        /// as a separate paragraph in the help panel.
        /// </param>
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

        /// <summary>
        /// Gets or sets the parent <see cref="BranchCommand" /> of this command.
        /// The value is <see langword="null"/> at the root of the command tree.
        /// </summary>
        internal BranchCommand? Parent { get; set; }

        /// <summary>
        /// Gets the root of this command tree. In particular, it's possible that this will
        /// return a reference to this command, if it is the root of the command tree.
        /// </summary>
        internal Command Root
        {
            get
            {
                Command current = this;
                while (current.Parent is not null)
                {
                    current = current.Parent;
                }
                return current;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CommandTree" /> that this command belongs to.
        /// </summary>
        internal CommandTree? Tree { get; set; }

        /// <summary>
        /// Gets the <see cref="IDependencyInjectionService" /> provided to the
        /// <see cref="CommandTree" /> that this command belongs to. This getter will throw
        /// an exception if: a) this command does not belong to a command tree; or
        /// b) the command tree this command belongs to does not have a reference
        /// to a dependency injection service.
        /// </summary>
        internal IDependencyInjectionService DependencyInjectionService
        {
            get
            {
                if (this.Tree is null)
                {
                    throw new InvalidOperationException(
                        "Cannot find the dependency injection service, because this command " +
                        "does not belong to a command tree.");
                }

                CommandTree tree = this.Tree;

                if (tree.DependencyInjectionService is null)
                {
                    throw new InvalidOperationException(
                        "Cannot find the dependency injection service, because the corresponding " +
                        "command tree was created without one.");
                }

                return tree.DependencyInjectionService;
            }
        }

        /// <summary>
        /// Gets the command in the form of a command-line prompt that can be executed
        /// to invoke this particular command. It does not contain command options.
        /// </summary>
        public string PathAsExecutableCliPrompt
            => this.Path.Select(x => x.LabelVisibleForUserInConsole).Join(" ");

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

        internal string ConsoleArgumentsRepresentingHelpPath
        {
            get
            {
                string rootLabel = this.Root.LabelVisibleForUserInConsole;
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
