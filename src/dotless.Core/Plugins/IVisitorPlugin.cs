namespace dotless.Core.Plugins
{
    using Parser.Tree;

    public interface IVisitorPlugin : IPlugin
    {
        Ruleset Apply(Ruleset tree);

        VisitorPluginType AppliesTo { get; }
    }

    public enum VisitorPluginType
    {
        BeforeEvaluation,
        AfterEvaluation
    }
}