using System.Collections.Generic;

namespace TreeBasedCli
{
    /// <summary>
    /// This is a builder class for creating <see cref="BranchCommand" /> objects.
    /// It has methods for setting the description and adding child commands.
    /// It also has a Build method that returns a new <see cref="BranchCommand" /> object
    /// with the provided label, description, and list of child commands.
    /// </summary>
    public class BranchCommandBuilder
    {
        private readonly string label;
        private string[] description;
        private List<Command> childCommands;

        /// <inheritdoc cref="BranchCommandBuilder" />
        public BranchCommandBuilder(string label)
        {
            this.label = label;
            this.description = new string[] { };
            this.childCommands = new List<Command>();
        }

        /// <summary>
        /// Sets the description of the branch command.
        /// </summary>
        public BranchCommandBuilder WithDesription(string[] description)
        {
            this.description = description;
            return this;
        }

        /// <summary>
        /// Adds a child command to the branch command.
        /// </summary>
        public BranchCommandBuilder WithChildCommand(Command command)
        {
            this.childCommands.Add(command);
            return this;
        }

        /// <summary>
        /// Builds a new instance of <see cref="BranchCommand"/> using
        /// the current builder configuration.
        /// </summary>
        public BranchCommand Build()
        {
            return new BranchCommand(
                label: this.label,
                description: this.description,
                childCommands: this.childCommands
            );
        }
    }
}
