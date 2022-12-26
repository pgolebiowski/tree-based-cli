using TreeBasedCli;

namespace Samples.AnimalKingdom
{
    internal static class ArgumentHandlerSettingsBuilder
    {
        public static ArgumentHandlerSettings Build()
            => new ArgumentHandlerSettings
            (
                name: "Animal Kingdom",
                version: "1.0",
                commandTree: new CommandTree(
                    root: BuildCommandTree())
            );

        private static Command BuildCommandTree()
            => new BranchCommandBuilder(label: "af")
                .WithDesription(new[]
                {
                    "This program prints animals."
                })
                .WithChildCommand(new CreateDogCommand())
                .WithChildCommand(new CreateCatCommand())
                .WithChildCommand(new CreateRandomAnimalCommand())
                .Build();
    }
}
