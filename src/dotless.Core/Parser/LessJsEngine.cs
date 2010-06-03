namespace dotless.Core.Parser
{
    using dotless.Core;

    public class LessJsEngine : ILessEngine
  {
    public string TransformToCss(LessSourceObject source)
    {
      var tree = new Parser().Parse(source.Content);

      return tree.ToCSS();
    }
  }
}