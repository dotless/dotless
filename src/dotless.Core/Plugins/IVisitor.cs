namespace dotless.Core.Plugins
{
    using Parser.Infrastructure.Nodes;

    public interface IVisitor
    {
        void Visit(Node node);
    }
}