namespace dotless.Core.Parser.Tree
{
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Variable : Node
    {
        public string Name { get; set; }

        public Variable(string name)
        {
            Name = name;
        }

        public override Node Evaluate(Env env)
        {
            var variable = env.FindVariable(Name);

            if (variable)
                return variable.Value.Evaluate(env);

            throw new ParsingException("variable " + Name + " is undefined", Index);
        }
    }
}