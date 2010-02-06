using System;
using dotless.Core.exceptions;

namespace dotless.Core.engine.Functions
{
    public class AbsFunction : NumberFunctionBase
    {
        protected override INode Eval(Number number, INode[] args)
        {
            return new Number(number.Unit, Math.Abs(number.Value));
        }

        protected override string Name
        {
            get { return "abs"; }
        }
    }

    public class RoundFunction : NumberFunctionBase
    {
        protected override INode Eval(Number number, INode[] args)
        {
            return new Number(number.Unit, Math.Round(number.Value));
        }

        protected override string Name
        {
            get { return "round"; }
        }
    }

    public class FloorFunction : NumberFunctionBase
    {
        protected override INode Eval(Number number, INode[] args)
        {
            return new Number(number.Unit, Math.Floor(number.Value));
        }

        protected override string Name
        {
            get { return "floor"; }
        }
    }

    public class CeilFunction : NumberFunctionBase
    {
        protected override INode Eval(Number number, INode[] args)
        {
            return new Number(number.Unit, Math.Ceiling(number.Value));
        }

        protected override string Name
        {
            get { return "ceil"; }
        }
    }

    public class PercentageFunction : NumberFunctionBase
    {
        protected override INode Eval(Number number, INode[] args)
        {
            if (number.Unit == "%")
                return number;

            if (string.IsNullOrEmpty(number.Unit))
                return new Number("%", number.Value * 100);

            throw new ParsingException(string.Format("Expected unitless number in function '{0}', found {1}", Name, number.ToCss()));
        }

        protected override string Name
        {
            get { return "percentage"; }
        }
    }

    public class IncrementFunction : NumberFunctionBase
    {
        protected override INode Eval(Number number, INode[] args)
        {
            return new Number(number.Unit, number.Value + 1);
        }

        protected override string Name
        {
            get { return "increment"; }
        }
    }
}