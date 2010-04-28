using System;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless.Functions
{
  public class AbsFunction : NumberFunctionBase
  {
    protected override Node Eval(Number number, Node[] args)
    {
      return new Number(Math.Abs(number.Value), number.Unit);
    }
  }
}