namespace dotless.Core.Parser.Tree
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using System.Collections.Generic;
    using System.Text;

    public class Selector : Node
    {
        private List<StringBuilder> _css;
        public NodeList<Element> Elements { get; set; }
        public NodeList<Comment> PreComments { get; set; }
        public NodeList<Comment> PostComments { get; set; }

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

        public override StringBuilder ToCSS(Env env, StringBuilder output)
        {
            if (_css != null)
            {
                return output.AppendJoin(_css);
            }

            var css = new List<StringBuilder>();

            if (PreComments)
            {
                css.Add(PreComments.ToCSS<Comment>(env));
            }

            css.Add(Elements.ToCSS<Element>(env));

            if (PostComments)
            {
                css.Add(PostComments.ToCSS<Comment>(env));
            }

            return output.AppendJoin(_css = css);
        }

        public override string ToString()
        {
            return ToCSS(new Env());
        }
    }
}