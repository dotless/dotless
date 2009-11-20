namespace dotless.Core
{
    using Abstractions;

    public class AspCacheDecorator : ILessEngine
    {
        private readonly ILessEngine underlying;
        private readonly ICache cache;

        public AspCacheDecorator(ILessEngine underlying, ICache cache)
        {
            this.underlying = underlying;
            this.cache = cache;
        }

        public string TransformToCss(string filename)
        {
            if (!cache.Exists(filename))
            {
                string css = underlying.TransformToCss(filename);
                cache.Insert(filename, css);
            }
            return cache.Retrieve(filename);
        }
    }
}