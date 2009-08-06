namespace nless.Core.engine
{
    public class Literal : Entity
    {
        protected Literal()
        {
        }

        public Literal(string value) : base(value)
        {
        }

        public Literal(string value, INode parent) : base(value, parent)
        {
        }

        public string Unit { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}