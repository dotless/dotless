using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;

namespace dotless.Core.Parser.Tree {
    public class Attribute : Node
    {
        public Node Name { get; set; }
        public Node Op { get; set; }
        public Node Value { get; set; }

        public Attribute(Node name, Node op, Node value)
        {
            Name = name;
            Op = op;
            Value = value;
        }

        public override Node Evaluate(Env env)
        {
            return new TextNode(string.Format("[{0}{1}{2}]", 
                Name.Evaluate(env).ToCSS(env),
                Op == null ? "" : Op.Evaluate(env).ToCSS(env), 
                Value == null ? "" :  Value.Evaluate(env).ToCSS(env)));
        }
    }
}
