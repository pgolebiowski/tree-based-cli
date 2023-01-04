using System;

namespace TreeBasedCli.Exceptions
{
    /// <summary>
    /// <para>
    /// Represents an error that occurs when the user provides an invalid command
    /// and/or options. This exception includes the command that the error concerns,
    /// as well as a message to be displayed to the user.
    /// </para>
    /// 
    /// <para>
    /// When this exception is thrown, the help guide for the command will also
    /// be displayed to the user. This differs from <see cref="MessageOnlyException" />,
    /// which does not trigger help guide generation.
    /// </para>
    /// </summary>
    public class WrongCommandUsageException : Exception
    {
        /// <inheritdoc cref="WrongCommandUsageException" />
        /// <param name="command">The command that the error concerns.</param>
        /// <param name="message">The message to be displayed to the user.</param>
        public WrongCommandUsageException(Command command, string message) : base(message)
        {
            this.Command = command;
        }

        /// <summary>
        /// Gets the command that the error concerns.
        /// </summary>
        public Command Command { get; }
    }
}
