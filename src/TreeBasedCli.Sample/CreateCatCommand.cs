using System.Threading.Tasks;
using TreeBasedCli.DependencyInjection;

namespace TreeBasedCli.Sample
{
    public class CreateCatCommand :
        LeafCommand<
            CreateCatCommand.Arguments,
            CreateCatCommand.Parser,
            CreateCatCommand.Handler>
    {
        private const string NameLabel = "--name";

        public CreateCatCommand() : base(DependencyInjectionService.Instance)
        {
            this.Label = "create-cat";
            this.Description = new[]
            {
                "Prints out a cat."
            };
            this.Options = new[]
            {
                new CommandOption(
                    label: NameLabel,
                    description: new[]
                    {
                        "Required. The name of the cat to print."
                    }
                ),
            };
        }

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

        public class Handler : ICommandHandler<Arguments>
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
