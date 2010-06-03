namespace dotless.Core.Parser.Tree
{
    using Infrastructure.Nodes;

    public class Element : Node
    {
        public Combinator Combinator { get; set; }
        public string Value { get; set; }

        public Element(Combinator combinator, string value)
        {
            Combinator = combinator ?? new Combinator("");
            Value = value.Trim();
        }

        public override string ToCSS()
        {
            return Combinator.ToCSS() + Value;
        }
    }
}