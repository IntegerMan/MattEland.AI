using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Shared.Collections
{
    /// <summary>
    /// Helper extension methods for working with collections or ranges.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Performs an operation on each member of the collection and returns the source collection
        /// </summary>
        /// <typeparam name="T">The type contained in the collection</typeparam>
        /// <param name="source">The collection</param>
        /// <param name="operation">The operation to perform on each member of the collection</param>
        /// <returns>The <paramref name="source"/> collection.</returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            if (source != null)
            {
                foreach (var item in source)
                {
                    operation(item);
                }
            }

            return source;
        }

        /// <summary>
        /// Performs an operation with each number from 0 to <paramref name="times"/>.
        /// </summary>
        /// <param name="times">The amount of times the operation should be invoked</param>
        /// <param name="operation">The operation to perform</param>
        public static void Each(this int times, Action<int> operation)
        {
            Enumerable.Range(0, times).Each(operation);
        }
    }
}