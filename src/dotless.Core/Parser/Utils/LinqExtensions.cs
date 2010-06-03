namespace dotless.Core.Parser.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtensions
    {
        public static TResult SelectFirst<TSource, TResult>(this IEnumerable<TSource> source,
                                                            Func<TSource, bool> predicate,
                                                            Func<TSource, TResult> resultSelector)
            where TSource : class
            where TResult : class
        {
            var first = source.FirstOrDefault(predicate);

            return first != null ? resultSelector(first) : null;
        }

        public static TResult SelectFirst<TSource, TResult>(this IEnumerable<TSource> source,
                                                            Func<TSource, TResult> resultSelector)
            where TResult : class
        {
            return source.Select(resultSelector).Where(result => result != null).FirstOrDefault();
        }
    }
}