namespace dotless.Core.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class StringExtensions
    {
        public static string Indent(this string str, int indent)
        {
            if(indent == 0) return str;

            var space = new string(' ', indent);
            return space + str.Replace("\n", "\n" + space);
        }

        public static string JoinStrings(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source.ToArray());
        }

        public static string AggregatePaths(this IEnumerable<string> source)
        {
            if (!source.Any())
                return "";
  
            var path = source.Aggregate("", Path.Combine);

            var dummyDrive = "C" + Path.VolumeSeparatorChar + Path.DirectorySeparatorChar;
            
            path = Path.GetFullPath(dummyDrive + path);

            return path.Substring(dummyDrive.Length).Replace(Path.DirectorySeparatorChar, '/');
        }
    }
}