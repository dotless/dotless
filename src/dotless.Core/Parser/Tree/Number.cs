namespace dotless.Core.Parser.Tree
{
    using System.Globalization;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Number : Node, IOperable
    {
        public double Value { get; set; }
        public string Unit { get; set; }

        public Number(string value, string unit)
        {
            Value = double.Parse(value, CultureInfo.InvariantCulture);
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

            return string.Format(CultureInfo.InvariantCulture, "{0:0.##}", Value);
        }

        public override void AppendCSS(Env env)
        {
            env.Output
                .Append(FormatValue())
                .Append(Unit);
        }


        // In an operation between two Dimensions,
        // we default to the first Number's unit,
        // so `1px + 2em` will yield `3px`.
        // In the future, we could implement some unit
        // conversions such that `100cm + 10mm` would yield
        // `101cm`.
        public Node Operate(Operation op, Node other)
        {
            Guard.ExpectNode<Number>(other, "right hand side of " + op.Operator, op.Index);

            var dim = (Number) other;

            var unit = Unit;
            var otherUnit = dim.Unit;

            if (unit == otherUnit && op.Operator == "/")
                unit = "";

            else if (string.IsNullOrEmpty(unit))
                unit = otherUnit;

            else if (!string.IsNullOrEmpty(otherUnit))
            {
                // convert units
            }

            return new Number(Operation.Operate(op.Operator, Value, dim.Value), unit)
                .ReducedFrom<Node>(this, other);
        }

        public Color ToColor()
        {
            return new Color(new[] {Value, Value, Value});
        }

        public double ToNumber()
        {
            return ToNumber(1d);
        }

        public double ToNumber(double max)
        {
            return Unit == "%" ? Value*max/100d : Value;
        }

        public static Number operator -(Number n)
        {
            return new Number(-n.Value, n.Unit);
        }
    }
}