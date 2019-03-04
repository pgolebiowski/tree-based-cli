using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeBasedCli.Internal
{
    internal class TwoColumnParagraphBodySection : SectionBuilder
    {
        private readonly int firstColumnWidth;
        private readonly IReadOnlyCollection<string> firstColumnOfParagraphs;
        private readonly IReadOnlyCollection<string> secondColumnOfParagraphs;
        private int firstColumnIndentSize = 8;
        private int columnSeparatorSize = 4;
        private int emptyLinesBetweenParagraphs = 2;

        public TwoColumnParagraphBodySection(
            int firstColumnWidth,
            IReadOnlyCollection<string> firstColumnOfParagraphs,
            IReadOnlyCollection<string> secondColumnOfParagraphs)
        {
            if (firstColumnWidth < 1)
            {
                throw new ArgumentOutOfRangeException(
                    "First column width has to be at least 1.");
            }

            this.firstColumnWidth = firstColumnWidth;
            this.firstColumnOfParagraphs = firstColumnOfParagraphs ?? new[] { "" };
            this.secondColumnOfParagraphs = secondColumnOfParagraphs ?? new[] { "" };
        }

        public int FirstColumnIndentSize
        {
            get => this.firstColumnIndentSize;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "Indent size cannot be a negative number.");
                }

                this.firstColumnIndentSize = value;
            }
        }

        public int ColumnSeparatorSize
        {
            get => this.columnSeparatorSize;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "Column separator size cannot be a negative number.");
                }

                this.columnSeparatorSize = value;
            }
        }

        public int EmptyLinesBetweenParagraphs
        {
            get => this.emptyLinesBetweenParagraphs;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "The number of empty lines between paragraphs cannot be a negative number.");
                }

                this.emptyLinesBetweenParagraphs = value;
            }
        }

        public override IEnumerable<string> Build(int lineLengthLimit)
        {
            var secondColumnIndentSize =
                this.firstColumnIndentSize +
                this.firstColumnWidth +
                this.columnSeparatorSize;

            var rawFirstColumnText = this.firstColumnOfParagraphs.InjectBetweenAdjacentElements("\n", this.emptyLinesBetweenParagraphs).Join();
            var rawSecondColumnText = this.secondColumnOfParagraphs.InjectBetweenAdjacentElements("\n", this.emptyLinesBetweenParagraphs).Join();

            var firstColumnLines = TextFormatter.Indent(rawFirstColumnText, this.firstColumnIndentSize, lineLengthLimit).ToArray();
            var secondColumnLines = TextFormatter.Indent(rawSecondColumnText, secondColumnIndentSize, lineLengthLimit).ToArray();
            var mergedLines = TextFormatter.MergeColumnsLineByLine(secondColumnLines, firstColumnLines).ToArray();

            return mergedLines;
        }
    }
}