namespace nless.Core.engine
{
    public class Operator : Entity
    {
        protected Operator()
        {
        }

        public Operator(string value) : base(value)
        {
        }

        public Operator(string value, INode parent) : base(value, parent)
        {
        }
    }
}