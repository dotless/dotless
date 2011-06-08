namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Collections.Generic;

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

        public override string ToString()
        {
            return ToCSS(new Env());
        }
    }
}