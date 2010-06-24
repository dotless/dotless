namespace dotless.Core.Parser.Infrastructure
{
    using Nodes;
    using Tree;

    public interface IOperable
    {
        Node Operate(Operation op, Node other);
        Color ToColor();
    }
}