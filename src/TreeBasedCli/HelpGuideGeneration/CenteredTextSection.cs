using System.Collections.Generic;

namespace TreeBasedCli.HelpGuideGeneration
{
    internal class CenteredTextSection : SectionBuilder
    {
        private readonly string text;

        public CenteredTextSection(string text)
        {
            this.text = text;
        }

        public override IEnumerable<string> Build(int lineLengthLimit)
        {
            return TextFormatter.AlignTextToTheCenter(this.text, lineLengthLimit);
        }
    }
}