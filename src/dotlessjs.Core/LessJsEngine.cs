using System;
using dotless.Core;

namespace dotless
{
  public class LessJsEngine : ILessEngine
  {
    public string TransformToCss(LessSourceObject source)
    {
      var tree = new Parser().Parse(source.Content);

      return tree.ToCSS(null);
    }
  }
}