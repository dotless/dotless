namespace dotless.Core
{
    using System.Collections.Generic;
    using Yahoo.Yui.Compressor;

    public class MinifierDecorator : ILessEngine
    {
        public ILessEngine Engine { get; private set; }

        public MinifierDecorator(ILessEngine engine)
        {
            Engine = engine;
        }

        public string TransformToCss(string source, string fileName)
        {
            var css = Engine.TransformToCss(source, fileName);
            return CssCompressor.Compress(css);
        }

        public IEnumerable<string> GetImports()
        {
            return Engine.GetImports();
        }
    }
}