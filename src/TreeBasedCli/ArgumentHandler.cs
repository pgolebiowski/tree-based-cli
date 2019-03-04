using System;
using System.Collections.Generic;
using System.Linq;
using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    public class ArgumentHandler
    {
        private readonly ArgumentHandlerSettings settings;

        private static HashSet<string> ExplicitHelpRequestIndicators = new HashSet<string>
        {
            "help",
            "--help",
            "-h"
        };

        public ArgumentHandler(ArgumentHandlerSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            this.Validate();
        }

        private void Validate()
        {
            if (this.settings.CommandTree == null)
            {
                throw new ArgumentException("There is no command tree.");
            }

            var root = this.settings.CommandTree.Root;

            if (root == null)
            {
                throw new ArgumentException("The command tree does not have a root.");
            }

            var overridesHelp = false;

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

        public void Handle(IReadOnlyCollection<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            try
            {
                if (ExplicitlyRequestedHelp(arguments, out var helpArguments))
                {
                    this.PrintHelp(helpArguments);
                }
                else
                {
                    this.RunCommand(arguments);
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
            this.DetermineTargetCommand(arguments, out var targetCommand, out var notConsumedArguments);
            this.PrintHelp(targetCommand);
        }

        private void RunCommand(IReadOnlyCollection<string> arguments)
        {
            this.DetermineTargetCommand(arguments, out var targetCommand, out var notConsumedArguments);

            if (targetCommand is BranchCommand branchCommand)
            {
                this.PrintHelp(branchCommand);
            }
            else if (targetCommand is LeafCommand leafCommand)
            {
                if (leafCommand.Action == null)
                {
                    throw ThrowHelper.MissingCommandImplementation(
                        leafCommand,
                        $"The command '{leafCommand.ConsoleArgumentsRepresentingPath}' ",
                        $"does not have an assigned action to invoke.");
                }

                var commandArguments = new CommandArguments(leafCommand, notConsumedArguments);

                leafCommand.Action.Invoke(commandArguments);
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

            var consumedArguments = 0;

            foreach (var argument in arguments)
            {
                if (targetCommand is LeafCommand leafCommand)
                {
                    targetCommand = leafCommand;
                    notConsumedArguments = arguments.Skip(consumedArguments).ToArray();
                    return;
                }
                else if (targetCommand is BranchCommand branchCommand)
                {
                    if (branchCommand.TryGetChildCommand(argument, out var childCommand))
                    {
                        consumedArguments++;
                        targetCommand = childCommand;
                        continue;
                    }
                    else
                    {
                        throw ThrowHelper.WrongCommandUsage(
                            branchCommand,
                            $"The command '{branchCommand.ConsoleArgumentsRepresentingPath}' " +
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
                    var help = this.settings.HelpProvider.ProvideHelp(leafCommand, exception);
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
