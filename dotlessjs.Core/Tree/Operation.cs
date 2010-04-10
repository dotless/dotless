using System;
using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Operation : Node, IEvaluatable
  {
    public Node First { get; set; }
    public Node Second { get; set; }
    public string Operator { get; set; }

    public Operation(string op, Node first, Node second)
    {
      First = first;
      Second = second;
      Operator = op.Trim();
    }

    public override Node Evaluate(Env env)
    {
      var a = First.Evaluate(env);
      var b = Second.Evaluate(env);

      if (a is Number && b is Color) {
        if (Operator == "*" || Operator == "+")
        {
          var temp = b;
          b = a;
          a = temp;
        }
        else
          throw new InvalidOperationException("Can't substract or divide a color from a number");
      }

      var operable = a as IOperable;
      if (operable != null) 
        return operable.Operate(Operator, b);
      
      return null;
    }

    public static double Operate(string op, double first, double second)
    {
      switch (op)
      {
        case "+":
          return first + second;
        case "-":
          return first - second;
        case "*":
          return first * second;
        case "/":
          return first / second;
        default:
          throw new InvalidOperationException("Unknown operator");
      }
    }
  }
}