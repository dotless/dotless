using System.Web.Optimization;

namespace dotless.Bundling
{
    public class LessBundle : StyleBundle
    {
        public LessBundle(string virtualPath) : base(virtualPath)
        {
            AddTransform();
        }

        public LessBundle(string virtualPath, string cdnPath) : base(virtualPath, cdnPath)
        {
            AddTransform();
        }

        private void AddTransform()
        {
            Transforms.Add(new LessTranform());
        }
    }
}
