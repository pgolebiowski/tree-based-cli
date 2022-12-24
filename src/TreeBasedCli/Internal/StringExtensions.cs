namespace TreeBasedCli.Internal
{
    internal static class StringExtensions
    {
        internal static string Escape(this string text, string toEscape)
            => text.Replace(toEscape, $"\\{toEscape}");

        internal static string Wrap(this string text, char toWrapWith)
            => text.Wrap(toWrapWith.ToString());

        internal static string Wrap(this string text, string toWrapWith)
            => toWrapWith + text + toWrapWith;
    }
}