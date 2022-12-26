using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TreeBasedCli.Internal;

namespace TreeBasedCli
{
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

        public Command Command { get; }

        public IReadOnlyCollection<string> UserInput { get; }

        public int UserInputWordCount { get; }

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
