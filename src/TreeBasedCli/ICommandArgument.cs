using System.Collections.Generic;

namespace TreeBasedCli
{
    public interface ICommandArgument
    {
        int Count { get; }
        IReadOnlyCollection<string> Values { get; }

        string ExpectedAsSingleValue();
        string ExpectedAsSinglePathToExistingFile();
        string ExpectedAsSinglePathToExistingDirectory();
        int ExpectedAsSingleInteger();
    }
}
