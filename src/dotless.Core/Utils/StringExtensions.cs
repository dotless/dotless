namespace dotless.Core.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Parser.Infrastructure;
    using Parser.Infrastructure.Nodes;
    using System;

    public static class StringExtensions
    {
        public static string JoinStrings(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source.ToArray());
        }

        public static string AggregatePaths(this IEnumerable<string> source, string currentDirectory)
        {
            if (!source.Any())
                return "";
  
            var path = source.Aggregate("", Path.Combine);

            return CanonicalizePath(path, currentDirectory);
        }

        /// <summary>
        /// Splits the given path into segments and resolves any parent path references
        /// it can (eg. "foo/../bar" becomes "bar" whereas "../foo" is left as-is).
        /// </summary>
        private static string CanonicalizePath(string path, string currentDirectory)
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

            IEnumerable<string> pathList = pathStack.Reverse().ToList();

            // if the imported file is outside the path of the first file then
            // path re-writing won't work.. so we can check to see if we can further
            // reduce the path. e.g.
            //
            // base/css/a.less
            //   @import "../../theme/b.less";
            // theme/b.less
            //   url("../base/file.png")
            //
            // then we end up with ../../base/file.png
            // which we re-write as ../file.png
            if (pathList.First().Equals(".."))
            {
                // get the total number of parent segments (../) in the path list
                var numberOfParents = pathList.TakeWhile(segment => segment.Equals("..")).Count();
                // get the relevant part of the current directory that the ../ goes down too
                var currentPathList = currentDirectory
                    .Split('\\', '/')
                    .Reverse()
                    .Take(numberOfParents)
                    .Reverse();
                // now see how many match going outwards
                int numberOfMatchingParents = 0, i = numberOfParents;

                foreach (var currentPathSegment in currentPathList)
                {
                    if (i < pathList.Count() && string.Equals(currentPathSegment, pathList.ElementAt(i++), StringComparison.InvariantCultureIgnoreCase))
                    {
                        numberOfMatchingParents++;
                    }
                    else
                    {
                        // once a higher up element of the path doesn't match it is invalid to look any deeper
                        break;
                    }
                }

                // skip out the ../ that match directories we are already in
                pathList = pathList.Take(numberOfParents - numberOfMatchingParents)
                    .Concat(pathList.Skip((numberOfParents - numberOfMatchingParents) + (numberOfMatchingParents*2)));
            }

            // Recombine the path segments. Note that there is a difference between doing this
            // and pathStack.Reverse().Aggregate("", Path.Combine), which would discard empty path
            // segments (and therefore strip leading slashes)
            return string.Join("/", pathList.ToArray());
        }
    }
}