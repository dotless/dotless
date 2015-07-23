using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;

namespace dotless.Core.Parser.Functions
{
    public class DefaultFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            return new TextNode("default()");
        }
    }
}
