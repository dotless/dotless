using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;
using dotless.Core.Parser.Tree;

namespace dotless.Core.Parser.Functions
{
    public class LengthFunction : ListFunctionBase
    {
        protected override Node Eval(Env env, Node[] list, Node[] args)
        {
            return new Number(list.Length);
        }
    }
}
