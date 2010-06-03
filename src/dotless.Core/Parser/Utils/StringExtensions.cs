namespace dotless.Core.Parser.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        public static string Indent(this string str, int indent)
        {
            var space = new string(' ', indent);
            return space + str.Replace("\n", "\n" + space);
        }

        public static string JoinStrings(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source.ToArray());
        }
    }
}