namespace dotless.Core.Parser.Tree
{
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Variable : Node
    {
        public string Name { get; set; }

        public Variable(string name)
        {
            Name = name;
        }

        public override Node Evaluate(Env env)
        {
            var name = Name;
            if (name.StartsWith("@@"))
            {
                var v = new Variable(name.Substring(1)).Evaluate(env);
                name = '@' + (v is TextNode ? (v as TextNode).Value : v.ToCSS(env));
            }

            var variable = env.FindVariable(name);

            if (variable)
                return variable.Value.Evaluate(env);

            throw new ParsingException("variable " + name + " is undefined", Index);
        }
    }
}