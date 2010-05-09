using System.Globalization;
using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Number : Node, IOperable
  {
    public double Value { get; set; }
    public string Unit { get; set; }

    public Number(string value, string unit)
    {
      Value = double.Parse(value);
      Unit = unit;
    }

    public Number(double value, string unit)
    {
      Value = value;
      Unit = unit;
    }

    public Number(double value)
      : this(value, "")
    {
    }

    private string FormatValue()
    {
      if (Value == 0)
        return "0";

      return Value.ToString("0.##", CultureInfo.InvariantCulture);
    }
    public override string ToCSS()
    {
      return FormatValue() + Unit;
    }
    

    // In an operation between two Dimensions,
    // we default to the first Number's unit,
    // so `1px + 2em` will yield `3px`.
    // In the future, we could implement some unit
    // conversions such that `100cm + 10mm` would yield
    // `101cm`.
    public Node Operate(string op, Node other)
    {
      var dim = (Number) other;

      var unit = Unit;
      var otherUnit = dim.Unit;

      if (unit == otherUnit && op == "/")
        unit = "";

      else if (string.IsNullOrEmpty(unit))
        unit = otherUnit;
      
      else if(!string.IsNullOrEmpty(otherUnit))
      {
        // convert units
      }

      return new Number(Operation.Operate(op, Value, dim.Value), unit);
    }

    public Color ToColor()
    {
      return new Color(new[] { Value, Value, Value });
    }

    public double ToNumber()
    {
      return ToNumber(1d);
    }

    public double ToNumber(double max)
    {
      return Unit == "%" ? Value*max/100d : Value;
    }
  }
}