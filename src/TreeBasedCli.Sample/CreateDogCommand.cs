using System.Threading.Tasks;
using TreeBasedCli.DependencyInjection;

namespace TreeBasedCli.Sample
{
    public class CreateDogCommand :
        LeafCommand<
            CreateDogCommand.Arguments,
            CreateDogCommand.Parser,
            CreateDogCommand.Handler>
    {
        private const string NameLabel = "--name";

        public CreateDogCommand() : base(DependencyInjectionService.Instance)
        {
            this.Label = "create-dog";
            this.Description = new[]
            {
                "Prints out a dog."
            };
            this.Options = new[]
            {
                new CommandOption(NameLabel)
                {
                    Description = new[]
                    {
                        "Required. The name of the dog to print."
                    }
                },
            };
        }

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

        public class Handler : ICommandHandler<Arguments>
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
