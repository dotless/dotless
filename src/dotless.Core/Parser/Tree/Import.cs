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

        public Import(Quoted path, IImporter importer, Value features)
            : this(path.Value, importer, features)
        {
            OriginalPath = path;
        }

        public Import(Url path, IImporter importer, Value features)
            : this(path.GetUrl(), importer, features)
        {
            OriginalPath = path;
        }

        /// <summary>
        ///  Create a evaluated node that will render a @import
        /// </summary>
        /// <param name="originalPath"></param>
        /// <param name="features"></param>
        private Import(Node originalPath, Node features)
        {
            OriginalPath = originalPath;
            Features = features;
            Css = true;
        }

        private Import(string path, IImporter importer, Value features)
        {
            Importer = importer;
            Path = path;
            Features = features;

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
            env.Output.Append("@import ")
                .Append(OriginalPath.ToCSS(env));

            if (Features)
            {
                env.Output
                    .Append(" ")
                    .Append(Features);
            }
            env.Output.Append(";");

            if (!env.Compress)
            {
                env.Output.Append("\n");
            }
        }

        public override void Accept(Plugins.IVisitor visitor)
        {
            Features = VisitAndReplace(Features, visitor, true);

            if (!Css)
            {
                InnerRoot = VisitAndReplace(InnerRoot, visitor);
            }
        }

        public override Node Evaluate(Env env)
        {
            Node features = null;

            if (Features)
                features = Features.Evaluate(env);

            if (Css)
                return new Import(OriginalPath, features);

            NodeHelper.ExpandNodes<Import>(env, InnerRoot.Rules);

            var rulesList = new NodeList(InnerRoot.Rules).ReducedFrom<NodeList>(this);

            if (features)
            {
                return new Directive("@media", features, rulesList);
            }

            return rulesList;
        }
    }
}