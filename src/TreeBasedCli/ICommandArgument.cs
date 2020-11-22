using System.Collections.Generic;

namespace TreeBasedCli
{
    public interface ICommandArgument : IReadOnlyCollection<string>
    {
        string ExpectedAsSingleValue();
        string ExpectedAsSinglePathToExistingFile();
        string ExpectedAsSinglePathToExistingDirectory();
        int ExpectedAsSingleInteger();
    }
}
