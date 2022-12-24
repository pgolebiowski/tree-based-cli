using System.Collections.Generic;
using System.Text;

namespace TreeBasedCli.Internal
{
    internal static class StringBuilderExtensions
    {
        public static void AppendLines(this StringBuilder builder, IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                builder.AppendLine(line);
            }
        }
    }
}