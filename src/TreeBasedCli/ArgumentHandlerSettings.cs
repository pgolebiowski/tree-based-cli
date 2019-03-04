using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    public class ArgumentHandlerSettings
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public CommandTree CommandTree { get; set; }
        
        private IHelpProvider helpProvider;
        public IHelpProvider HelpProvider
        {
            get => this.helpProvider ?? new DefaultHelpProvider(this);
            set => this.helpProvider = value;
        }
    }
}
