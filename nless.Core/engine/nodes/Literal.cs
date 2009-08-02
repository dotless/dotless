using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nless.Core.engine.nodes
{
    public class Literal : Entity
    {
        protected Literal()
        {
        }

        internal string Value { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            return Value;
        }

        public Literal(string value)
        {
            Value = value;
        }
    }
}
