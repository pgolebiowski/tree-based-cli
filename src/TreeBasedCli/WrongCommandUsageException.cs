using System;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents an error that occurs when the user provides an invalid
    /// command and/or options. The guide will be automatically presented
    /// to the user, along with the error message.
    /// </summary>
    public class WrongCommandUsageException : Exception
    {
        public WrongCommandUsageException(Command command, string message) : base(message)
        {
            this.Command = command;
        }

        public Command Command { get; }
    }
}
