using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeBasedCli
{
    public interface IHelpProvider
    {
        string ProvideHelp(BranchCommand command, WrongCommandUsageException exception = null);
        string ProvideHelp(LeafCommand command, WrongCommandUsageException exception = null);
    }
}
