namespace dotless.Core.Parser.Tree
{
    using System.Globalization;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using System;

    public class Number : Node, IOperable, IComparable
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

        /// <summary>
        ///  Formats the value (no unit) based on precision.
        /// </summary>
        private string FormatValue()
        {
            return Value.ToString("0." +new string('#', GetPrecision()), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///  Gets the precision for the value based on the unit
        /// </summary>
        private int GetPrecision()
        {
            //TODO: It would be nice to look up which units have sensible precision.
            //      e.g. do sub pixels or sub points make sense?
            //      e.g. does it make sense for anything other than em to have more precision? Radians?
            //
            // For now, follow less.js and allow any number of decimals
            return 9;
        }

        protected override Node CloneCore() {
            return new Number(Value, Unit);
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
            Guard.ExpectNode<Number>(other, "right hand side of " + op.Operator, op.Location);

            var dim = (Number) other;

            var unit = Unit;
            var otherUnit = dim.Unit;

            if (string.IsNullOrEmpty(unit))
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
            return new Color(Value, Value, Value);
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

        public int CompareTo(object obj)
        {
            Number n = obj as Number;

            if (n)
            {
                if (n.Value > Value)
                {
                    return -1;
                }
                else if (n.Value < Value)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            return -1;
        }
    }
}