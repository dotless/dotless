namespace dotless.Core.Plugins
{
    using Parser.Tree;

    public interface IPlugin
    {
        Ruleset Apply(Ruleset tree);

        PluginType AppliesTo { get; }
    }

    public enum PluginType
    {
        BeforeEvaluation,
        AfterEvaluation
    }
}