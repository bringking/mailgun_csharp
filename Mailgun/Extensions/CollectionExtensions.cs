using System.Collections.Generic;

namespace Mailgun.Extensions
{
    /// <summary>
    /// Extensions to the ICollection interface
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds a value to a collection of key value pairs only if both the key and the are not null and not empty
        /// </summary>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddIfNotNullOrEmpty(this ICollection<KeyValuePair<string, string>> items, string key,
            string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                items.Add(new KeyValuePair<string, string>(key, value));
            }
        }
        /// <summary>
        /// Generic add extension method for adding typed Key Value pairs into collection with a friendly method
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="items"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Add<TK, TV>(this ICollection<KeyValuePair<TK, TV>> items, TK key, TV value)
        {
            items.Add(new KeyValuePair<TK, TV>(key, value));
        }
    }
}