using System;
using System.Collections.Generic;

namespace TreeBasedCli
{
    public interface IUserInputToCommandOption
    {
        Command Command { get; }

        IReadOnlyCollection<string> UserInput { get; }
        int UserInputWordCount { get; }

        string ExpectedAsSingleValue();
        string ExpectedAsSinglePathToExistingFile();
        string ExpectedAsSinglePathToExistingDirectory();
        int ExpectedAsSingleInteger();
        TEnum ExpectedAsEnumValue<TEnum>() where TEnum : struct, Enum;
    }
}
