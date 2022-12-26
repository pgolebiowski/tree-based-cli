namespace TreeBasedCli
{
    public class FailedParseResult<T> : IParseResult<T>
    {
        public FailedParseResult(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
    }
}