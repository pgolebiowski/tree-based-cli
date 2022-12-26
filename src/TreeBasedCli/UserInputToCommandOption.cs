using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents the input that the user provided to a single command option as
    /// a sequence of space-separated texts. It facilitates parsing of the most
    /// common types of user arguments, including: a single-word text, an integer,
    /// a path to an existing file, a path to an existing directory.
    /// </summary>
    public partial class UserInputToCommandOption : IUserInputToCommandOption
    {
        private readonly string optionLabel;

        public UserInputToCommandOption(
            Command command, string optionLabel, IReadOnlyCollection<string> userInput)
        {
            this.Command = command;

            this.optionLabel = optionLabel;

            this.UserInput = userInput;
            this.UserInputWordCount = userInput.Count;
        }

        /// <summary>
        /// The command to which the corresponding option belongs.
        /// </summary>
        public Command Command { get; }

        /// <summary>
        /// Gets the input provided by the user to this command option as a read-only collection of strings.
        /// The input consists of a sequence of space-separated texts.
        /// </summary>
        public IReadOnlyCollection<string> UserInput { get; }

        /// <summary>
        /// Gets the number of words in the user input provided to this command option.
        /// </summary>
        public int UserInputWordCount { get; }

        /// <summary>
        /// Gets the single value of this argument, throwing a <see cref="WrongCommandUsageException" />
        /// if the number of values is not equal to 1.
        /// </summary>
        public string ExpectedAsSingleValue()
        {
            if (this.UserInputWordCount != 1)
            {
                string countingWord = this.UserInputWordCount == 0 ? "none was" : $"{this.UserInputWordCount} were";

                ThrowHelper.WrongCommandUsage(
                    this.Command,
                    $"The command '{this.Command.ConsoleArgumentsRepresentingPath}' ",
                    $"expects a single value for the option '{this.optionLabel}' ",
                    $"but {countingWord} provided.");
            }

            return this.UserInput.First();
        }

        /// <summary>
        /// Gets the single value of this argument, interpreted as a path to an existing file.
        /// This method throws a <see cref="WrongCommandUsageException" /> if the number of values is not equal to 1.
        /// It throws a <see cref="MessageOnlyException" /> if the specified file does not exist.
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

        /// <summary>
        /// Gets the single value of this argument, interpreted as a path to an existing directory.
        /// This method throws a <see cref="WrongCommandUsageException" /> if the number of values is not equal to 1.
        /// It throws a <see cref="MessageOnlyException" /> if the specified directory does not exist.
        /// </summary>
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

        /// <summary>
        /// Gets the single value of this argument, interpreted as an integer.
        /// This method throws a <see cref="WrongCommandUsageException" /> if the number of values is not equal to 1.
        /// It throws a <see cref="MessageOnlyException" /> if the value cannot be parsed as an integer.
        /// </summary>
        public int ExpectedAsSingleInteger()
        {
            string value = this.ExpectedAsSingleValue();

            if (int.TryParse(value, out int result))
            {
                return result;
            }

            throw new MessageOnlyException($"Could not parse '{value}' as an integer.");
        }

        /// <summary>
        /// Gets the single value of this argument, interpreted as a value of the specified enumeration type.
        /// This method throws a <see cref="WrongCommandUsageException" /> if the number of values is not equal to 1.
        /// It throws a <see cref="MessageOnlyException" /> if the value cannot be mapped to a value of the specified enumeration type.
        /// </summary>
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
