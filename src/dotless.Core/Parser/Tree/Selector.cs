namespace dotless.Core.Parser.Tree
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
	using System.Collections.Generic;

    public class Selector : Node
    {
        private string _css;
        public NodeList<Element> Elements { get; set; }
		public NodeList<Comment> PreComments { get; set; }
		public NodeList<Comment> PostComments { get; set; }
		
        public Selector(NodeList<Element> elements)
        {
			Elements = elements;
			
			if (Elements[0].Combinator.Value == "")
     			Elements[0].Combinator.Value = " ";
        }

        public bool Match(Selector other)
        {
            return
                other.Elements.Count <= Elements.Count &&
                Elements[0].Value == other.Elements[0].Value;
        }

        public override string ToCSS(Env env)
        {
            if (!string.IsNullOrEmpty(_css))
                return _css;

			List<string> css = new List<string>();
		
			if  (PreComments) {
				css.AddRange(PreComments.Select(c => c.ToCSS(env)));
			}
			
			css.AddRange(Elements.Select(e => e.ToCSS(env)));
			
			if  (PostComments) {
				css.AddRange(PostComments.Select(c => c.ToCSS(env)));
			}

            return _css = css.JoinStrings("");
        }

        public override string ToString()
        {
            return ToCSS(new Env());
        }
    }
}