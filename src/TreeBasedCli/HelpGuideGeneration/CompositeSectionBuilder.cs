using System.Collections.Generic;

namespace TreeBasedCli.HelpGuideGeneration
{
    internal abstract class CompositeSectionBuilder : SectionBuilder
    {
        protected abstract IEnumerable<SectionBuilder> BuildSections(int lineLengthLimit);

        public override IEnumerable<string> Build(int lineLengthLimit)
        {
            foreach (SectionBuilder section in this.BuildSections(lineLengthLimit))
            {
                foreach (string line in section.Build(lineLengthLimit))
                {
                    yield return line;
                }
            }
        }
    } 
}