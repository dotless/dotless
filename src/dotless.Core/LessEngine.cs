namespace dotless.Core
{
    public class LessEngine : ILessEngine
    {
        public string TransformToCss(LessSourceObject source)
        {
            var tree = new Parser.Parser().Parse(source.Content);

            return tree.ToCSS();
        }
    }
}