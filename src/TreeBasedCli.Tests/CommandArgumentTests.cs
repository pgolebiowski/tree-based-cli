using Xunit;
using System.Threading.Tasks;

namespace TreeBasedCli.Tests
{
    public class CommandArgumentTests
    {
        private enum SampleEnum { None, One, Two, Three };

        [Fact]
        public async Task should_parse_correct_enum_value()
        {
            // given
            SampleEnum result = SampleEnum.None;
            string argumentLabel = "--enum-value";
            var command = new LeafCommand
            {
                Label = "verb",
                Options = new[]
                {
                    new CommandOption(argumentLabel)
                },
                TaskToRun = args =>
                {
                    result = args.GetArgument(argumentLabel).ExpectedAsEnumValue<SampleEnum>();
                    return Task.CompletedTask;
                }
            };

            var commandTree = new CommandTree
            {
                Root = command
            };

            var handler = new ArgumentHandler(
                new ArgumentHandlerSettings(name: "name", version: "version", commandTree));

            // when
            await handler.HandleAsync(new[] { "--enum-value", "Two" });

            // then
            Assert.Equal(SampleEnum.Two, result);
        }
    }
}
