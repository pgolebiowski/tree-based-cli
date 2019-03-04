namespace TreeBasedCli.Internal
{
    public static class StringExtensions
    {
        public static string Escape(this string text, string toEscape)
            => text.Replace(toEscape, $"\\{toEscape}");

        public static string Wrap(this string text, char toWrapWith)
            => text.Wrap(toWrapWith.ToString());

        public static string Wrap(this string text, string toWrapWith)
            => toWrapWith + text + toWrapWith;
    }
}