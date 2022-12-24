using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeBasedCli
{
    public class FishShellCompletionScriptBuilder
    {
        private readonly List<string> lines;

        public FishShellCompletionScriptBuilder()
        {
            this.lines = new List<string>();

            this.AddFishUsingCommandFunction();
        }

        private void AddFishUsingCommandFunction()
        {
            this.lines.Add("#!/usr/bin/env fish");
            this.lines.Add("");

            this.lines.Add(@"
            function __fish_using_command
                set cmd (commandline -opc)
                if [ (count $cmd) -eq (count $argv) ]
                    for i in (seq (count $argv))
                        if [ $cmd[$i] != $argv[$i] ]
                            return 1
                        end
                    end
                    return 0
                end
                return 1
            end");
            this.lines.Add("");
        }

        public void AddCommandTree(Command rootCommand)
        {
            if (rootCommand is BranchCommand branchCommand)
            {
                var stack = new Stack<Command>();

                foreach (Command childCommand in branchCommand.ChildCommands)
                {
                    stack.Push(childCommand);
                }

                while (stack.Any())
                {
                    Command currentCommand = stack.Pop();

                    IReadOnlyCollection<Command> pathWithoutCurrentCommand = currentCommand.Path
                        .TakeWhile(x => x != currentCommand)
                        .ToArray();

                    this.AddSubcommand(rootCommand, pathWithoutCurrentCommand, currentCommand);

                    if (currentCommand is LeafCommand leafCommand)
                    {
                        this.AddCommandOptions(rootCommand, leafCommand);
                    }

                    if (currentCommand is BranchCommand currentCommandBranchCommand)
                    {
                        foreach (Command childCommand in currentCommandBranchCommand.ChildCommands)
                        {
                            stack.Push(childCommand);
                        }
                    }
                }
            }
            else
            {
                throw new Exception("The root command can only be a branch command.");
            }
        }

        private void AddSubcommand(Command rootCommand, IReadOnlyCollection<Command> path, Command childCommand)
        {
            string usingCommandArgument = string.Join(" ", path.Select(x => x.Label));
            string description = string.Join(" ", childCommand.Description).Replace("\"", "\\\"");

            this.lines.Add(
                "complete " +
                $"--command {rootCommand.Label} " +
                $"--condition \"__fish_using_command {usingCommandArgument}\" " +
                "--no-files " +
                $"--arguments {childCommand.Label} " +
                $"--description \"{description}\""
            );
        }

        private void AddCommandOptions(Command rootCommand, LeafCommand command)
        {
            if (command.Options is null)
            {
                return;
            }

            string usingCommandArgument = string.Join(" ", command.Path.Select(x => x.Label));

            foreach (CommandOption option in command.Options)
            {
                string description = string.Join(" ", option.Description).Replace("\"", "\\\"");

                this.lines.Add(
                    "complete " +
                    $"--command {rootCommand.Label} " +
                    $"--condition \"__fish_using_command {usingCommandArgument}\" " +
                    "--no-files " +
                    $"--long-option {option.Label.TrimStart('-')} " +
                    $"--description \"{description}\""
                );
            }
        }

        public string Build()
        {
            return string.Join(Environment.NewLine, this.lines);
        }
    }
}