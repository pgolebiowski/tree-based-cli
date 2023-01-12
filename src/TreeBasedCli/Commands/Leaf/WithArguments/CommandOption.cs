using TreeBasedCli.Exceptions;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents a command option, which is a user-specified flag or parameter
    /// that modifies the behavior of a command.
    /// </summary>
    public class CommandOption
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CommandOption"/> class, which represents
        /// a user-specified flag or parameter that modifies the behavior of a command.
        /// </summary>
        /// <param name="label">
        /// The string representing the label for the option.
        /// It cannot be null, empty, or consist exclusively of whitespace characters.
        /// </param>
        /// <param name="description">
        /// A description of the option, its usage and its effects. Each item of the array
        /// will be generated in documentation as a separate paragraph.
        /// </param>
        public CommandOption(string label, string[] description)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw ThrowHelper.Exception(
                    "Option label cannot be null, empty, or consist ",
                    "exclusively of whitespace characters.");
            }

            this.Label = label;

            this.Description = description;
        }

        /// <summary>
        /// Gets the string representing the label for the option.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Gets the description of the option, its usage and its effects.
        /// </summary>
        public string[] Description { get; }
    }
}
