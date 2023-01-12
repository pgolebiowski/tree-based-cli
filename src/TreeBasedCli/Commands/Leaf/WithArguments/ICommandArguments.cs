using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents the input arguments provided by the user to a command.
    /// </summary>
    public interface ICommandArguments
    {
        /// <summary>
        /// Gets the input arguments provided by the user, as a collection of space-delimited words.
        /// </summary>
        IReadOnlyCollection<string> Arguments { get; }

        /// <summary>
        /// Gets the command to which the arguments belongs.
        /// </summary>
        LeafCommand Command { get; }

        /// <summary>
        /// Determines whether the arguments contain an option with the specified label.
        /// </summary>
        /// <param name="optionLabel">The label of the option to search for.</param>
        bool ContainsArgument(string optionLabel);

        /// <summary>
        /// Tries to get the value of an option provided by the user.
        /// </summary>
        /// <param name="optionLabel">The label of the option for which to get the value.</param>
        /// <param name="result">If successful, the value of the option provided by the user.</param>
        bool TryGetArgument(string optionLabel, [NotNullWhen(true)] out IUserInputToCommandOption? result);

        /// <summary>
        /// Get the value of an option provided by the user or <see langword="null"/>
        /// if the option was not found.
        /// </summary>
        /// <param name="optionLabel">The label of the option for which to get the value.</param>
        IUserInputToCommandOption? GetArgumentOrNull(string optionLabel);

        /// <summary>
        /// Gets the value of an option provided by the user or throws an exception
        /// if the option was not found.
        /// </summary>
        /// <param name="optionLabel">The label of the option for which to get the value.</param>
        IUserInputToCommandOption GetArgument(string optionLabel);
    }
}
