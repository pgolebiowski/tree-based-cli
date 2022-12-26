using System;
using TreeBasedCli.Internal;

namespace TreeBasedCli.Exceptions
{
    internal partial class ThrowHelper
    {
        public static Exception Exception(params string[] text)
            => throw new Exception(text.Join());

        public static Exception TooNarrowConsole(int increaseWidthBy)
            => MessageOnlyException(
                $"The console is too narrow. Increase its width by {increaseWidthBy}.");

        public static Exception MessageOnlyException(params string[] text)
            => throw new MessageOnlyException(text.Join());

        public static Exception WrongCommandUsage(Command command, params string[] message)
            => throw new WrongCommandUsageException(command, message.Join());

        internal static Exception UnrecognizedType(object obj)
        {
            if (obj is Type type)
            {
            }
            else
            {
                type = obj.GetType();
            }

            throw new Exception($"Unrecognized type: '{type.FullName}'.");
        }
    }
}