namespace dotless.Core.utils
{
    using System.Text.RegularExpressions;

    public static class RegexExtentions
    {
        public static bool IsIdent(this string str)
        {
            var rule = new Regex("^[.#]");
            return rule.Match(str).Success;
        }
    }
}