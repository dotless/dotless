namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Collections.Generic;
    using Plugins;

    public class Selector : Node
    {
        private string _css;
        public NodeList<Element> Elements { get; set; }

        public Selector(IEnumerable<Element> elements)
        {
            if (elements is NodeList<Element>)
                Elements = elements as NodeList<Element>;
            else
                Elements = new NodeList<Element>(elements);

            if (Elements[0].Combinator.Value == "")
                Elements[0].Combinator.Value = " ";
        }

        public bool Match(Selector other)
        {
            return
                other.Elements.Count <= Elements.Count &&
                Elements[0].Value == other.Elements[0].Value;
        }

        public override Node Evaluate(Env env)
        {
            NodeList<Element> evaldElements = new NodeList<Element>();
            foreach (Element element in Elements)
            {
                evaldElements.Add(element.Evaluate(env) as Element);
            }

            return new Selector(evaldElements).ReducedFrom<Selector>(this);
        }

        public override void AppendCSS(Env env)
        {
            if (_css != null)
            {
                env.Output.Append(_css);
                return;
            }

            env.Output.Push();

            env.Output.Append(Elements);

            _css = env.Output.Pop().ToString();

            env.Output.Append(_css);
        }

        public override void Accept(IVisitor visitor)
        {
            Elements = VisitAndReplace(Elements, visitor);
        }

        public override string ToString()
        {
            return ToCSS(new Env());
        }
    }
}