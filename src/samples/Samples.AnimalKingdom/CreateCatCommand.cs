using System.Threading.Tasks;
using TreeBasedCli;

namespace Samples.AnimalKingdom
{
    public class CreateCatCommand :
        LeafCommand<
            CreateCatCommand.Arguments,
            CreateCatCommand.Parser,
            CreateCatCommand.Handler>
    {
        private const string NameLabel = "--name";

        public CreateCatCommand() : base(
            label: "create-cat",
            description: new[]
            {
                "Prints out a cat."
            },
            options: new[]
            {
                new CommandOption(
                    label: NameLabel,
                    description: new[]
                    {
                        "Required. The name of the cat to print."
                    }
                ),
            })
        { }

        public record Arguments(string CatName) : IParsedCommandArguments;

        public class Parser : ICommandArgumentParser<Arguments>
        {
            public IParseResult<Arguments> Parse(CommandArguments arguments)
            {
                string name = arguments.GetArgument(NameLabel).ExpectedAsSingleValue();

                var result = new Arguments(
                    CatName: name
                );

                return new SuccessfulParseResult<Arguments>(result);
            }
        }

        public class Handler : ILeafCommandHandler<Arguments>
        {
            private readonly IUserInterface userInterface;

            public Handler(IUserInterface userInterface)
            {
                this.userInterface = userInterface;
            }

            public Task HandleAsync(Arguments arguments, LeafCommand _)
            {
                this.userInterface.WriteLine($"I am a cat 😸 with the name {arguments.CatName}!");
                return Task.CompletedTask;
            }
        }
    }
}
