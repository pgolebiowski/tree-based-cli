using TreeBasedCli.HelpGuideGeneration;

namespace TreeBasedCli
{
    public class ArgumentHandlerSettings
    {
        public ArgumentHandlerSettings(
            string name, string version, CommandTree commandTree)
        {
            this.Name = name;
            this.Version = version;
            this.CommandTree = commandTree;
            this.HelpProvider = new DefaultHelpProvider(this);
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public CommandTree CommandTree { get; set; }

        public IHelpProvider HelpProvider { get; set; }
    }
}
