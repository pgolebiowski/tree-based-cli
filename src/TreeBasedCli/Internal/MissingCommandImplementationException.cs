namespace TreeBasedCli.Internal
{
    internal class MissingCommandImplementationException : WrongCommandUsageException
    {
        public MissingCommandImplementationException(Command command, string message)
            : base(command, message)
        {
        }
    }
}