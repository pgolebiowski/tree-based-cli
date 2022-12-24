namespace TreeBasedCli.Sample
{
    internal static class ArgumentHandlerSettingsBuilder
    {
        public static ArgumentHandlerSettings Build()
            => new ArgumentHandlerSettings
            (
                name: "Animal Factory",
                version: "1.0",
                commandTree: new CommandTree
                {
                    Root = BuildCommandTree()
                }
            );

        private static Command BuildCommandTree()
            => new BranchCommandBuilder(label: "af")
                .WithDesription(new[]
                {
                    "This program produces animals."
                })
                .WithChildCommand(new CreateDogCommand())
                .WithChildCommand(new CreateCatCommand())
                .Build();
    }
}
