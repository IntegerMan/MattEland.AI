using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MattEland.Shared.Collections
{
    /// <summary>
    /// Contains extension methods related to Dictionary types
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Looks for a given key in the dictionary, and if present, will invoke a <paramref name="loadAction"/>
        /// with the value present in the dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of key the Dictionary expects</typeparam>
        /// <typeparam name="TValue">The type of value the Dictionary contains</typeparam>
        /// <param name="dictionary">The dictionary to inspect. This can be null.</param>
        /// <param name="key">The key to search for.</param>
        /// <param name="loadAction">
        /// The action that will be invoked with the given value, if one is present. This cannot be null.
        /// </param>
        /// <returns><c>true</c> if the key was present, otherwise <c>false</c>.</returns>
        public static bool TryLoad<TKey, TValue>(
            [CanBeNull] this IReadOnlyDictionary<TKey, TValue> dictionary, 
            [CanBeNull] TKey key,
            [NotNull] Action<TValue> loadAction)
        {
            if (loadAction == null) throw new ArgumentNullException(nameof(loadAction));

            if (dictionary == null || !dictionary.TryGetValue(key, out var value)) return false;

            loadAction(value);

            return true;
        }

    }
}