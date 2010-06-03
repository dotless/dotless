using System;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless.Functions
{
  public class FloorFunction : NumberFunctionBase
  {
    protected override Node Eval(Number number, Node[] args)
    {
      return new Number(Math.Floor(number.Value), number.Unit);
    }
  }
}