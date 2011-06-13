namespace dotless.Core.Parser.Functions
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class EFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectMaxArguments(1, Arguments.Count, this, Index);

            if (Arguments.Count == 0)
                return new TextNode("");

            var str = Arguments[0];
            if (str is Quoted)
                return new TextNode((str as Quoted).UnescapeContents());

            return new TextNode(str.ToCSS(env));
        }
    }

    public class CFormatString : Function
    {
        protected override Node Evaluate(Env env)
        {
            if (Arguments.Count == 0)
                return new Quoted("", false);

            Func<Node, string> stringValue = n => n is Quoted ? ((Quoted)n).Value : n.ToCSS(env);

            var str = stringValue(Arguments[0]);

            var args = Arguments.Skip(1).ToArray();
            var i = 0;

            MatchEvaluator replacement = m =>
                                             {
                                                 var value = (m.Value == "%s") ?
                                                                stringValue(args[i++]) :
                                                                args[i++].ToCSS(env);

                                                 return char.IsUpper(m.Value[1]) ?
                                                     HttpUtility.UrlEncode(value) :
                                                     value;
                                             };

            str = Regex.Replace(str, "%[sda]", replacement, RegexOptions.IgnoreCase);

            var quote = Arguments[0] is Quoted ? (Arguments[0] as Quoted).Quote : null;

            return new Quoted(str, quote);
        }
    }
}