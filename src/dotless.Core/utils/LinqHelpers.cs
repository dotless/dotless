using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotless.Core.utils
{
    public static class LinqHelpers
    {
        public static IEnumerable<T> Distinct<T, TProp>(this IEnumerable<T> source, Func<T, TProp> propertySelector)
        {
            return source.Distinct(new PropertyComparer<T, TProp>(propertySelector));
        }

        public class PropertyComparer<T, TProp> : IEqualityComparer<T>
        {
            public Func<T, TProp> PropertySelector { get; set; }

            public PropertyComparer(Func<T,TProp> propertySelector)
            {
                PropertySelector = propertySelector;
            }

            public bool Equals(T x, T y)
            {
                return Equals(PropertySelector(x), PropertySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return PropertySelector(obj).GetHashCode();
            }
        }

        public static string JoinStrings(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source.ToArray());
        }
    }
}
