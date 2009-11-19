using dotless.Core.engine.CssNodes;

namespace dotless.Core.engine
{
    public interface ILessToCssDomConverter
    {
        CssDocument BuildCssDocument(Element LessRootElement);
    }
}