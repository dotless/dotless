namespace dotless.Core.Parser.Functions
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public abstract class ColorFunctionBase : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectMinArguments(1, Arguments.Count(), this, Index);
            Guard.ExpectNode<Color>(Arguments[0], this, Arguments[0].Index);

            var color = Arguments[0] as Color;

            if (Arguments.Count == 2)
            {
                Guard.ExpectNode<Number>(Arguments[1], this, Arguments[1].Index);

                var number = Arguments[1] as Number;
                var edit = EditColor(color, number);

                if (edit != null)
                    return edit;
            }

            return Eval(color);
        }

        protected abstract Node Eval(Color color);

        protected virtual Node EditColor(Color color, Number number)
        {
            return null;
        }
    }
}