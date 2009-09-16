namespace nless.Core.engine
{
    public class Anonymous : Literal
    {
        protected Anonymous()
        {
        }

        public Anonymous(string value) : base(value)
        {
        }

        public Anonymous(string value, INode parent) : base(value, parent)
        {
        }
    }
}