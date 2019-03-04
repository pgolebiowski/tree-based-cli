using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    public class CommandOption
    {
        public string Label { get; }
        public string[] Description { get; set; }

        public CommandOption(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw ThrowHelper.Exception(
                    "Option label cannot be null, empty, or consist ",
                    "exclusively of whitespace characters.");
            }

            this.Label = label;
        }
    }
}
