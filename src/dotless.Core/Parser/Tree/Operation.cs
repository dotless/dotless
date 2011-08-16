namespace dotless.Core.Parser.Tree
{
    using System;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Operation : Node
    {
        public Node First { get; set; }
        public Node Second { get; set; }
        public string Operator { get; set; }

        public Operation(string op, Node first, Node second)
        {
            First = first;
            Second = second;
            Operator = op.Trim();
        }

        public override Node Evaluate(Env env)
        {
            var a = First.Evaluate(env);
            var b = Second.Evaluate(env);

            if (a is Number && b is Color)
            {
                if (Operator == "*" || Operator == "+")
                {
                    var temp = b;
                    b = a;
                    a = temp;
                }
                else
                    throw new ParsingException("Can't substract or divide a color from a number", Index);
            }

            try
            {
                var operable = a as IOperable;
                if (operable != null)
                    return operable.Operate(this, b).ReducedFrom<Node>(this);

                throw new ParsingException(string.Format("Cannot apply operator {0} to the left hand side: {1}", Operator, a.ToCSS(env)), Index);
            }
            catch (DivideByZeroException e)
            {
                throw new ParsingException(e, Index);
            }
            catch (InvalidOperationException e)
            {
                throw new ParsingException(e, Index);
            }
        }

        public static double Operate(string op, double first, double second)
        {
            if(op == "/" && second == 0)
                throw new DivideByZeroException();

            switch (op)
            {
                case "+":
                    return first + second;
                case "-":
                    return first - second;
                case "*":
                    return first * second;
                case "/":
                    return first / second;
                default:
                    throw new InvalidOperationException("Unknown operator");
            }
        }
    }
}