using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    public class CommandOption
    {
        public CommandOption(string label, string[] description)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw ThrowHelper.Exception(
                    "Option label cannot be null, empty, or consist ",
                    "exclusively of whitespace characters.");
            }

            this.Label = label;

            this.Description = description;
        }

        public string Label { get; }
        public string[] Description { get; }
    }
}
