using System.Collections.Generic;

namespace TreeBasedCli.HelpGuideGeneration
{
    internal abstract class SectionBuilder
    {
        public virtual bool IsLineLengthLimitEnough(int lineLengthLimit) => true;
        public abstract IEnumerable<string> Build(int lineLengthLimit);
    }
}