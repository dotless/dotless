namespace dotless.Core.Parser.Tree
{
    using System.IO;
    using Importers;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Import : Directive
    {
        public IImporter Importer { get; set; }
        public string Path { get; set; }
        protected Node OriginalPath { get; set; }
        protected bool Css { get; set; }
        public Ruleset InnerRoot { get; set; }

        public Import(Quoted path, IImporter importer)
            : this(path.Value, importer)
        {
            OriginalPath = path;
        }

        public Import(Url path, IImporter importer)
            : this(path.GetUrl(), importer)
        {
            OriginalPath = path;
        }

        private Import(string path, IImporter importer)
        {
            Importer = importer;
            Path = path;

            if (path.EndsWith(".css"))
            {
                Css = true;
            } else
            {
                Css = !Importer.Import(this); // it is assumed to be css if it cannot be found as less

                if (Css && path.EndsWith(".less"))
                {
                    throw new FileNotFoundException("You are importing a file ending in .less that cannot be found.", path);
                }
            }
        }

        protected override void AppendCSS(Env env, Context context)
        {
             base.AppendCSS(env); // should throw InvalidOperationException
        }

        public override Node Evaluate(Env env)
        {
            if (Css)
                return new NodeList(new TextNode("@import " + OriginalPath.ToCSS(env) + ";\n"));

            NodeHelper.ExpandNodes<Import>(env, InnerRoot.Rules);

            return new NodeList(InnerRoot.Rules).ReducedFrom<NodeList>(this);
        }
    }
}