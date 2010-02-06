using System;

namespace dotless.Core.utils
{
  public class NumberExtensions
  {
    public static decimal Normalize(decimal value)
    {
      return Normalize(value, 0m);
    }
    public static decimal Normalize(decimal value, decimal min)
    {
      return Normalize(value, min, 1m);
    }
    public static decimal Normalize(decimal value, decimal min, decimal max)
    {
      return Math.Min(Math.Max(value, min), max);
    }

    public static double Normalize(double value)
    {
      return Normalize(value, 0d);
    }
    public static double Normalize(double value, double min)
    {
      return Normalize(value, min, 1d);
    }
    public static double Normalize(double value, double min, double max)
    {
      return Math.Min(Math.Max(value, min), max);
    }
  }
}