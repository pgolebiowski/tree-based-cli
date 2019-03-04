using System.Collections.Generic;

namespace TreeBasedCli.Internal
{
    internal abstract class CompositeSectionBuilder : SectionBuilder
    {
        protected abstract IEnumerable<SectionBuilder> BuildSections(int lineLengthLimit);

        public override IEnumerable<string> Build(int lineLengthLimit)
        {
            foreach (var section in this.BuildSections(lineLengthLimit))
            {
                foreach (var line in section.Build(lineLengthLimit))
                {
                    yield return line;
                }
            }
        }
    } 
}