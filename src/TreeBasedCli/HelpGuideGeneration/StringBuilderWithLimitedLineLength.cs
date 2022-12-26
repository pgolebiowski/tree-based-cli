using System;
using System.Collections.Generic;
using System.Text;
using TreeBasedCli.Internal;

namespace TreeBasedCli.HelpGuideGeneration
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
            IEnumerable<string> sectionLines = sectionBuilder.Build(this.lineLengthLimit);
            this.builder.AppendLines(sectionLines);
        }

        public override string ToString()
        {
            return this.builder.ToString();
        }
    }
}