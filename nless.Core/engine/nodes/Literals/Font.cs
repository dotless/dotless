using System.Text;
using System.Linq;

namespace nless.Core.engine
{
    public class Font : Literal
    {
        public Font(string value) : base(value)
        {
        }
    }
    public class Keyword : Literal
    {
        public Keyword(string value) : base(value)
        {
        }
        public override string Inspect()
        {
            return string.Format("#{0}", this);
        }
    }
    public class FontFamily : Literal
    {
        internal Literal[] Family { get; set; }

        public FontFamily(params string[] family)
            : this(family.Select(f => new Literal(f)).ToArray())
        {
        }

        public FontFamily(params Literal[] family)
        {
            Family = family;
        }

        public override string ToCss()
        {
            var sb = new StringBuilder();
            foreach (var family in Family)
                sb.AppendFormat("{0},", family.ToCss());
            return sb.ToString(0, sb.Length - 1);
        }
    }
}
