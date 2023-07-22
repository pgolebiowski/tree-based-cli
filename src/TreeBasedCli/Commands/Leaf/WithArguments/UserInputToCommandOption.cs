using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TreeBasedCli.Exceptions;

namespace TreeBasedCli
{
    /// <inheritdoc cref="IUserInputToCommandOption" />
    public partial class UserInputToCommandOption : IUserInputToCommandOption
    {
        private readonly string optionLabel;

        /// <inheritdoc cref="IUserInputToCommandOption" />
        public UserInputToCommandOption(
            Command command, string optionLabel, IReadOnlyCollection<string> userInput)
        {
            this.Command = command;

            this.optionLabel = optionLabel;

            this.UserInput = userInput;
            this.UserInputWordCount = userInput.Count;
        }

        /// <inheritdoc cref="IUserInputToCommandOption.Command" />
        public Command Command { get; }

        /// <inheritdoc cref="IUserInputToCommandOption.UserInput" />
        public IReadOnlyCollection<string> UserInput { get; }

        /// <inheritdoc cref="IUserInputToCommandOption.UserInputWordCount" />
        public int UserInputWordCount { get; }

        /// <inheritdoc cref="IUserInputToCommandOption.ExpectedAsSingleValue" />
        public string ExpectedAsSingleValue()
        {
            if (this.UserInputWordCount != 1)
            {
                string countingWord = this.UserInputWordCount == 0 ? "none was" : $"{this.UserInputWordCount} were";

                ThrowHelper.WrongCommandUsage(
                    this.Command,
                    $"The command '{this.Command.PathAsExecutableCliPrompt}' ",
                    $"expects a single value for the option '{this.optionLabel}' ",
                    $"but {countingWord} provided.");
            }

            return this.UserInput.First();
        }

        /// <inheritdoc cref="IUserInputToCommandOption.ExpectedAsSinglePathToExistingFile" />
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

        /// <inheritdoc cref="IUserInputToCommandOption.ExpectedAsSinglePathToExistingDirectory" />
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

        /// <inheritdoc cref="IUserInputToCommandOption.ExpectedAsSingleInteger" />
        public int ExpectedAsSingleInteger()
        {
            string value = this.ExpectedAsSingleValue();

            if (int.TryParse(value, out int result))
            {
                return result;
            }

            throw new MessageOnlyException($"Could not parse '{value}' as an integer.");
        }

        /// <inheritdoc cref="IUserInputToCommandOption.ExpectedAsEnumValue{TEnum}" />
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

        /// <inheritdoc cref="IUserInputToCommandOption.ExpectedAsEnumValues{TEnum}" />
        public IReadOnlySet<TEnum> ExpectedAsEnumValues<TEnum>() where TEnum : struct, Enum
        {
            var parsedEnums = new HashSet<TEnum>();

            foreach (string value in this.UserInput)
            {
                try
                {
                    TEnum parsedEnum = Enum.Parse<TEnum>(value);
                    parsedEnums.Add(parsedEnum);
                }
                catch
                {
                    string availableValues = string.Join(", ", Enum.GetValues<TEnum>());

                    throw new WrongCommandUsageException(
                        this.Command,
                        $"There is no enum value for {typeof(TEnum)} that maps to '{value}'. " +
                        $"Available values are: [ {availableValues} ].");
                }
            }

            return parsedEnums;
        }
    }
}
