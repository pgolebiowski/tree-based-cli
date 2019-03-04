using System;

namespace TreeBasedCli
{
    /// <summary>
    /// Represents an exception that can be constructed with a message only.
    /// This message is meant to be presented to the user directly, without
    /// other information (e.g. the stack trace).
    /// </summary>
    public class MessageOnlyException : Exception
    {
        public MessageOnlyException(string message) : base(message)
        {
        }
    }
}
