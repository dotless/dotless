namespace dotless.Core.Input
{
    using System.Web;
    using System.IO;

    public class AspRelativePathResolver : IPathResolver
    {
        public string GetFullPath(string path)
        {
            path = path.Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(HttpContext.Current.Request.PhysicalPath, path);
        }
    }
}