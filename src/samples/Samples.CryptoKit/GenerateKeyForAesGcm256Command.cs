using TreeBasedCli;

namespace Samples.CryptoKit
{
    public class GenerateKeyForAesGcm256Command :
        LeafCommand<
            GenerateKeyForAesGcm256Command.Arguments,
            GenerateKeyForAesGcm256Command.Parser,
            GenerateKeyForAesGcm256Command.Handler>
    {
        public GenerateKeyForAesGcm256Command() : base(
            label: "generate-key",
            description: new[]
            {
                "Generate a 256-bit cryptographic key and print it in base64."
            },
            options: new CommandOption[] { })
        { }

        public record Arguments() : IParsedCommandArguments;

        public class Parser : ICommandArgumentParser<Arguments>
        {
            public IParseResult<Arguments> Parse(CommandArguments arguments)
            {
                return new SuccessfulParseResult<Arguments>(
                    new Arguments()
                );
            }
        }

        public class Handler : ILeafCommandHandler<Arguments>
        {
            public Task HandleAsync(Arguments arguments, LeafCommand _)
            {
                Console.WriteLine($"here is your new key: 'blah blah blah'");
                return Task.CompletedTask;
            }
        }
    }
}
