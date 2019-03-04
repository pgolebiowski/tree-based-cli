using System.Collections.Generic;

namespace TreeBasedCli.Internal
{
    internal class DescriptionSection : CompositeSectionBuilder
    {
        private readonly IReadOnlyCollection<string> description;

        public DescriptionSection(IReadOnlyCollection<string> description)
        {
            this.description = description;
        }

        private bool HasDescription => this.description != null;

        protected override IEnumerable<SectionBuilder> BuildSections(int lineLengthLimit)
        {
            if (!this.HasDescription)
            {
                yield break;
            }

            yield return new HeaderSection("Command description:");
            yield return new EmptySection(linesCount: 1);
            yield return new ParagraphBodySection(this.description);
        }
    }
}