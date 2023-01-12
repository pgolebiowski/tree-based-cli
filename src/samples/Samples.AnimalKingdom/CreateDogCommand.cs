using System.Threading.Tasks;
using TreeBasedCli;

namespace Samples.AnimalKingdom
{
    public class CreateDogCommand :
        LeafCommand<
            CreateDogCommand.Arguments,
            CreateDogCommand.Parser,
            CreateDogCommand.Handler>
    {
        private const string NameLabel = "--name";

        public CreateDogCommand() : base(
            label: "create-dog",
            description: new[]
            {
                "Prints out a dog."
            },
            options: new[]
            {
                new CommandOption(
                    label: NameLabel,
                    description: new[]
                    {
                        "Required. The name of the dog to print."
                    }
                ),
            })
        { }

        public record Arguments(string DogName) : IParsedCommandArguments;

        public class Parser : ICommandArgumentParser<Arguments>
        {
            public IParseResult<Arguments> Parse(CommandArguments arguments)
            {
                string name = arguments.GetArgument(NameLabel).ExpectedAsSingleValue();

                var result = new Arguments(
                    DogName: name
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
                this.userInterface.WriteLine($"I am a doggo 🐶 with the name {arguments.DogName}!");
                return Task.CompletedTask;
            }
        }
    }
}
