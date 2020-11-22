namespace TreeBasedCli
{
    public interface ICommandArgument
    {
        int Count { get; }
        string ExpectedAsSingleValue();
        string ExpectedAsSinglePathToExistingFile();
        string ExpectedAsSinglePathToExistingDirectory();
        int ExpectedAsSingleInteger();
    }
}
