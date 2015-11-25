namespace dotless.Core.Parser.Infrastructure.Nodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BooleanNode : Node
    {
        public bool Value { get; set; }

        public BooleanNode(bool value)
        {
            Value = value;
        }

        protected override Node CloneCore() {
            return new BooleanNode(Value);
        }

        public override string ToString() {
            return Value.ToString().ToLowerInvariant();
        }
    }
}
