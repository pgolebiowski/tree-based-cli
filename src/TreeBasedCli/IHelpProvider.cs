namespace TreeBasedCli
{
    public interface IHelpProvider
    {
        string ProvideHelp(BranchCommand command, WrongCommandUsageException exception);
        string ProvideHelp(LeafCommand command, WrongCommandUsageException exception);
    }
}
