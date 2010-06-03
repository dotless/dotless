namespace dotless.Core.Parser.Infrastructure
{
    using Nodes;
    using Tree;

    public interface IOperable
    {
        Node Operate(string op, Node other);
        Color ToColor();
    }
}