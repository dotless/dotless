namespace dotless.Core.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class StringExtensions
    {
        public static string JoinStrings(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source.ToArray());
        }

        public static string AggregatePaths(this IEnumerable<string> source)
        {
            if (!source.Any())
                return "";
  
            var path = source.Aggregate("", Path.Combine);

            return CanonicalizePath(path);
        }

        /// <summary>
        /// Splits the given path into segments and resolves any parent path references
        /// it can (eg. "foo/../bar" becomes "bar" whereas "../foo" is left as-is).
        /// </summary>
        private static string CanonicalizePath(string path)
        {
            var pathStack = new Stack<string>();

            // Split assuming we might get any combination of backward and forward slashes
            string[] segments = path.Split('\\', '/');
            foreach (string segment in segments)
            {
                // If the parent reference is the first one on the stack, do nothing 
                // (because there's nothing we can do, and removing it would break the path)
                if (segment.Equals("..") && pathStack.Count > 0 && pathStack.Peek() != "..")
                    pathStack.Pop();
                else
                    pathStack.Push(segment);
            }

            // Recombine the path segments. Note that there is a difference between doing this
            // and pathStack.Reverse().Aggregate("", Path.Combine), which would discard empty path
            // segments (and therefore strip leading slashes)
            return string.Join("/", pathStack.Reverse().ToArray());
        }
    }
}