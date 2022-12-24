using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeBasedCli.Internal
{
    internal static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
            => !collection.Any();

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
            => collection == null || collection.IsEmpty();

        public static string Join(this IEnumerable<string> collection, string separator = "")
            => string.Join(separator, collection);

        public static void ForEach<T>(this IEnumerable<T> collection, Action<IterationInfo<T>> action)
        {
            using (var enumerator = collection.GetEnumerator())
            {
                var thereAreMoreElements = enumerator.MoveNext();
                var index = 0;

                while (thereAreMoreElements)
                {
                    var current = enumerator.Current;
                    thereAreMoreElements = enumerator.MoveNext();

                    var iteration = new IterationInfo<T>(
                        current,
                        index,
                        isFirst: index == 0,
                        isLast: !thereAreMoreElements);

                    action.Invoke(iteration);
                }
            }
        }

        public static IReadOnlyCollection<T> InjectBetweenAdjacentElements<T>(
            this IEnumerable<T> collection,
            T elementToInject,
            int numberOfElementsToInject = 1)
        {
            if (numberOfElementsToInject < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "The number of elements to inject cannot be a negative number.");
            }

            var result = new List<T>();

            collection.ForEach(x =>
            {
                result.Add(x.CurrentElement);

                if (!x.IsLastIteration)
                {
                    for (var i = 0; i < numberOfElementsToInject; ++i)
                    {
                        result.Add(elementToInject);
                    }
                }
            });

            return result;
        }
    }
}