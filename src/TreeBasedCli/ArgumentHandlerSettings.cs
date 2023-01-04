using TreeBasedCli.HelpGuideGeneration;

namespace TreeBasedCli
{
    /// <summary>
    /// Settings class for creating an instance of <see cref="ArgumentHandler"/>,
    /// which is responsible for parsing and executing commands in a command-line interface.
    /// </summary>
    public class ArgumentHandlerSettings
    {
        /// <inheritdoc cref="ArgumentHandlerSettings" />
        /// <param name="name">The name of the command-line interface application.</param>
        /// <param name="version">The version of the command-line interface application.</param>
        /// <param name="commandTree">
        /// The command tree for the command-line interface application,
        /// which represents the hierarchical structure of commands and subcommands.
        /// </param>
        /// <param name="helpProvider">
        /// An optional help provider for the command-line interface. If not provided,
        /// a default help provider will be used to generate documentation and error messages.
        /// </param>
        public ArgumentHandlerSettings(
            string name,
            string version,
            CommandTree commandTree,
            IHelpProvider? helpProvider = null)
        {
            this.Name = name;
            this.Version = version;
            this.CommandTree = commandTree;
            this.HelpProvider = helpProvider ?? new DefaultHelpProvider(this);
        }

        /// <summary>
        /// Gets the name of the command-line interface application,
        /// which is displayed in the generated documentation.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the version of the command-line interface application,
        /// which is displayed in the generated documentation.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the command tree for the command-line interface application,
        /// which represents the hierarchical structure of commands and subcommands.
        /// </summary>
        public CommandTree CommandTree { get; }

        /// <summary>
        /// The help provider for the command-line interface, which is responsible
        /// for generating documentation and error messages.
        /// </summary>
        public IHelpProvider HelpProvider { get; }
    }
}
