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
        ///  The original path node
        /// </summary>
        protected Node OriginalPath { get; set; }

        public string Path { get; set; }

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
        ///  Whether it is a "once" import
        /// </summary>
        public bool IsOnce { get; set; }

        /// <summary>
        /// The action to perform with this node
        /// </summary>
        private ImportAction? _importAction;

        public Import(Quoted path, Value features, bool isOnce)
            : this((Node)path, features, isOnce)
        {
            OriginalPath = path;
        }

        public Import(Url path, Value features, bool isOnce)
            : this((Node)path, features, isOnce)
        {
            OriginalPath = path;
            Path = path.GetUnadjustedUrl();
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
            _importAction = ImportAction.LeaveImport;
        }

        private Import(Node path, Value features, bool isOnce)
        {
            if (path == null)
                throw new ParserException("Imports do not allow expressions");

            OriginalPath = path;
            Features = features;
            IsOnce = isOnce;
        }

        private ImportAction GetImportAction(IImporter importer)
        {
            if (!_importAction.HasValue)
            {
                _importAction = importer.Import(this);
            }

            return _importAction.Value;
        }

        public override void AppendCSS(Env env, Context context)
        {
            ImportAction action = GetImportAction(env.Parser.Importer);
            if (action == ImportAction.ImportNothing)
            {
                return;
            }

            if (action == ImportAction.ImportCss)
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

            if (_importAction == ImportAction.ImportLess)
            {
                InnerRoot = VisitAndReplace(InnerRoot, visitor);
            }
        }

        public override Node Evaluate(Env env)
        {
            OriginalPath = OriginalPath.Evaluate(env);
            var quoted = OriginalPath as Quoted;
            if (quoted != null)
            {
                Path = quoted.Value;
            }

            ImportAction action = GetImportAction(env.Parser.Importer);
            if (action == Importers.ImportAction.ImportNothing)
            {
                return new NodeList().ReducedFrom<NodeList>(this);
            }

            Node features = null;

            if (Features)
                features = Features.Evaluate(env);

            if (action == ImportAction.LeaveImport)
                return new Import(OriginalPath, features);

            if (action == ImportAction.ImportCss)
            {
                var importCss = new Import(OriginalPath, null) { _importAction = ImportAction.ImportCss, InnerContent = InnerContent };
                if (features)
                    return new Media(features, new NodeList() { importCss });
                return importCss;
            }

            using (env.Parser.Importer.BeginScope(this))
            {
                NodeHelper.ExpandNodes<Import>(env, InnerRoot.Rules);
            }

            var rulesList = new NodeList(InnerRoot.Rules).ReducedFrom<NodeList>(this);

            if (features)
            {
                return new Media(features, rulesList);
            }

            return rulesList;
        }
    }
}