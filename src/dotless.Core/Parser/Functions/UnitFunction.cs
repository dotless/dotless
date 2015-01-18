namespace dotless.Core.Parser.Functions
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class UnitFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectMaxArguments(2, Arguments.Count, this, Location);
            Guard.ExpectNode<Number>(Arguments[0], this, Location);

            var number = Arguments[0] as Number;

            var unit = string.Empty;
            if (Arguments.Count == 2)
            {
                if (Arguments[1] is Keyword)
                {
                    unit = ((Keyword)Arguments[1]).Value;
                }
                else
                {
                    unit = Arguments[1].ToCSS(env);
                }
            }

            return new Number(number.Value, unit);
        }
    }
}
