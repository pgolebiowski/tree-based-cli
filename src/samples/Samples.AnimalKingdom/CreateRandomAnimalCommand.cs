using System;
using System.Threading.Tasks;
using TreeBasedCli;

namespace Samples.AnimalKingdom
{
    public class CreateRandomAnimalCommand :
        LeafCommand<CreateRandomAnimalCommand.Handler>
    {
        public CreateRandomAnimalCommand() : base(
            label: "create-random-animal",
            description: new[]
            {
                "Prints out a random animal."
            },
            DependencyInjectionService.Instance)
        { }

        public class Handler : ILeafCommandHandler
        {
            private readonly IUserInterface userInterface;

            public Handler(IUserInterface userInterface)
            {
                this.userInterface = userInterface;
            }

            public Task HandleAsync(LeafCommand _)
            {
                var animals = new[]
                {
                    "🦔",
                    "🐝",
                    "🐘"
                };

                string animal = animals[Random.Shared.Next(0, 3)];
                this.userInterface.WriteLine(animal);

                return Task.CompletedTask;
            }
        }
    }
}
