using System.Collections.Generic;

namespace TreeBasedCli.Internal
{
    internal abstract class SectionBuilder
    {
        public virtual bool IsLineLengthLimitEnough(int lineLengthLimit) => true;
        public abstract IEnumerable<string> Build(int lineLengthLimit);
    }
}