using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Dimension : Node, IOperable
  {
    public double Value { get; set; }
    public string Unit { get; set; }

    public Dimension(string value, string unit)
    {
      Value = double.Parse(value);
      Unit = unit;
    }

    public Dimension(double value, string unit)
    {
      Value = value;
      Unit = unit;
    }

    public override string ToCSS(Env env)
    {
      var css = Value + Unit;
      return css;
    }
    

    // In an operation between two Dimensions,
    // we default to the first Dimension's unit,
    // so `1px + 2em` will yield `3px`.
    // In the future, we could implement some unit
    // conversions such that `100cm + 10mm` would yield
    // `101cm`.
    public Node Operate(string op, Node other)
    {
      var dim = (Dimension) other;

      var unit = Unit;
      if (string.IsNullOrEmpty(unit))
        unit = dim.Unit;
      else if(!string.IsNullOrEmpty(dim.Unit))
      {
        // convert units
      }

      return new Dimension(Operation.Operate(op, Value, dim.Value), unit);
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