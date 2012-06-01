namespace dotless.Core.Plugins
{
    using Parser.Tree;
    using dotless.Core.Parser.Infrastructure;

    public interface IVisitorPlugin : IPlugin
    {
        Root Apply(Root tree);

        VisitorPluginType AppliesTo { get; }

        void OnPreVisiting(Env env);
        void OnPostVisiting(Env env);
    }

    public enum VisitorPluginType
    {
        BeforeEvaluation,
        AfterEvaluation
    }
}