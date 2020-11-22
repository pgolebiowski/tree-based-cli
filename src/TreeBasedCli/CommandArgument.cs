using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents the value provided as an argument to a single
    /// command option. In general, it is a sequence of strings.
    /// However, the user may have custom requirements on the
    /// format of this sequence (e.g. in particular, it may be
    /// expected to be a single value only). This class is intended
    /// to facilitate parsing of most common types of arguments.
    /// </summary>
    public partial class CommandArgument : ICommandArgument
    {
        private readonly Command command;
        private readonly string optionLabel;
        private readonly IReadOnlyCollection<string> raw;

        public CommandArgument(Command command, string optionLabel, IReadOnlyCollection<string> raw)
        {
            this.command = command;
            this.optionLabel = optionLabel;
            this.raw = raw;

            this.Count = this.raw.Count;
        }

        public int Count { get; }

        public string ExpectedAsSingleValue()
        {
            if (this.Count != 1)
            {
                var countingWord = this.Count == 0 ? "none was" : $"{this.Count} were";

                ThrowHelper.WrongCommandUsage(
                    this.command,
                    $"The command '{this.command.ConsoleArgumentsRepresentingPath}' ",
                    $"expects a single value for the option '{this.optionLabel}' ",
                    $"but {countingWord} provided.");
            }

            return this.raw.First();
        }

        public string ExpectedAsSinglePathToExistingFile()
        {
            var path = this.ExpectedAsSingleValue();

            if (!File.Exists(path))
            {
                ThrowHelper.MessageOnlyException(
                    $"The file '{path}' does not exist.");
            }

            return path;
        }

        public string ExpectedAsSinglePathToExistingDirectory()
        {
            var path = this.ExpectedAsSingleValue();

            if (!Directory.Exists(path))
            {
                ThrowHelper.MessageOnlyException(
                    $"The directory '{path}' does not exist.");
            }

            return path;
        }

        public int ExpectedAsSingleInteger()
        {
            var value = this.ExpectedAsSingleValue();

            if (int.TryParse(value, out var result))
            {
                return result;
            }

            throw new MessageOnlyException($"Could not parse '{value}' as an integer.");
        }

        public IEnumerator<string> GetEnumerator() => this.raw.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.raw.GetEnumerator();
    }
}
