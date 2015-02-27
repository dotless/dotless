namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Collections.Generic;
	using System.Linq;
    using Plugins;

    public class Selector : Node
    {
        public NodeList<Element> Elements { get; set; }

        public Selector(IEnumerable<Element> elements)
        {
            if (elements is NodeList<Element>)
                Elements = elements as NodeList<Element>;
            else
                Elements = new NodeList<Element>(elements);
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
                if (element.NodeValue is Extend)
                {
                    if (env.MediaPath.Any())
                    {
                        env.MediaPath.Peek().AddExtension(this, (Extend)(((Extend)element.NodeValue).Evaluate(env)),env);
                    }
                    else //Global extend
                    {
                        env.AddExtension(this, (Extend)(((Extend)element.NodeValue).Evaluate(env)), env);
                    }
                }
                else
                {
                    evaldElements.Add(element.Evaluate(env) as Element);
                }
            }

            return new Selector(evaldElements).ReducedFrom<Selector>(this);
        }

        public override void AppendCSS(Env env)
        {
            env.Output.Push();

            if (Elements[0].Combinator.Value == "")
                env.Output.Append(' ');

            env.Output.Append(Elements);
            env.Output.Append(env.Output.Pop().ToString());
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