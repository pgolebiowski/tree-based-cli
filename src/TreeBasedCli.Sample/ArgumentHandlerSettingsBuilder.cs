namespace TreeBasedCli.Sample
{
    internal static class ArgumentHandlerSettingsBuilder
    {
        public static ArgumentHandlerSettings Build()
            => new ArgumentHandlerSettings
            {
                Name = "Animal Factory",
                CommandTree = new CommandTree
                {
                    Root = BuildCommandTree()
                }
            };

        private static Command BuildCommandTree()
            => new BranchCommand
            {
                Label = "af",
                Description = new[]
                {
                    "This program produces animals."
                },
                ChildCommands = new Command[]
                {
                    new CreateDogCommand(),
                    new CreateCatCommand(),
                }
            };
    }
}
