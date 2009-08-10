using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace nless.Core.utils
{
    public static class RegexExtentions
    {
        public static bool IsIdent(this string str)
        {
            var rule = new Regex("^[.#]");
            return rule.Match(str).Success;
        }
    }
}
