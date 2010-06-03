using dotless.Tree;

namespace dotless.Infrastructure
{
  public interface IOperable
  {
    Node Operate(string op, Node other);
    Color ToColor();
  }
}