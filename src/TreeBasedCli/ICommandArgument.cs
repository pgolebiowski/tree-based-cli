using System;
using System.Collections.Generic;

namespace TreeBasedCli
{
    public interface ICommandArgument
    {
        Command Command { get; }

        int Count { get; }
        IReadOnlyCollection<string> Values { get; }

        string ExpectedAsSingleValue();
        string ExpectedAsSinglePathToExistingFile();
        string ExpectedAsSinglePathToExistingDirectory();
        int ExpectedAsSingleInteger();
        TEnum ExpectedAsEnumValue<TEnum>() where TEnum : struct, Enum;
    }
}
