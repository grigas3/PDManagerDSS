using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDManager.Core.Common.Extensions
{
    /// <summary>
    /// Linq Extensions
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Contains All
        /// Checks whether a list contains all values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> values)
        {
            return values.All(value => source.Contains(value));
        }

    }
}
