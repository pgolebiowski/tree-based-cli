using System.Collections.Generic;

namespace TreeBasedCli
{
    public class BranchCommandBuilder
    {
        private readonly string label;
        private string[] description;
        private List<Command> childCommands;

        public BranchCommandBuilder(string label)
        {
            this.label = label;
            this.description = new string[] { };
            this.childCommands = new List<Command>();
        }

        public BranchCommandBuilder WithDesription(string[] description)
        {
            this.description = description;
            return this;
        }

        public BranchCommandBuilder WithChildCommand(Command command)
        {
            this.childCommands.Add(command);
            return this;
        }

        public BranchCommand Build()
        {
            return new BranchCommand(this.childCommands)
            {
                Label = this.label,
                Description = this.description
            };
        }
    }
}
