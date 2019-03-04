using System;
using System.Collections.Generic;
using System.Linq;
using TreeBasedCli.Internal;

namespace TreeBasedCli
{
    public class LeafCommand : Command
    {
        public CommandOption[] Options { get; set; }
        public Action<CommandArguments> Action { get; set; }

        public HashSet<string> OptionLabels
        {
            get
            {
                var result = new HashSet<string>();

                if (this.Options == null)
                    return result;
                
                foreach (var option in this.Options)
                {
                    result.Add(option.Label);
                }

                return result;
            }
        }
    }
}
