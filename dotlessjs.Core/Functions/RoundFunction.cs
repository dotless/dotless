using System;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless.Functions
{
  public class RoundFunction : NumberFunctionBase
  {
    protected override Node Eval(Number number, Node[] args)
    {
      return new Number(Math.Round(number.Value), number.Unit);
    }
  }
}