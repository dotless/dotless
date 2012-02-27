namespace dotless.Core.Parser.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using dotless.Core.Utils;
    using dotless.Core.Parser.Infrastructure.Nodes;
    using dotless.Core.Parser.Tree;

    public abstract class IsFunction : Function
    {
        protected override Infrastructure.Nodes.Node Evaluate(Infrastructure.Env env)
        {
            Guard.ExpectNumArguments(1, Arguments.Count, this, Index);

            return new Keyword(IsEvaluator(Arguments[0]) ? "true" : "false");
        }

        protected abstract bool IsEvaluator(Node node);
    }

    public abstract class IsTypeFunction<T> : IsFunction where T:Node
    {
        protected override bool IsEvaluator(Node node)
        {
            return node is T;
        }
    }

    public class IsColorFunction : IsTypeFunction<Color>
    {
    }

    public class IsNumber : IsTypeFunction<Number>
    {
    }

    public class IsString : IsTypeFunction<Quoted>
    {
    }

    public class IsKeyword : IsTypeFunction<Keyword>
    {
    }

    public class IsUrl : IsTypeFunction<Url>
    {
    }

    public abstract class IsDimensionUnitFunction : IsTypeFunction<Number>
    {
        protected abstract string Unit { get; }

        protected override bool IsEvaluator(Node node)
        {
            bool isNumber = base.IsEvaluator(node);
            if (isNumber)
            {
                if (((Number)node).Unit == Unit)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class IsEm : IsDimensionUnitFunction
    {
        protected override string Unit
        {
            get
            {
                return "em";
            }
        }
    }

    public class IsPercentage : IsDimensionUnitFunction
    {
        protected override string Unit
        {
            get
            {
                return "%";
            }
        }
    }

    public class IsPixel : IsDimensionUnitFunction
    {
        protected override string Unit
        {
            get
            {
                return "px";
            }
        }
    }
}
