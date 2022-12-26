using System;
using System.Collections.Generic;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents the input that the user provided to a single command option as
    /// a sequence of space-separated texts. It facilitates parsing of the most
    /// common types of user arguments, including: a single-word text, an integer,
    /// a path to an existing file, a path to an existing directory.
    /// </summary>
    public interface IUserInputToCommandOption
    {
        /// <summary>
        /// The command to which the corresponding option belongs.
        /// </summary>
        Command Command { get; }

        /// <summary>
        /// Gets the input provided by the user to this command option as a read-only collection of strings.
        /// The input consists of a sequence of space-separated texts.
        /// </summary>
        IReadOnlyCollection<string> UserInput { get; }

        /// <summary>
        /// Gets the number of words in the user input provided to this command option.
        /// </summary>
        int UserInputWordCount { get; }

        /// <summary>
        /// Gets the single value of this argument, throwing a <see cref="WrongCommandUsageException" />
        /// if the number of values is not equal to 1.
        /// </summary>
        string ExpectedAsSingleValue();

        /// <summary>
        /// Gets the single value of this argument, interpreted as a path to an existing file.
        /// This method throws a <see cref="WrongCommandUsageException" /> if the number of values is not equal to 1.
        /// It throws a <see cref="MessageOnlyException" /> if the specified file does not exist.
        /// </summary>
        string ExpectedAsSinglePathToExistingFile();

        /// <summary>
        /// Gets the single value of this argument, interpreted as a path to an existing directory.
        /// This method throws a <see cref="WrongCommandUsageException" /> if the number of values is not equal to 1.
        /// It throws a <see cref="MessageOnlyException" /> if the specified directory does not exist.
        /// </summary>
        string ExpectedAsSinglePathToExistingDirectory();

        /// <summary>
        /// Gets the single value of this argument, interpreted as an integer.
        /// This method throws a <see cref="WrongCommandUsageException" /> if the number of values is not equal to 1.
        /// It throws a <see cref="MessageOnlyException" /> if the value cannot be parsed as an integer.
        /// </summary>
        int ExpectedAsSingleInteger();

        /// <summary>
        /// Gets the single value of this argument, interpreted as a value of the specified enumeration type.
        /// This method throws a <see cref="WrongCommandUsageException" /> if the number of values is not equal to 1.
        /// It throws a <see cref="MessageOnlyException" /> if the value cannot be mapped to a value of the specified enumeration type.
        /// </summary>
        TEnum ExpectedAsEnumValue<TEnum>() where TEnum : struct, Enum;
    }
}
