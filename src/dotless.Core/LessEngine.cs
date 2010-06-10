namespace dotless.Core
{
    public class LessEngine : ILessEngine
    {
        public string TransformToCss(string source)
        {
            var tree = new Parser.Parser().Parse(source);

            return tree.ToCSS();
        }
    }
}