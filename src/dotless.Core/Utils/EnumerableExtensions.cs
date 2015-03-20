using System;
using System.Collections.Generic;

namespace dotless.Core.Utils
{
    internal static class EnumerableExtensions
    {
        internal static bool IsSubsequenceOf<TElement>(this IList<TElement> subsequence, IList<TElement> sequence)
        {
            return subsequence.IsSubsequenceOf(sequence, (element1, element2) => Equals(element1, element2));
        }

        internal static bool IsSubsequenceOf<TElement>(
            this IList<TElement> subsequence,
            IList<TElement> sequence, Func<TElement, TElement, bool> areEqual)
        {
            return subsequence.IsSubsequenceOf(sequence, (_, element1, __, element2) => areEqual(element1, element2));
        }

        /// <summary>
        /// Helper method for checking whether the elements in one IList are a subsequence of the elements in another.
        /// The parameters of the equality function are:
        /// - Index of the subsequence element within the subsequence
        /// - The subsequence element
        /// - Index of the parent sequence element within the parent sequence
        /// - The parent sequence element
        /// 
        /// This allows for equality comparisons where the conditions are different based on the position of each element.
        /// </summary>
        internal static bool IsSubsequenceOf<TElement>(
            this IList<TElement> subsequence,
            IList<TElement> sequence, Func<int, TElement, int, TElement, bool> areEqual)
        {
            // Trivial case: empty sequence is a subsequence of any sequence, including the empty sequence
            if (subsequence.Count == 0)
            {
                return true;
            }

            // Another trivial case: potential subsequence was not empty, but the sequence to test against is
            // so we know it can't contain the subsequence.
            if (sequence.Count == 0)
            {
                return false;
            }

            // Iterate seqEnumerator until we find an element that matches the first element of subEnumerator
            int sequenceIndex = 0;
            while (!areEqual(0, subsequence[0], sequenceIndex, sequence[sequenceIndex]))
            {
                sequenceIndex++;
                if (sequenceIndex >= sequence.Count)
                {
                    return false;
                }
            }

            int subsequenceIndex = 0;
            // Now, we've got two enumerators that are at a matching item
            while (true)
            {
                sequenceIndex += 1;
                subsequenceIndex += 1;

                if (subsequenceIndex >= subsequence.Count)
                {
                    // Ran out of subsequence to test against, so it's a match
                    return true;
                }

                if (sequenceIndex >= sequence.Count)
                {
                    // Ran out of sequence to test against, so it's not a match
                    return false;
                }

                if (!areEqual(subsequenceIndex, subsequence[subsequenceIndex], sequenceIndex, sequence[sequenceIndex]))
                {
                    // Current elements differ, so it's not a match
                    return false;
                }
            }
        }
    }
}