using TreeBasedCli.Exceptions;

namespace TreeBasedCli.HelpGuideGeneration
{
    /// <summary>
    /// An interface for providing help documentation for a command-line interface.
    /// </summary>
    public interface IHelpProvider
    {
        /// <summary>
        /// Generates help documentation for a branch command in the command tree.
        /// </summary>
        string ProvideHelp(BranchCommand command, WrongCommandUsageException? exception);

        /// <summary>
        /// Generates help documentation for a leaf command in the command tree.
        /// </summary>
        string ProvideHelp(LeafCommand command, WrongCommandUsageException? exception);
    }
}
