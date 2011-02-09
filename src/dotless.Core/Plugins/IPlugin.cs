namespace dotless.Core.Plugins
{
    using Parser.Tree;

    public interface IPlugin
    {
        Ruleset Apply(Ruleset tree);
    }
}