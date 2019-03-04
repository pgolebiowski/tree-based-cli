using System;
using System.Collections.Generic;

namespace TreeBasedCli.Internal
{
    internal class ParagraphBodySection : SectionBuilder
    {
        private const int BodyIndentSize = 8;
        private readonly string body;

        public ParagraphBodySection(string paragraph, int emptyLinesBetweenParagraphs = 1)
            : this(new[] { paragraph }, emptyLinesBetweenParagraphs)
        {
        }

        public ParagraphBodySection(
            IReadOnlyCollection<string> paragraphs,
            int emptyLinesBetweenParagraphs = 1)
        {
            if (paragraphs.IsEmpty())
            {
                throw new ArgumentException(
                    "Cannot construct section body from an empty list of paragraphs.");
            }

            if (emptyLinesBetweenParagraphs < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "The number of lines between paragraphs cannot be a negative number.");
            }

            var paragraphSeparator = new string('\n', emptyLinesBetweenParagraphs + 1);
            this.body = paragraphs.Join(paragraphSeparator);
        }

        public override IEnumerable<string> Build(int lineLengthLimit)
        {
            return TextFormatter.Indent(this.body, BodyIndentSize, lineLengthLimit);
        }
    }
}