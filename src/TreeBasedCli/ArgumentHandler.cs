using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeBasedCli.Exceptions;
using TreeBasedCli.Extensions;

namespace TreeBasedCli
{
    /// <summary>
    /// The main class responsible for parsing and executing commands in a command-line interface.
    /// It acts as the entry point to the program, which takes raw input arguments provided by the user.
    /// </summary>
    public class ArgumentHandler
    {
        private readonly ArgumentHandlerSettings settings;

        private static IReadOnlySet<string> ExplicitHelpRequestIndicators = new HashSet<string>
        {
            "help",
            "--help",
            "-h"
        };

        /// <summary>
        /// Creates a new instance of <see cref="ArgumentHandler"/>.
        /// </summary>
        /// <param name="settings">
        /// The settings for the command-line interface, including the name,
        /// version, command tree, and help provider.
        /// </param>
        public ArgumentHandler(ArgumentHandlerSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            this.Validate();
        }

        /// <summary>
        /// Validates the settings for the command-line interface, including
        /// the root command in the command tree and the presence of any explicit help options in commands.
        /// </summary>
        private void Validate()
        {
            if (this.settings.CommandTree == null)
            {
                throw new ArgumentException("There is no command tree.");
            }

            Command root = this.settings.CommandTree.Root;

            if (root == null)
            {
                throw new ArgumentException("The command tree does not have a root.");
            }

            bool overridesHelp = false;

            if (root is BranchCommand branchCommand)
            {
                overridesHelp = branchCommand.ChildCommands
                    ?.Any(x => ExplicitHelpRequestIndicators.Contains(x.Label)) == true;
            }
            else if (root is LeafCommand leafCommand)
            {
                overridesHelp = leafCommand.Options
                    ?.Any(x => ExplicitHelpRequestIndicators.Contains(x.Label)) == true;
            }
            else
            {
                ThrowHelper.UnrecognizedType(root);
            }

            if (overridesHelp)
            {
                ThrowHelper.Exception(
                    "Providing the help command explicitly is forbidden, as it collides ",
                    "with the in-built mechanism. Please don't use the following labels ",
                    $"for the root command: [ {ExplicitHelpRequestIndicators.Join(", ")} ].");
            }
        }

        /// <summary>
        /// Handles the input arguments for the command-line interface, either by running
        /// the requested command or displaying help information.
        /// </summary>
        /// <param name="arguments">The input arguments for the command-line interface.</param>
        /// <returns>A task representing the asynchronous execution of the command or help display.</returns>
        public async Task HandleAsync(IReadOnlyCollection<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            try
            {
                if (ExplicitlyRequestedHelp(arguments, out IReadOnlyCollection<string> helpArguments))
                {
                    this.PrintHelp(helpArguments);
                }
                else
                {
                    await this.RunCommandAsync(arguments);
                }
            }
            catch (WrongCommandUsageException exception)
            {
                this.PrintHelp(exception.Command, exception);
            }
        }

        private static bool ExplicitlyRequestedHelp(
            IReadOnlyCollection<string> arguments,
            out IReadOnlyCollection<string> remainingArguments)
        {
            if (arguments.IsNullOrEmpty())
            {
                remainingArguments = default;
                return false;
            }

            if (ExplicitHelpRequestIndicators.Contains(arguments.First()))
            {
                remainingArguments = arguments.Skip(1).ToArray();
                return true;
            }
            else
            {
                remainingArguments = default;
                return false;
            }
        }

        private void PrintHelp(IReadOnlyCollection<string> arguments)
        {
            this.DetermineTargetCommand(arguments, out Command targetCommand, out IReadOnlyCollection<string> notConsumedArguments);
            this.PrintHelp(targetCommand);
        }

        private async Task RunCommandAsync(IReadOnlyCollection<string> arguments)
        {
            this.DetermineTargetCommand(arguments, out Command targetCommand, out IReadOnlyCollection<string> notConsumedArguments);

            if (targetCommand is BranchCommand branchCommand)
            {
                this.PrintHelp(branchCommand);
            }
            else if (targetCommand is LeafCommand leafCommand)
            {
                var commandArguments = new CommandArguments(leafCommand, notConsumedArguments);
                await leafCommand.TaskToRun(commandArguments);
            }
            else
            {
                ThrowHelper.UnrecognizedType(targetCommand);
            }
        }

        private void DetermineTargetCommand(
            IReadOnlyCollection<string> arguments,
            out Command targetCommand,
            out IReadOnlyCollection<string> notConsumedArguments)
        {
            targetCommand = this.settings.CommandTree.Root;

            if (arguments.IsNullOrEmpty())
            {
                notConsumedArguments = default;
                return;
            }

            int consumedArguments = 0;

            foreach (string argument in arguments)
            {
                if (targetCommand is LeafCommand leafCommand)
                {
                    targetCommand = leafCommand;
                    notConsumedArguments = arguments.Skip(consumedArguments).ToArray();
                    return;
                }
                else if (targetCommand is BranchCommand branchCommand)
                {
                    if (branchCommand.TryGetChildCommand(argument, out Command? childCommand))
                    {
                        consumedArguments++;
                        targetCommand = childCommand;
                        continue;
                    }
                    else
                    {
                        throw ThrowHelper.WrongCommandUsage(
                            branchCommand,
                            $"The command '{branchCommand.PathAsExecutableCliPrompt}' " +
                            $"does not have a child command '{argument}'.");
                    }
                }
                else
                {
                    ThrowHelper.UnrecognizedType(targetCommand);
                }
            }

            // if this code is reached, all the arguments are valid, but they lead
            // to a branch command

            notConsumedArguments = default;
        }

        private void PrintHelp(Command command, WrongCommandUsageException exception = null)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            switch (command)
            {
                case LeafCommand leafCommand:
                    string help = this.settings.HelpProvider.ProvideHelp(leafCommand, exception);
                    Console.WriteLine(help);
                    break;

                case BranchCommand branchCommand:
                    help = this.settings.HelpProvider.ProvideHelp(branchCommand, exception);
                    Console.WriteLine(help);
                    break;

                default:
                    throw ThrowHelper.UnrecognizedType(command);
            }
        }
    }
}
