using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TreeBasedCli
{
    public class LeafCommand : Command
    {
        public LeafCommand(
            string label,
            string[] description,
            IReadOnlyList<CommandOption> options)
                : base(label, description)
        {
            this.Options = options;
        }

        public IReadOnlyList<CommandOption> Options { get; }
        public Func<CommandArguments, Task> TaskToRun { get; set; }

        public IReadOnlySet<string> OptionLabels
        {
            get
            {
                var result = new HashSet<string>();

                if (this.Options == null)
                    return result;

                foreach (CommandOption option in this.Options)
                {
                    result.Add(option.Label);
                }

                return result;
            }
        }
    }
}
