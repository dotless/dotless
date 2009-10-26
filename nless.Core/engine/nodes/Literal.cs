namespace dotless.Core.engine
{
    public class Literal : Entity
    {
        public string Unit { get; set; }

        protected Literal()
        {
        }

        public Literal(string value) : base(value)
        {
        }

        public Literal(string value, INode parent) : base(value, parent)
        {
        }

        public override string ToString()
        {
            return Value;
        }
    }
}