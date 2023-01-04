using System;

namespace TreeBasedCli.Exceptions
{
    /// <summary>
    /// <para>
    /// Represents an exception that is used to communicate a message to the user
    /// directly. Unlike <see cref="WrongCommandUsageException"/>, this exception
    /// does not trigger the display of the help guide for the command.
    /// </para>
    /// 
    /// <para>
    /// Use this exception when you assume that the user knows how to invoke the command
    /// and which options to choose and does not need to be reminded of that.
    /// </para>
    /// </summary>
    public class MessageOnlyException : Exception
    {
        /// <inheritdoc cref="MessageOnlyException" />
        /// <param name="message">The message to be displayed to the user.</param>
        public MessageOnlyException(string message) : base(message)
        {
        }
    }
}
