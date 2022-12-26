using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TreeBasedCli
{
    public interface ICommandArguments
    {
        IReadOnlyCollection<string> Arguments { get; }
        LeafCommand Command { get; }
        bool ContainsArgument(string optionLabel);
        bool TryGetArgument(string optionLabel, [NotNullWhen(true)] out IUserInputToCommandOption? result);
        IUserInputToCommandOption? GetArgumentOrNull(string optionLabel);
        IUserInputToCommandOption GetArgument(string optionLabel);
    }
}
