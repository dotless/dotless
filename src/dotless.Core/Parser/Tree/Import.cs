namespace dotless.Core.Parser.Tree
{
    using System.Text.RegularExpressions;
    using Importers;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using System.Collections.Generic;
    using System;

    public class Import : Directive
    {
        public Importer Importer { get; set; }
        public string Path { get; set; }
        protected Node OriginalPath { get; set; }
        protected bool Css { get; set; }
        public Ruleset InnerRoot { get; set; }

        public Import(Quoted path, Importer importer)
            : this(path.Value, importer)
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
            Path = path;

            if (path.EndsWith(".css"))
            {
                Css = true;
            } else
            {
                Css = !Importer.Import(this); // it is assumed to be css if it cannot be found as less

                if (Css && path.EndsWith(".less"))
                {
                    throw new Exception("You are importing a file ending in .less that cannot be found");
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