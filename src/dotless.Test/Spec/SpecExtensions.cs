using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace dotless.Test.Spec
{
    internal static class SpecExtensions
    {
        public static IEnumerable<Pair<T1, T2>> Zip<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second)
        {
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
            {
                yield return new Pair<T1, T2>(firstEnumerator.Current, secondEnumerator.Current);
            }
        }
    }

    // why is a generic Pair not defined in .NET?
    internal class Pair<T1, T2>
    {
        public T1 First { get; private set; }
        public T2 Second { get; private set; }

        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
}