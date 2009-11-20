using dotless.Core.engine.CssNodes;

namespace dotless.Core.engine.Pipeline
{
    public interface ICssBuilder
    {
        string ToCss(CssDocument document);
    }
}