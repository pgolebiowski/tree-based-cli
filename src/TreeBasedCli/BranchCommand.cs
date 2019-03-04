using System.Linq;

namespace TreeBasedCli
{
    public class BranchCommand : Command
    {
        public Command[] ChildCommands { get; set; }

        public bool TryGetChildCommand(string label, out Command command)
        {
            command = this.ChildCommands?.FirstOrDefault(x => x.Label == label);
            return command != null;
        }
    }
}
