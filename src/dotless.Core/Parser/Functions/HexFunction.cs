using dotless.Core.Exceptions;

namespace dotless.Core.Parser.Functions
{
	using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;

    public class HexFunction : NumberFunctionBase
    {
		protected override Node Eval(Env env, Number number, Node[] args)
		{
			if (string.IsNullOrEmpty(number.Unit))
				return new TextNode(((int)number.Value).ToString("X2"));

			throw new ParsingException(string.Format("Expected unitless number in function 'hex', found {0}", number.ToCSS(env)), number.Index);
		}
    }
}