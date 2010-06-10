namespace dotless.Core.Parser.Tree
{
    using System.Text.RegularExpressions;
    using Importers;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Import : Directive
    {
        public Importer Importer { get; set; }
        public string Path { get; set; }
        protected Node OriginalPath { get; set; }
        protected bool Css { get; set; }
        public Ruleset InnerRoot { get; set; }

        public Import(Quoted path, Importer importer)
            : this(path.Contents, importer)
        {
            OriginalPath = path;
        }

        public Import(Url path, Importer importer)
            : this(path.GetUrl(), importer)
        {
            OriginalPath = path;
        }

        private Import(string path, Importer importer)
        {
            Importer = importer;
            var regex = new Regex(@"\.(le|c)ss$");

            Path = regex.IsMatch(path) ? path : path + ".less";

            Css = Path.EndsWith("css");

            if(!Css)
                Importer.Import(this);
        }

        protected override string ToCSS(Context context)
        {
            return base.ToCSS(); // should throw InvalidOperationException
        }

        public override Node Evaluate(Env env)
        {
            if (Css)
                return new NodeList(new TextNode("@import " + OriginalPath.ToCSS() + ";\n"));

            NodeHelper.ExpandNodes<Import>(env, InnerRoot);

            return new NodeList(InnerRoot.Rules);
        }
    }
}