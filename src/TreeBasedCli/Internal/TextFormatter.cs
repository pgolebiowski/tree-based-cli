using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeBasedCli.Internal
{
    public static class TextFormatter
    {
        public static IEnumerable<string> MergeColumnsLineByLine(
            IReadOnlyList<string> toBeOverriden,
            IReadOnlyList<string> overriding)
        {
            var minLength = Math.Min(toBeOverriden.Count, overriding.Count);
            var maxLength = Math.Max(toBeOverriden.Count, overriding.Count);
            var i = 0;

            for (; i < minLength; ++i)
            {
                var line = overriding[i] + toBeOverriden[i].Substring(overriding[i].Length);
                yield return line;
            }

            var longer = toBeOverriden;

            if (longer.Count < overriding.Count)
            {
                longer = overriding;
            }

            for (; i < maxLength; ++i)
            {
                var line = longer[i];
                yield return line;
            }
        }
        
        public static IEnumerable<string> Indent(string text, int indentSize, int lineLengthLimit)
        {
            var indentedTextLengthLimit = lineLengthLimit - indentSize;

            if (indentedTextLengthLimit <= 0)
                throw ThrowHelper.TooNarrowConsole(1 - indentedTextLengthLimit);

            var lines = WrapTextTryingNotToCutWords(text, indentedTextLengthLimit).ToArray();
            return lines.Select(x => new string(' ', indentSize) + x);
        }

        public static IReadOnlyCollection<string> AlignTextToTheCenter(string text, int lineLengthLimit)
        {
            if (text == null)
                throw new ArgumentNullException(text);

            if (lineLengthLimit <= 0)
                throw ThrowHelper.TooNarrowConsole(1 - lineLengthLimit);
            
            var wrapped = WrapTextTryingNotToCutWords(text, lineLengthLimit).ToArray();

            for (var i = 0; i < wrapped.Length; ++i)
            {
                var line = wrapped[i];
                var freeSpace = lineLengthLimit - line.Length;

                if (freeSpace >= 2)
                {
                    wrapped[i] = Indent(line, freeSpace / 2, lineLengthLimit).Join(Environment.NewLine);
                }
            }

            return wrapped;
        }

        public static IEnumerable<string> WrapTextTryingNotToCutWords(string text, int lineLengthLimit)
        {
            if (text == null)
                throw new ArgumentNullException(text);

            if (lineLengthLimit <= 0)
                throw ThrowHelper.TooNarrowConsole(1 - lineLengthLimit);

            var split = text.Split(new char[] { ' ' });
            var words = new Stack<string>(split.Reverse());
            var currentLine = new StringBuilder();

            bool isLineEmpty() => currentLine.Length == 0;
            string dumpLine()
            {
                var line = currentLine.ToString();
                currentLine.Clear();
                return line;
            }

            while (words.Any())
            {
                var next = words.Pop();

                if (next == "\n")
                {
                    yield return dumpLine();
                    continue;
                }

                if (next.Contains("\n"))
                {
                    var indexOf = next.IndexOf("\n");
                    var before = next.Substring(0, indexOf);

                    if (indexOf < next.Length - 1)
                    {
                        var after = next.Substring(indexOf + 1);
                        words.Push(after);
                    }

                    words.Push("\n");
                    words.Push(before);
                    continue;
                }

                var appendSpace = !isLineEmpty();
                var spaceLength = (appendSpace ? 1 : 0);
                var increasedLength = spaceLength + next.Length;

                // 1) the word can still fit the current line
                if (currentLine.Length + increasedLength <= lineLengthLimit)
                {
                    if (appendSpace)
                    {
                        currentLine.Append(" ");
                    }

                    currentLine.Append(next);

                    // it fits but there is no space left, so end of line
                    if (currentLine.Length == lineLengthLimit)
                    {
                        yield return dumpLine();
                    }
                    continue;
                }

                // 2) the word cannot fit the current line, but it also cannot fit
                //    a whole line. thus add whatever can be added now and push back
                //    the remainder.
                if (next.Length > lineLengthLimit)
                {
                    var leftSpace = lineLengthLimit - currentLine.Length - spaceLength;

                    if (leftSpace <= 0)
                    {
                        yield return dumpLine();
                        words.Push(next);
                        continue;
                    }

                    var toPush = next.Substring(0, leftSpace);
                    var remainder = next.Substring(leftSpace);

                    if (appendSpace)
                    {
                        currentLine.Append(" ");
                    }

                    currentLine.Append(toPush);
                    words.Push(remainder);

                    continue;
                }

                // 3) the word cannot fit the current line, but it can fit in the
                //    next line. thus just push it there
                yield return dumpLine();
                currentLine.Append(next);
            }

            // there is something not yielded
            if (!isLineEmpty())
            {
                yield return dumpLine();
            }
        }
    }
}