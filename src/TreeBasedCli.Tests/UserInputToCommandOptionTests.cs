using System.Collections.Generic;
using Shouldly;
using TreeBasedCli.Exceptions;
using Xunit;

namespace TreeBasedCli.Tests
{
    public class UserInputToCommandOptionTests
    {
        public enum TestEnum
        {
            Value1,
            Value2,
            Value3
        }

        [Theory]
        [InlineData("Value1", TestEnum.Value1)]
        [InlineData("Value2", TestEnum.Value2)]
        [InlineData("Value3", TestEnum.Value3)]
        public void ExpectedAsEnumValue_ShouldReturnCorrectEnumValue(string input, TestEnum expected)
        {
            // given
            var command = new Command("test-command", new[] { "Test Description" });
            var userInput = new[] { input };
            var option = new UserInputToCommandOption(command, "--test-option", userInput);

            // when
            TestEnum result = option.ExpectedAsEnumValue<TestEnum>();

            // then
            result.ShouldBe(expected);
        }

        [Fact]
        public void ExpectedAsEnumValues_ShouldReturnCorrectEnumValues()
        {
            // given
            var command = new Command("test-command", new[] { "Test Description" });
            var userInput = new[] { "Value1", "Value2", "Value3" };
            var option = new UserInputToCommandOption(command, "--test-option", userInput);

            // whtn
            IReadOnlySet<TestEnum> result = option.ExpectedAsEnumValues<TestEnum>();

            // then
            result.ShouldBe(new HashSet<TestEnum> { TestEnum.Value1, TestEnum.Value2, TestEnum.Value3 }, ignoreOrder: true);
        }

        [Fact]
        public void ExpectedAsEnumValues_WhenInputIsEmpty_ShouldReturnEmptySet()
        {
            // given
            var command = new Command("test-command", new[] { "Test Description" });
            var userInput = new string[] { };
            var option = new UserInputToCommandOption(command, "--test-option", userInput);

            // when
            IReadOnlySet<TestEnum> result = option.ExpectedAsEnumValues<TestEnum>();

            // then
            result.ShouldBeEmpty();
        }

        [Fact]
        public void ExpectedAsEnumValues_WhenInvalidEnumValueProvided_ShouldThrowMessageOnlyException()
        {
            // given
            var command = new Command("test-command", new[] { "Test Description" });
            var userInput = new[] { "InvalidValue" };
            var option = new UserInputToCommandOption(command, "--test-option", userInput);

            // when
            Should.Throw<WrongCommandUsageException>(() => option.ExpectedAsEnumValues<TestEnum>());
        }
    }
}
