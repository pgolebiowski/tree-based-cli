using Xunit;
using System.Threading.Tasks;
using System;

namespace TreeBasedCli.Tests
{
    public class CommandArgumentTests
    {
        private enum SampleEnum { None, One, Two, Three };

        private class FutureValueHolder<T>
        {
            public T Value { get; set; }
        }

        private class SampleCommand : LeafCommand
        {
            private const string argumentLabel = "--enum-value";
            private readonly FutureValueHolder<SampleEnum> result;

            public SampleCommand(FutureValueHolder<SampleEnum> result) : base(
                label: "verb",
                description: new string[] { },
                options: new[]
                {
                    new CommandOption(
                        label: argumentLabel,
                        description: new string[] { })
                })
            {
                this.result = result;
            }

            public override Task TaskToRun(CommandArguments commandArguments)
            {
                this.result.Value = commandArguments.GetArgument(argumentLabel).ExpectedAsEnumValue<SampleEnum>();
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task should_parse_correct_enum_value()
        {
            // given
            var resultHolder = new FutureValueHolder<SampleEnum>();
            var command = new SampleCommand(resultHolder);

            var commandTree = new CommandTree
            {
                Root = command
            };

            var handler = new ArgumentHandler(
                new ArgumentHandlerSettings(name: "name", version: "version", commandTree));

            // when
            await handler.HandleAsync(new[] { "--enum-value", "Two" });

            // then
            Console.WriteLine($"received value: {resultHolder.Value}");
            Assert.Equal(SampleEnum.Two, resultHolder.Value);
        }
    }
}
