namespace dotless.Core
{
    using System.Collections.Generic;
    using Cache;

    public class CacheDecorator : ILessEngine
    {
        public readonly ILessEngine Underlying;
        public readonly ICache Cache;

        public CacheDecorator(ILessEngine underlying, ICache cache)
        {
            Underlying = underlying;
            Cache = cache;
        }

        public string TransformToCss(string source, string fileName)
        {
            if (!Cache.Exists(fileName))
            {
                var css = Underlying.TransformToCss(source, fileName);
                var imports = GetImports();
                Cache.Insert(fileName, imports, css);
                return css;
            }

            return Cache.Retrieve(fileName);
        }

        public IEnumerable<string> GetImports()
        {
            return Underlying.GetImports();
        }
    }
}