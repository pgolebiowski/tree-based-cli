using System.Collections.Generic;

namespace TreeBasedCli
{
    public interface ICommandArguments
    {
        IReadOnlyCollection<string> Arguments { get; }
        LeafCommand Command { get; }
        bool TryGetArgument(string optionLabel, out ICommandArgument result);
        ICommandArgument GetArgumentOrNull(string optionLabel);
        ICommandArgument GetArgument(string optionLabel);
    }
}
