using System.Collections.Generic;

namespace TreeBasedCli.HelpGuideGeneration
{
    internal class NameAndVersionSection : CompositeSectionBuilder
    {
        private readonly string name;
        private readonly string version;

        public NameAndVersionSection(string name, string version)
        {
            this.name = name;
            this.version = version;
        }

        private bool HasName => this.name != null;
        private bool HasVersion => this.version != null;

        protected override IEnumerable<SectionBuilder> BuildSections(int lineLengthLimit)
        {
            if (!this.HasName && !this.HasVersion)
            {
                yield break;
            }

            if (this.HasName)
            {
                yield return new CenteredTextSection(this.name);
            }

            if (this.HasVersion)
            {
                yield return new CenteredTextSection(this.version);
            }

            yield return new EmptySection(linesCount: 2);
        }
    }
}