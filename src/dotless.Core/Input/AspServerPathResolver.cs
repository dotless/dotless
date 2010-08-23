namespace dotless.Core.Input
{
    using Abstractions;

    class AspServerPathResolver : IPathResolver
    {
        public string GetFullPath(string path)
        {
            return System.Web.HttpContext.Current.Server.MapPath(path);
        }
    }
}