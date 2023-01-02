using TreeBasedCli;

namespace Samples.CryptoKit
{
    public class DecryptWithAesGcm256Command :
        LeafCommand<
            DecryptWithAesGcm256Command.Arguments,
            DecryptWithAesGcm256Command.Parser,
            DecryptWithAesGcm256Command.Handler>
    {
        private const string InputLabel = "--input";
        private const string OutputLabel = "--output";

        public DecryptWithAesGcm256Command() : base(
            label: "decrypt",
            description: new[]
            {
                "Decrypts the specified file using a cryptographic key and additional authenticated data."
            },
            options: new[]
            {
                new CommandOption(
                    label: InputLabel,
                    description: new[]
                    {
                        "The path to the input file that is to be decrypted."
                    }
                ),
                new CommandOption(
                    label: OutputLabel,
                    description: new[]
                    {
                        "The path to the output file where the decrypted data is to be written."
                    }
                ),
            })
        { }

        public record Arguments(string InputPath, string OutputPath) : IParsedCommandArguments;

        public class Parser : ICommandArgumentParser<Arguments>
        {
            public IParseResult<Arguments> Parse(CommandArguments arguments)
            {
                string inputPath = arguments.GetArgument(InputLabel).ExpectedAsSinglePathToExistingFile();
                string outputPath = arguments.GetArgument(OutputLabel).ExpectedAsSingleValue();

                var result = new Arguments(
                    InputPath: inputPath,
                    OutputPath: outputPath
                );

                return new SuccessfulParseResult<Arguments>(result);
            }
        }

        public class Handler : ICommandHandler<Arguments>
        {
            public Task HandleAsync(Arguments arguments, LeafCommand _)
            {
                Console.WriteLine($"decrypting file {arguments.InputPath}");
                Console.WriteLine($"writing output to {arguments.OutputPath}");
                return Task.CompletedTask;
            }
        }
    }
}
