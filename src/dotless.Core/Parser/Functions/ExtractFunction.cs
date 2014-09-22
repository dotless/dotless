using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;
using dotless.Core.Parser.Tree;
using dotless.Core.Utils;

namespace dotless.Core.Parser.Functions
{
    public class ExtractFunction : ListFunctionBase
    {
        protected override Node Eval(Env env, Node[] list, Node[] args)
        {
            Guard.ExpectNumArguments(1, args.Length, this, Location);
            Guard.ExpectNode<Number>(args[0], this, args[0].Location);

            var index = (int)(args[0] as Number).Value;

            // Extract function indecies are 1-based
            return list[index-1];
        }
    }
}
