namespace dotless.Core.Input
{
    using System.Web;

    class AspServerPathResolver : IPathResolver
    {
        public string GetFullPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}