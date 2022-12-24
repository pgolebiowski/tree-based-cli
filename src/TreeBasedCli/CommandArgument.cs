using System;
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
        private readonly string optionLabel;

        /// <summary>
        /// The raw value of the argument, as provided by the user.
        /// </summary>
        private readonly IReadOnlyCollection<string> raw;

        public CommandArgument(Command command, string optionLabel, IReadOnlyCollection<string> raw)
        {
            this.Command = command;
            this.optionLabel = optionLabel;
            this.raw = raw;

            this.Count = this.raw.Count;
        }

        public Command Command { get; }

        public int Count { get; }

        public IReadOnlyCollection<string> Values => this.raw;

        /// <summary>
        /// Gets the single value of this argument, throwing a <see cref="WrongCommandUsageException" />
        /// if the number of values is not equal to 1.
        /// </summary>
        public string ExpectedAsSingleValue()
        {
            if (this.Count != 1)
            {
                string countingWord = this.Count == 0 ? "none was" : $"{this.Count} were";

                ThrowHelper.WrongCommandUsage(
                    this.Command,
                    $"The command '{this.Command.ConsoleArgumentsRepresentingPath}' ",
                    $"expects a single value for the option '{this.optionLabel}' ",
                    $"but {countingWord} provided.");
            }

            return this.raw.First();
        }

        /// <summary>
        /// Gets the single value of this argument, interpreted as a path to an existing file.
        /// This method throws a <see cref="WrongCommandUsageException" /> if the number of values is
        /// not equal to 1. It throws a <see cref="MessageOnlyException" /> if the specified file does not exist.
        /// </summary>
        public string ExpectedAsSinglePathToExistingFile()
        {
            string path = this.ExpectedAsSingleValue();

            if (!File.Exists(path))
            {
                ThrowHelper.MessageOnlyException(
                    $"The file '{path}' does not exist.");
            }

            return path;
        }

        public string ExpectedAsSinglePathToExistingDirectory()
        {
            string path = this.ExpectedAsSingleValue();

            if (!Directory.Exists(path))
            {
                ThrowHelper.MessageOnlyException(
                    $"The directory '{path}' does not exist.");
            }

            return path;
        }

        public int ExpectedAsSingleInteger()
        {
            string value = this.ExpectedAsSingleValue();

            if (int.TryParse(value, out int result))
            {
                return result;
            }

            throw new MessageOnlyException($"Could not parse '{value}' as an integer.");
        }

        public TEnum ExpectedAsEnumValue<TEnum>() where TEnum : struct, Enum
        {
            string value = this.ExpectedAsSingleValue();

            try
            {
                return Enum.Parse<TEnum>(value);
            }
            catch
            {
                string availableValues = string.Join(", ", Enum.GetValues<TEnum>());

                throw ThrowHelper.WrongCommandUsage(
                    this.Command,
                    $"There is no enum value for {typeof(TEnum)} that maps to '{value}'. ",
                    $"Available values are: [ {availableValues} ].");
            }
        }
    }
}
