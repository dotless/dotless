using dotless.Core.engine.CssNodes;

namespace dotless.Core.engine
{
    public interface ICssDomPreprocessor
    {
        CssDocument Process(CssDocument document);
    }
}