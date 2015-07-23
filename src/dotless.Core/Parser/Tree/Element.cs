namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Plugins;

    public class Element : Node
    {
        public Combinator Combinator { get; set; }
        public string Value { get; set; }
        public Node NodeValue { get; set; }

        public Element(Combinator combinator, string textValue) : this(combinator)
        {
            Value = textValue.Trim();
        }

        public Element(Combinator combinator, Node value) : this(combinator)
        {
            TextNode textValue = value as TextNode;
            if (textValue != null && !(value is Quoted))
            {
                Value = textValue.Value.Trim();
            }
            else
            {
                NodeValue = value;
            }
        }

        private Element(Combinator combinator)
        {
            Combinator = combinator ?? new Combinator("");
        }

        public override Node Evaluate(Env env)
        {
            if (NodeValue != null)
            {
                var newNodeValue = NodeValue.Evaluate(env);

                return new Element(Combinator, newNodeValue)
                    .ReducedFrom<Element>(this);
            }
            else
                return this;
        }

        public override void AppendCSS(Env env)
        {
            env.Output
                .Append(Combinator)
                .Push();

            if (NodeValue != null)
            {
                env.Output.Append(NodeValue)
                    .Trim();
            }
            else
            {
                env.Output.Append(Value);
            }
            
            env.Output
                .PopAndAppend();
        }

        public override void Accept(IVisitor visitor)
        {
            Combinator = VisitAndReplace(Combinator, visitor);

            NodeValue = VisitAndReplace(NodeValue, visitor, true);
        }

        internal Element Clone() {
            return new Element(Combinator) { Value = Value, NodeValue = NodeValue};
        }
    }
}