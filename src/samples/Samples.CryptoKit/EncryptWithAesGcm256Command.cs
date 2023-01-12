using TreeBasedCli;

namespace Samples.CryptoKit
{
    public class EncryptWithAesGcm256Command :
        LeafCommand<
            EncryptWithAesGcm256Command.Arguments,
            EncryptWithAesGcm256Command.Parser,
            EncryptWithAesGcm256Command.Handler>
    {
        private const string InputLabel = "--input";
        private const string OutputLabel = "--output";

        public EncryptWithAesGcm256Command() : base(
            label: "encrypt",
            description: new[]
            {
                "Encrypts the specified file using a cryptographic key and additional authenticated data."
            },
            options: new[]
            {
                new CommandOption(
                    label: InputLabel,
                    description: new[]
                    {
                        "The path to the input file that is to be encrypted."
                    }
                ),
                new CommandOption(
                    label: OutputLabel,
                    description: new[]
                    {
                        "The path to the output file where the encrypted data is to be written."
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

        public class Handler : ILeafCommandHandler<Arguments>
        {
            public Task HandleAsync(Arguments arguments, LeafCommand _)
            {
                Console.WriteLine($"encrypting file {arguments.InputPath}");
                Console.WriteLine($"writing output to {arguments.OutputPath}");
                return Task.CompletedTask;
            }
        }
    }
}
