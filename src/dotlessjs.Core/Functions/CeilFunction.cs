using System;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless.Functions
{
  public class CeilFunction : NumberFunctionBase
  {
    protected override Node Eval(Number number, Node[] args)
    {
      return new Number(Math.Ceiling(number.Value), number.Unit);
    }
  }
}