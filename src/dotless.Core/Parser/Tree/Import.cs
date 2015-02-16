using System;
using System.Linq;

namespace dotless.Core.Parser.Tree
{
    using System.IO;
    using Importers;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using dotless.Core.Exceptions;

    public class Import : Directive
    {
        /// <summary>
        ///  The importer to use to import the 
        /// </summary>
        public IImporter Importer { get; set; }

        /// <summary>
        ///  The path to this import
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///  The original path node
        /// </summary>
        protected Node OriginalPath { get; set; }

        /// <summary>
        ///  The inner root - if the action is ImportLess
        /// </summary>
        public Ruleset InnerRoot { get; set; }

        /// <summary>
        ///  The inner content - if the action is ImportCss
        /// </summary>
        public string InnerContent { get; set; }

        /// <summary>
        ///  The media features (if present)
        /// </summary>
        public Node Features { get; set; }

        /// <summary>
        ///  The type of import: reference, inline, less, css, once, multiple or optional
        /// </summary>
        public ImportOptions ImportOptions { get; set; }

        /// <summary>
        /// The action to perform with this node
        /// </summary>
        protected ImportAction ImportAction { get; set; }

        public Import(Quoted path, IImporter importer, Value features, ImportOptions option)
            : this(path.Value, importer, features, option)
        {
            OriginalPath = path;
        }

        public Import(Url path, IImporter importer, Value features, ImportOptions option)
            : this(path.GetUnadjustedUrl(), importer, features, option)
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
            ImportAction = ImportAction.LeaveImport;
        }

        private Import(string path, IImporter importer, Value features, ImportOptions option)
        {
            if (path == null)
                throw new ParserException("Imports do not allow expressions");

            Importer = importer;
            Path = path;
            Features = features;
            ImportOptions = option;

            ImportAction = Importer.Import(this); // it is assumed to be css if it cannot be found as less
        }

        public override void AppendCSS(Env env, Context context)
        {
            if (ImportAction == ImportAction.ImportNothing)
            {
                return;
            }

            if (ImportAction == ImportAction.ImportCss)
            {
                env.Output.Append(InnerContent);
                return;
            }

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

            if (ImportAction == ImportAction.ImportLess)
            {
                InnerRoot = VisitAndReplace(InnerRoot, visitor);
            }
        }

        public override Node Evaluate(Env env)
        {
            if (ImportAction == Importers.ImportAction.ImportNothing)
            {
                return new NodeList().ReducedFrom<NodeList>(this);
            }

            Node features = null;

            if (Features)
                features = Features.Evaluate(env);

            if (ImportAction == ImportAction.LeaveImport)
                return new Import(OriginalPath, features);

            if (ImportAction == ImportAction.ImportCss)
            {
                var importCss = new Import(OriginalPath, null) { ImportAction = ImportAction.ImportCss, InnerContent = InnerContent };
                if (features)
                    return new Media(features, new NodeList() { importCss });
                return importCss;
            }

            NodeHelper.ExpandNodes<Import>(env, InnerRoot.Rules);

            var rulesList = new NodeList(InnerRoot.Rules).ReducedFrom<NodeList>(this);
            if (IsOptionSet(ImportOptions, ImportOptions.Reference))
            {
                foreach (var ruleSet in rulesList.OfType<Ruleset>())
                {
                    ruleSet.IsReference = true;
                }
            }

            if (features)
            {
                return new Media(features, rulesList);
            }

            return rulesList;
        }
        private bool IsOptionSet(ImportOptions options, ImportOptions test)
        {
            return (options & test) == test;
        }
    }

    [Flags]
    public enum ImportOptions {
        Once = 1,
        Multiple = 2,
        Optional = 4,
        Css = 8,
        Less = 16,
        Inline = 32,
        Reference = 64 
    }
}