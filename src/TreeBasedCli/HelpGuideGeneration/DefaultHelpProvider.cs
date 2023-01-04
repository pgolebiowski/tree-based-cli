using System;
using System.Collections.Generic;
using System.Linq;
using TreeBasedCli.Exceptions;
using TreeBasedCli.Extensions;

namespace TreeBasedCli.HelpGuideGeneration
{
    internal class DefaultHelpProvider : IHelpProvider
    {
        private readonly ArgumentHandlerSettings settings;

        public DefaultHelpProvider(ArgumentHandlerSettings settings)
            => this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        public string ProvideHelp(BranchCommand command, WrongCommandUsageException? exception)
        {
            int lineLengthLimit = Math.Min(Console.WindowWidth - 8, 120);
            var help = new StringBuilderWithLimitedLineLength(lineLengthLimit);

            help.AppendSection(new EmptySection(linesCount: 1));
            help.AppendSection(new NameAndVersionSection(this.settings.Name, this.settings.Version));

            bool thereAreChildCommands = !command.ChildCommands.IsNullOrEmpty();

            AppendErrorSection(
                help,
                exception,
                notImplementedCondition: !thereAreChildCommands,
                notImplementedMessage:
                    $"The command '{command.PathAsExecutableCliPrompt}' " +
                    "is not fully implemented. Code needs to be changed. You can either " +
                    "define child commands or turn this branch command into a leaf command.");

            help.AppendSection(new DescriptionSection(command.Description));

            if (thereAreChildCommands)
            {
                help.AppendSection(new EmptySection(linesCount: 3));

                help.AppendSection(new HeaderSection("Usage:"));
                help.AppendSection(new EmptySection(linesCount: 1));
                help.AppendSection(new ParagraphBodySection(command.PathAsExecutableCliPrompt + " <child command>"));
                help.AppendSection(new EmptySection(linesCount: 3));

                help.AppendSection(new HeaderSection("Child commands:"));
                help.AppendSection(new EmptySection(linesCount: 1));

                int maxChildCommandLabelLength = command.ChildCommands
                    .Select(x => x.Label.Length)
                    .Max();

                command.ChildCommands.ForEach(x =>
                {
                    Command childCommand = x.CurrentElement;

                    help.AppendSection(new TwoColumnParagraphBodySection(
                            firstColumnWidth: maxChildCommandLabelLength,
                            firstColumnOfParagraphs: new[] { childCommand.LabelVisibleForUserInConsole },
                            secondColumnOfParagraphs: childCommand.Description
                        ));

                    if (!x.IsLastIteration)
                    {
                        help.AppendSection(new EmptySection(linesCount: 2));
                    }
                });

                help.AppendSection(new EmptySection(linesCount: 3));

                help.AppendSection(new HeaderSection("For more details on a particular child command, run:"));
                help.AppendSection(new EmptySection(linesCount: 1));
                help.AppendSection(new ParagraphBodySection(command.ConsoleArgumentsRepresentingHelpPath + " <child command>"));
            }

            help.AppendSection(new EmptySection(linesCount: 1));

            return help.ToString();
        }

        public string ProvideHelp(LeafCommand command, WrongCommandUsageException? exception)
        {
            int lineLengthLimit = Math.Min(Console.WindowWidth - 8, 120);
            var help = new StringBuilderWithLimitedLineLength(lineLengthLimit);

            help.AppendSection(new EmptySection(linesCount: 1));
            help.AppendSection(new NameAndVersionSection(this.settings.Name, this.settings.Version));

            AppendErrorSection(help,
                exception,
                notImplementedCondition: false,
                notImplementedMessage: null);

            help.AppendSection(new DescriptionSection(command.Description));
            help.AppendSection(new EmptySection(linesCount: 3));

            bool hasChildOptions = !command.Options.IsNullOrEmpty();

            help.AppendSection(new HeaderSection("Usage:"));
            help.AppendSection(new EmptySection(linesCount: 1));
            help.AppendSection(new ParagraphBodySection(command.PathAsExecutableCliPrompt + (hasChildOptions ? " <options>" : "")));

            if (hasChildOptions)
            {
                help.AppendSection(new EmptySection(linesCount: 3));

                help.AppendSection(new HeaderSection("Options:"));
                help.AppendSection(new EmptySection(linesCount: 1));

                int maxOptionLabelLength = command.Options
                    .Select(x => x.Label.Length)
                    .Max();

                command.Options.ForEach(x =>
                {
                    CommandOption option = x.CurrentElement;

                    help.AppendSection(new TwoColumnParagraphBodySection(
                        firstColumnWidth: maxOptionLabelLength,
                        firstColumnOfParagraphs: new[] { option.Label },
                        secondColumnOfParagraphs: option.Description
                    ));

                    if (!x.IsLastIteration)
                    {
                        help.AppendSection(new EmptySection(linesCount: 2));
                    }
                });
            }

            help.AppendSection(new EmptySection(linesCount: 1));

            return help.ToString();
        }

        private static void AppendErrorSection(
            StringBuilderWithLimitedLineLength help,
            WrongCommandUsageException? exception,
            bool notImplementedCondition,
            string? notImplementedMessage)
        {
            var errors = new List<string>();

            if (exception is not null)
            {
                errors.Add(exception.Message);
            }

            if (notImplementedCondition && (notImplementedMessage is not null))
            {
                errors.Add(notImplementedMessage);
            }

            if (!errors.IsEmpty())
            {
                bool multipleErrors = errors.Count > 1;

                string header = multipleErrors ? "Errors:" : "Error:";
                string summary = multipleErrors
                    ? errors.Select((error, i) => $"{i + 1}) {error}").Join("\n\n")
                    : errors.Join("\n\n");

                help.AppendSection(new HeaderSection(header));
                help.AppendSection(new EmptySection(linesCount: 1));
                help.AppendSection(new ParagraphBodySection(summary));
                help.AppendSection(new EmptySection(linesCount: 3));
            }
        }
    }
}