using dotless.Core.engine.CssNodes;

namespace dotless.Core.engine
{
    public interface ICssBuilder
    {
        string ToCss(CssDocument document);
    }
}