using System;
using System.Collections.Generic;

namespace TreeBasedCli.Internal
{
    internal class HeaderSection : SectionBuilder
    {
        private const int HeaderIndentSize = 4;
        private readonly string header;

        public HeaderSection(string header)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                throw new ArgumentException("Section header cannot be empty.");
            }

            this.header = header;
        }

        public override IEnumerable<string> Build(int lineLengthLimit)
        {
            return TextFormatter.Indent(this.header, HeaderIndentSize, lineLengthLimit);
        }
    }
}