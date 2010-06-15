namespace dotless.Core.Parser.Tree
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Call : Node
    {
        public string Name { get; set; }
        public NodeList<Expression> Arguments { get; set; }
        private Node Evaluated { get; set; }

        public Call(string name, NodeList<Expression> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        protected Call()
        {
        }

        public override Node Evaluate(Env env)
        {
            if (Evaluated != null)
                return Evaluated;

            var args = Arguments.Select(a => a.Evaluate(env));

            if (env != null)
            {
                var function = env.GetFunction(Name);

                if (function != null)
                {
                    function.Name = Name;
                    Evaluated = function.Call(env, args);
                    return Evaluated;
                }
            }

            Evaluated = new TextNode(Name + "(" + Arguments.Select(a => a.Evaluate(env).ToCSS(env)).JoinStrings(", ") + ")");
            return Evaluated;
        }
    }
}