using System;
using System.Collections.Generic;

namespace TreeBasedCli.HelpGuideGeneration
{
    internal class EmptySection : SectionBuilder
    {
        private readonly int linesCount;

        public EmptySection(int linesCount)
        {
            if (linesCount < 1)
            {
                throw new ArgumentOutOfRangeException(
                    $"Expected line count to be greater than 1, but was {linesCount}.");
            }

            this.linesCount = linesCount;
        }

        public override IEnumerable<string> Build(int lineLengthLimit)
        {
            for (int i = 0; i < linesCount; ++i)
            {
                yield return "";
            }
        }
    }
}