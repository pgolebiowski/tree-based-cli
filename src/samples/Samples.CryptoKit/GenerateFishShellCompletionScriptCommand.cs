using TreeBasedCli;

namespace Samples.CryptoKit
{
    internal class GenerateFishShellCompletionScriptCommand :
        LeafCommand<GenerateFishShellCompletionScriptCommand.Handler>
    {
        public GenerateFishShellCompletionScriptCommand() : base(
            label: "fish-shell-completion-script",
            description: new[]
            {
                "Prints out a Fish shell script that handles autocompletion."
            })
        { }

        public class Handler : ILeafCommandHandler
        {
            public Task HandleAsync(LeafCommand executedCommand)
            {
                var builder = new FishShellCompletionScriptBuilder();

                builder.AddCommandTree(executedCommand);

                string script = builder.GenerateScript();

                Console.WriteLine(script);
                return Task.CompletedTask;
            }
        }
    }
}
