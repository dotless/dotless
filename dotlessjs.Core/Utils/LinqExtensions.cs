using System;
using System.Collections.Generic;
using System.Linq;

namespace dotless.Utils
{
  public static class LinqExtensions
  {
    public static string JoinStrings(this IEnumerable<string> source, string separator)
    {
      return string.Join(separator, source.ToArray());
    }

    public static TResult SelectFirst<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> resultSelector)
      where TSource : class 
      where TResult : class
    {
      var first = source.FirstOrDefault(predicate);

      return first != null ? resultSelector(first) : null;
    }

    public static TResult SelectFirst<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> resultSelector) 
      where TResult : class
    {
      return source.Select(resultSelector).Where(result => result != null).FirstOrDefault();
    }
  }
}