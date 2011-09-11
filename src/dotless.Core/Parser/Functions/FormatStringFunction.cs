namespace dotless.Core.Parser.Functions
{
    using System;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using dotless.Core.Exceptions;

    public class FormatStringFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            if (Arguments.Count == 0)
                return new Quoted("", false);

            Func<Node, string> unescape = n => n is Quoted ? ((Quoted) n).UnescapeContents() : n.ToCSS(env);

            var format = unescape(Arguments[0]);

            var args = Arguments.Skip(1).Select(unescape).ToArray();

            string result;

            try
            {
                result = string.Format(format, args);
            }
            catch (FormatException e)
            {
                throw new ParserException(string.Format("Error in formatString :{0}", e.Message), e);
            }

            return new Quoted(result, false);
        }
    }
}