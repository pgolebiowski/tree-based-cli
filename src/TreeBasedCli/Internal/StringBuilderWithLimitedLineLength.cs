using System;
using System.Collections.Generic;
using System.Text;

namespace TreeBasedCli.Internal
{
    internal class StringBuilderWithLimitedLineLength
    {
        private readonly StringBuilder builder;
        private readonly int lineLengthLimit;

        public StringBuilderWithLimitedLineLength(int lineLengthLimit)
        {
            if (lineLengthLimit <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    "Line length limit has to be greater than 0.");
            }

            this.builder = new StringBuilder();
            this.lineLengthLimit = lineLengthLimit;
        }

        public void AppendSection(SectionBuilder sectionBuilder)
        {
            var sectionLines = sectionBuilder.Build(this.lineLengthLimit);
            this.builder.AppendLines(sectionLines);
        }

        public void AppendLine(string line = null)
        {
            this.builder.AppendLine(line);
        }

        public void AppendLines(IEnumerable<string> lines)
        {
            this.builder.AppendLines(lines);
        }

        public override string ToString()
        {
            return this.builder.ToString();
        }
    }
}